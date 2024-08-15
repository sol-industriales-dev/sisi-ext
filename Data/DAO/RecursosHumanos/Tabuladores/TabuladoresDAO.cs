using Core.DAO.Maquinaria.Reporte;
using Core.DAO.RecursosHumanos.Tabuladores;
using Core.DTO;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos.Tabuladores;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad.Nomina;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using Core.Entity.Administrativo.RecursosHumanos.Tabuladores;
using Core.Entity.Enkontrol.Compras.OrdenCompra;
using Core.Entity.Enkontrol.Compras.Requisicion;
using Core.Entity.Principal.Alertas;
using Core.Entity.Principal.Multiempresa;
using Core.Entity.Principal.Usuarios;
using Core.Entity.RecursosHumanos.Reclutamientos;
using Core.Enum;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Alertas;
using Core.Enum.Principal.Bitacoras;
using Core.Enum.RecursosHumanos.Reclutamientos;
using Core.Enum.RecursosHumanos.Tabuladores;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Maquinaria.Reporte;
using Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Reportes.Reports.RecursosHumanos.Tabuladores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Data.DAO.RecursosHumanos.Tabuladores
{
    public class TabuladoresDAO : GenericDAO<tblP_Usuario>, ITabuladoresDAO
    {
        #region INIT
        private readonly Dictionary<string, object> resultado = new Dictionary<string, object>();
        private const string _NOMBRE_CONTROLADOR = "TabuladoresController";
        private const int _SISTEMA = (int)SistemasEnum.RH;
        private const string _CORREO_DIANA_ALVAREZ = "diana.alvarez@construplan.com.mx";
        private const string _CORREO_MANUEL_CRUZ = "m.cruz@construplan.com.mx";
        private const string _CORREO_OMAR_NUNEZ = "omar.nunez@construplan.com.mx";
        private const string _CORREO_ALEXANDRA_GOMEZ = "alexandra.gomez@construplan.com.mx";
        private const int _USUARIO_OMAR_NUNEZ_ID = 7939;
        private const int _USUARIO_GERARDO_REINA_ID = 1164;
        private const int _PERFIL_ID_ADMINISTRADOR = 1;
        #endregion

        #region OTROS DAOS
        IEncabezadoDAO encabezadoFactoryServices = new EncabezadoFactoryServices().getEncabezadoServices();
        #endregion

        #region LINEA DE NEGOCIOS
        public Dictionary<string, object> GetLineaNegocios()
        {
            resultado.Clear();
            try
            {
                #region SE OBTIENE LISTADO DE LINEA DE NEGOCIOS
                List<LineaNegocioDTO> lstLineaNegociosDTO = GetListaLineasNegocios().OrderBy(o => o.concepto).ToList();

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("lstLineaNegocios", lstLineaNegociosDTO);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CELineaNegocio(LineaNegocioDTO objFiltroDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado.Clear();
                try
                {
                    #region VALIDACIONES
                    if (string.IsNullOrEmpty(objFiltroDTO.concepto)) { throw new Exception("Es necesario indicar la línea de negocio."); }
                    if (string.IsNullOrEmpty(objFiltroDTO.abreviacion)) { throw new Exception("Es necesario indicar la abreviación."); }
                    #endregion

                    tblRH_TAB_CatLineaNegocio objCE = _context.tblRH_TAB_CatLineaNegocio.Where(w => w.id == objFiltroDTO.id && w.registroActivo).FirstOrDefault();
                    if (objCE == null)
                    {
                        #region NUEVO REGISTRO
                        objCE = new tblRH_TAB_CatLineaNegocio();
                        objCE.concepto = objFiltroDTO.concepto.Trim();
                        objCE.abreviacion = objFiltroDTO.abreviacion.Trim();
                        objCE.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaCreacion = DateTime.Now;
                        objCE.registroActivo = true;
                        _context.tblRH_TAB_CatLineaNegocio.Add(objCE);
                        _context.SaveChanges();
                        #endregion
                    }
                    else
                    {
                        #region ACTUALIZAR REGISTRO
                        objCE.concepto = objFiltroDTO.concepto.Trim();
                        objCE.abreviacion = objFiltroDTO.abreviacion.Trim();
                        objCE.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();
                        #endregion
                    }

                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, objFiltroDTO.id <= 0 ? "Se ha registrado con éxito." : "Se ha actualizado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, objFiltroDTO.id <= 0 ? (int)AccionEnum.AGREGAR : (int)AccionEnum.ACTUALIZAR, objFiltroDTO.id, JsonUtils.convertNetObjectToJson(objCE));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "CELineaNegocio", e, objFiltroDTO.id <= 0 ? AccionEnum.AGREGAR : AccionEnum.ACTUALIZAR, objFiltroDTO.id, objFiltroDTO);
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarLineaNegocio(LineaNegocioDTO objFiltroDTO)
        {
            resultado.Clear();
            try
            {
                #region VALIDACIONES
                string mensajeError = "Ocurrió un error al obtener la información de la línea de negocio.";
                if (objFiltroDTO.id <= 0) { throw new Exception(mensajeError); }
                #endregion

                #region SE OBTIENE LA INFORMACIÓN DE LA LINEA DE NEGOCIO PARA ACTUALIZAR
                //tblRH_TAB_CatLineaNegocio objLineaNegocio = _context.tblRH_TAB_CatLineaNegocio.Where(w => w.id == objFiltroDTO.id && w.registroActivo).FirstOrDefault();
                LineaNegocioDTO objLineaNegocioDTO = GetListaLineasNegocios(new LineaNegocioDTO { id = objFiltroDTO.id }).FirstOrDefault();
                if (objLineaNegocioDTO == null)
                    throw new Exception(mensajeError);

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("objLineaNegocio", objLineaNegocioDTO);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objFiltroDTO.id, objFiltroDTO);
                resultado.Clear();
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarLineaNegocio(LineaNegocioDTO objFiltroDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado.Clear();
                try
                {
                    #region VALIDACIONES
                    const string MENSAJE_ERROR_GENERAL = "Ocurrió un error al eliminar el registro.";
                    if (objFiltroDTO.id <= 0) { throw new Exception(MENSAJE_ERROR_GENERAL); }

                    List<TabuladorDetDTO> lstTabuladoresDetDTO = GetListaTabuladoresDet(new TabuladorDetDTO { FK_LineaNegocio = objFiltroDTO.id }).ToList();
                    if (lstTabuladoresDetDTO.Count() > 0)
                        throw new Exception("No se puede eliminar la linea de negocio, ya que se encuentra en uso.");
                    #endregion

                    #region SE ELIMINA LA LINEA DE NEGOCIO SELECCIONADA
                    tblRH_TAB_CatLineaNegocio objEliminar = _context.tblRH_TAB_CatLineaNegocio.Where(w => w.id == objFiltroDTO.id && w.registroActivo).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception(MENSAJE_ERROR_GENERAL);

                    objEliminar.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEliminar.fechaModificacion = DateTime.Now;
                    objEliminar.registroActivo = false;
                    _context.SaveChanges();

                    #region SE ELIMINA EL DETALLE DE LA LINEA DE NEGOCIO
                    List<tblRH_TAB_CatLineaNegocioDet> lstEliminarDet = _context.tblRH_TAB_CatLineaNegocioDet.Where(w => w.FK_LineaNegocio == objFiltroDTO.id && w.registroActivo).ToList();
                    foreach (var item in lstEliminarDet)
                    {
                        tblRH_TAB_CatLineaNegocioDet objEliminarDet = lstEliminarDet.Where(w => w.id == item.id).FirstOrDefault();
                        objEliminarDet.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objEliminarDet.fechaModificacion = DateTime.Now;
                        objEliminarDet.registroActivo = false;
                        _context.SaveChanges();
                    }
                    #endregion
                    #endregion

                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, objFiltroDTO.id, JsonUtils.convertNetObjectToJson(objEliminar));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "EliminarLineaNegocio", e, AccionEnum.ELIMINAR, objFiltroDTO.id, objFiltroDTO);
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        #region LINEA DE NEGOCIOS REL CC
        public Dictionary<string, object> GetLineaNegociosRelCC(LineaNegocioDetDTO objFiltroDTO)
        {
            resultado.Clear();
            try
            {
                #region VALIDACIONES
                if (objFiltroDTO.FK_LineaNegocio <= 0) { throw new Exception("Ocurrió un error al obtener el listado de CC."); }
                #endregion

                #region SE OBTIENE LISTADO DE CC RELACIONADO A LA LINEA DE NEGOCIO
                List<LineaNegocioDetDTO> lstCC = _context.Select<LineaNegocioDetDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT t1.id, t2.cc, t2.ccDescripcion AS ccDescripcion, t2.estatus AS registroActivoCC
	                                FROM tblRH_TAB_CatLineaNegocioDet AS t1
	                                INNER JOIN tblC_Nom_CatalogoCC AS t2 ON t2.cc = t1.cc
		                                WHERE t1.FK_LineaNegocio = @FK_LineaNegocio AND t1.registroActivo = @registroActivo
			                                ORDER BY t2.cc",
                    parametros = new { FK_LineaNegocio = objFiltroDTO.FK_LineaNegocio, registroActivo = true }
                }).ToList();

                List<LineaNegocioDetDTO> lstLineaNegocioDetDTO = new List<LineaNegocioDetDTO>();
                LineaNegocioDetDTO objLineaNegocioDetDTO = new LineaNegocioDetDTO();
                foreach (var item in lstCC)
                {
                    if (!string.IsNullOrEmpty(item.cc) && !string.IsNullOrEmpty(item.ccDescripcion))
                    {
                        objLineaNegocioDetDTO = new LineaNegocioDetDTO();
                        objLineaNegocioDetDTO.id = item.id;
                        objLineaNegocioDetDTO.ccDescripcion = string.Format("[{0}] {1}", item.cc.Trim(), item.ccDescripcion.Trim());
                        objLineaNegocioDetDTO.registroActivoCC = item.registroActivoCC;
                        lstLineaNegocioDetDTO.Add(objLineaNegocioDetDTO);
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("lstLineaNegocioDetDTO", lstLineaNegocioDetDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetLineaNegociosRelCC", e, AccionEnum.CONSULTA, objFiltroDTO.id, objFiltroDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CELineaNegocioRelCC(LineaNegocioDetDTO objFiltroDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado.Clear();
                try
                {
                    #region VALIDACIONES
                    if (objFiltroDTO.FK_LineaNegocio <= 0) { throw new Exception("Ocurrió un error al crear la relación."); }
                    if (objFiltroDTO.lstCC == null) { throw new Exception("Es necesario seleccionar al menos un CC."); }

                    // SE VERIFICA SI EL CC YA SE ENCUENTRA EN OTRA LINEA DE NEGOCIO.
                    List<tblC_Nom_CatalogoCC> lstCC = _context.tblC_Nom_CatalogoCC.Where(w => w.estatus).ToList();
                    foreach (var item in objFiltroDTO.lstCC)
                    {
                        tblRH_TAB_CatLineaNegocioDet objCatLineaNegocioDet = _context.tblRH_TAB_CatLineaNegocioDet.Where(w => w.cc == item && w.registroActivo).FirstOrDefault();
                        if (objCatLineaNegocioDet != null)
                        {
                            tblC_Nom_CatalogoCC objCC = lstCC.Where(w => w.cc == item).FirstOrDefault();
                            if (objCC == null)
                                throw new Exception("Ocurrió un error al obtener la información del CC.");

                            string mensajeError = string.Format("El CC [{0}] {1}, ya se encuentra en otra linea de negocio.", objCC.cc, objCC.ccDescripcion.Trim());
                            throw new Exception(mensajeError);
                        }
                    }
                    #endregion

                    #region SE CREA RELACIÓN DE LA LINEA DE NEGOCIO CON EL CC SELECCIONADO
                    List<tblRH_TAB_CatLineaNegocioDet> lstCE = new List<tblRH_TAB_CatLineaNegocioDet>();
                    tblRH_TAB_CatLineaNegocioDet objCE = new tblRH_TAB_CatLineaNegocioDet();
                    foreach (var item in objFiltroDTO.lstCC)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            objCE = new tblRH_TAB_CatLineaNegocioDet();
                            objCE.FK_LineaNegocio = objFiltroDTO.FK_LineaNegocio;
                            objCE.cc = item.Trim();
                            objCE.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objCE.fechaCreacion = DateTime.Now;
                            objCE.registroActivo = true;
                            lstCE.Add(objCE);
                        }
                    }
                    _context.tblRH_TAB_CatLineaNegocioDet.AddRange(lstCE);
                    _context.SaveChanges();
                    #endregion

                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha registrado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, objFiltroDTO.id, JsonUtils.convertNetObjectToJson(objFiltroDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "CELineaNegocioRelCC", e, AccionEnum.CONSULTA, objFiltroDTO.id, objFiltroDTO);
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarLineaNegocioRelCC(LineaNegocioDetDTO objFiltroDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado.Clear();
                try
                {
                    #region VALIDACIONES
                    string mensajeError = "Ocurrió un error al eliminar el registro seleccionado.";
                    if (objFiltroDTO.id <= 0) { throw new Exception(mensajeError); }

                    // SE VERIFICA SI EL CC YA SE ENCUENTRA ASIGNADO EN UNA PLANTILLA
                    int FK_LineaNegocio = _context.tblRH_TAB_CatLineaNegocioDet.Where(w => w.id == objFiltroDTO.id && w.registroActivo).Select(s => s.FK_LineaNegocio).FirstOrDefault();
                    List<tblRH_TAB_PlantillasPersonal> lstPlantillas = _context.tblRH_TAB_PlantillasPersonal.Where(w => w.FK_LineaNegocio == FK_LineaNegocio && w.registroActivo).ToList();
                    if (lstPlantillas.Count() > 0)
                        throw new Exception("No se puede eliminar el CC, ya que se encuentra asignado en una plantilla en uso.");

                    // SE VERIFICA SI EL CC YA SE ENCUENTRA ASIGNADO EN UN TABULADOR
                    List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.FK_LineaNegocio == FK_LineaNegocio && w.registroActivo).ToList();
                    if (lstTabuladoresDet.Count() > 0)
                        throw new Exception("No se puede elimianr el CC, ya que se encuentra asignado en un tabulador en uso.");
                    #endregion

                    #region SE ELIMINA LA RELACIÓN DEL CC CON LA LINEA DEL NEGOCIO
                    tblRH_TAB_CatLineaNegocioDet objEliminar = _context.tblRH_TAB_CatLineaNegocioDet.Where(w => w.id == objFiltroDTO.id && w.registroActivo).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception(mensajeError);

                    objEliminar.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEliminar.fechaModificacion = DateTime.Now;
                    objEliminar.registroActivo = false;
                    _context.SaveChanges();

                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, objFiltroDTO.id, JsonUtils.convertNetObjectToJson(objFiltroDTO));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                    resultado.Clear();
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboCCDisponibles(LineaNegocioDetDTO objFiltroDTO)
        {
            resultado.Clear();
            try
            {
                #region VALIDACIONES
                if (objFiltroDTO.FK_LineaNegocio <= 0) { throw new Exception("Ocurrió un error al obtener el listado de CC disponibles."); }
                #endregion

                #region FILL COMBO CC DISPONIBLES PARA LA LINEA DE NEGOCIO SELECCIONADA
                // LISTADO DE CC
                List<tblC_Nom_CatalogoCC> lstCC = _context.tblC_Nom_CatalogoCC.Where(w => w.cc != "0" && w.estatus).ToList();

                // LISTADO DE CC NO DISPONIBLES
                List<string> lstCCNoDisponibles = _context.tblRH_TAB_CatLineaNegocioDet.Where(w => w.registroActivo).Select(s => s.cc).ToList();

                // LISTADO DE CC DISPONIBLES
                lstCC = lstCC.Where(w => !lstCCNoDisponibles.Contains(w.cc)).OrderBy(o => o.cc).ToList();

                List<ComboDTO> lstCCDTO = new List<ComboDTO>();
                ComboDTO objCCDTO = new ComboDTO();
                foreach (var item in lstCC)
                {
                    tblC_Nom_CatalogoCC objCC = lstCC.Where(w => w.cc == item.cc).FirstOrDefault();
                    if (objCC != null)
                    {
                        if (!string.IsNullOrEmpty(objCC.cc) && !string.IsNullOrEmpty(objCC.ccDescripcion))
                        {
                            objCCDTO = new ComboDTO();
                            objCCDTO.Value = objCC.cc.Trim();
                            objCCDTO.Text = string.Format("[{0}] {1}", objCC.cc.Trim(), objCC.ccDescripcion.Trim());
                            lstCCDTO.Add(objCCDTO);
                        }
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstCCDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboCCDisponibles", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        #endregion
        #endregion

        #region ASIGNACIÓN TABULADORES
        public Dictionary<string, object> InitCEAsignacionTabulador()
        {
            resultado.Clear();
            try
            {
                #region SE OBTIENE LISTADO DE LINEA DE NEGOCIO
                List<tblRH_TAB_CatLineaNegocio> lstLineaNegocios = _context.tblRH_TAB_CatLineaNegocio.Where(w => w.registroActivo).OrderBy(o => o.concepto).ToList();

                string objHTML = string.Empty;
                foreach (var item in lstLineaNegocios)
                {
                    if (item.id > 0 && !string.IsNullOrEmpty(item.concepto))
                        objHTML += string.Format("<div class='col-lg-12'><label><input type='checkbox' class='chkCE_LineaNegocio' id='chkCE_LineaNegocio_{0}'>&nbsp;{1}</label></div>", item.id, item.concepto.Trim());
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("objHTML", objHTML);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "InitCEAsignacionTabulador", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Clear();
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetAsignacionTabuladores(TabuladorDetDTO objFiltroDTO)
        {
            resultado.Clear();
            try
            {
                #region SE OBTIENE LISTADO DE ASIGNACION TABULADORES
                List<TabuladorDetDTO> lstTabuladores = _context.Select<TabuladorDetDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT t1.id, t2.descripcion AS puestoDesc, t2.puesto AS FK_Puesto
	                                    FROM tblRH_TAB_Tabuladores AS t1
	                                    INNER JOIN tblRH_EK_Puestos AS t2 ON t2.puesto = t1.FK_Puesto
		                                    WHERE t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND t1.tabuladorAutorizado = @tabuladorAutorizado
			                                    ORDER BY t2.descripcion",
                    parametros = new { registroActivo = true, tabuladorAutorizado = EstatusGestionAutorizacionEnum.AUTORIZADO }
                }).ToList();

                List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.registroActivo).ToList();
                foreach (var item in lstTabuladores)
                {
                    // SE VERIFICA CUANTOS TABULADORES TIENE REGISTRADO EL PUESTO
                    item.cantTabuladores = lstTabuladoresDet.Where(w => w.FK_Tabulador == item.id && w.registroActivo).Count();

                    // SE CONCATENA ID CON EL PUESTO
                    if (item.FK_Puesto > 0 && !string.IsNullOrEmpty(item.puestoDesc))
                        item.puestoDesc = string.Format("[{0}] {1}", item.FK_Puesto, item.puestoDesc.Trim());
                }

                lstTabuladores = lstTabuladores.OrderBy(o => o.puestoDesc).ToList();

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("lstTabuladores", lstTabuladores);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetAsignacionTabuladores", e, AccionEnum.CONSULTA, objFiltroDTO.id, objFiltroDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CrearAsignacionTabulador(TabuladorDTO objTabuladorDTO, List<TabuladorDetDTO> lstTabuladoresDTO, List<GestionAutorizanteDTO> lstGestionAutorizantesDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado.Clear();
                try
                {
                    #region VALIDACIONES
                    if (objTabuladorDTO.FK_Puesto <= 0) { throw new Exception("Es necesario seleccionar el puesto."); }
                    foreach (var item in lstTabuladoresDTO)
                    {
                        if (item.FK_LineaNegocio <= 0) { throw new Exception("Ocurrió un error al registrar el tabulador."); }
                        if (item.FK_Categoria <= 0) { throw new Exception("Es necesario seleccionar una categoría."); }
                        if (item.sueldoBase <= 0) { throw new Exception("Es necesario indicar el sueldo base."); }

                        tblRH_TAB_CatEsquemaPago objEsquemaPago = _context.tblRH_TAB_CatEsquemaPago.Where(w => w.id == item.FK_EsquemaPago && w.registroActivo).FirstOrDefault();
                        if (objEsquemaPago == null) { throw new Exception("Ocurrió un error al verificar el esquema de pago."); }

                        string[] splitEsquemaPago = objEsquemaPago.concepto.Split('/');
                        if (splitEsquemaPago.Length == 2)
                            if (item.complemento <= 0) { throw new Exception("Es necesario indicar el complemento."); }

                        if (item.totalNominal <= 0) { throw new Exception("Es necesario indicar el total nominal."); }
                        if (item.sueldoMensual <= 0) { throw new Exception("Es necesario indicar el sueldo mensual."); }
                        if (item.FK_EsquemaPago <= 0) { throw new Exception("Es necesario seleccionar el esquema de pago."); }
                        if (item.FK_Categoria <= 0) { throw new Exception("Ocurrió un error al obtener la categoría."); }
                    }

                    bool tieneAutorizante = false;
                    foreach (var item in lstGestionAutorizantesDTO)
                    {
                        if (item.FK_UsuarioAutorizacion > 0)
                            tieneAutorizante = true;
                    }
                    if (!tieneAutorizante) { throw new Exception("Es necesario indicar al menos un autorizante."); }

                    // SE VERIFICA SI EL PUESTO CON LAS CATEGORIAS SELECCIONADAS, NO SE ENCUENTREN EN USO.
                    List<tblRH_TAB_CatCategorias> lstCategorias = _context.tblRH_TAB_CatCategorias.Where(w => w.registroActivo).ToList();
                    List<tblRH_TAB_CatLineaNegocio> lstLineaNegocio = _context.tblRH_TAB_CatLineaNegocio.Where(w => w.registroActivo).ToList();

                    foreach (var item in lstTabuladoresDTO)
                    {
                        int FK_Puesto = objTabuladorDTO.FK_Puesto;
                        int FK_LineaNegocio = item.FK_LineaNegocio;
                        int FK_Categoria = item.FK_Categoria;
                        int FK_Tabulador = _context.tblRH_TAB_Tabuladores.Where(w => w.FK_Puesto == FK_Puesto && w.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo).Select(s => s.id).FirstOrDefault();
                        bool tabuladorEnUso = false;
                        if (FK_Tabulador > 0)
                        {
                            int cantTabDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.FK_Tabulador == FK_Tabulador && w.FK_LineaNegocio == FK_LineaNegocio && w.FK_Categoria == FK_Categoria && w.registroActivo).Count();
                            if (cantTabDet > 0)
                                tabuladorEnUso = true;

                            tblRH_TAB_CatCategorias objCategoria = lstCategorias.Where(w => w.id == FK_Categoria && w.registroActivo).FirstOrDefault();
                            tblRH_TAB_CatLineaNegocio objLineaNegocio = lstLineaNegocio.Where(w => w.id == FK_LineaNegocio && w.registroActivo).FirstOrDefault();

                            if (objCategoria == null) { throw new Exception("Ocurrió un error al obtener la categoría."); }
                            if (objLineaNegocio == null) { throw new Exception("Ocurrió un error al obtener la línea de negocio."); }

                            if (tabuladorEnUso && objCategoria != null && objLineaNegocio != null)
                            {
                                string mensajeError = string.Format("Ya se encuentra en uso la categoría [{0}] de la línea de negocio [{1}], favor de seleccionar otra categoría.",
                                    !string.IsNullOrEmpty(objCategoria.concepto) ? objCategoria.concepto.Trim() : string.Empty,
                                    !string.IsNullOrEmpty(objLineaNegocio.concepto) ? objLineaNegocio.concepto.Trim() : string.Empty);
                                throw new Exception(mensajeError);
                            }
                        }
                    }
                    #endregion

                    if (objTabuladorDTO.FK_Tabulador == 0)
                        objTabuladorDTO.FK_Tabulador = _context.tblRH_TAB_Tabuladores.Where(w => w.FK_Puesto == objTabuladorDTO.FK_Puesto && w.registroActivo).Select(s => s.id).FirstOrDefault();

                    tblRH_TAB_Tabuladores objCE = _context.tblRH_TAB_Tabuladores.Where(w => w.id == objTabuladorDTO.FK_Tabulador && w.registroActivo).FirstOrDefault();
                    if (objCE == null)
                    {
                        #region NUEVO REGISTRO
                        #region REGISTRO PRINCIPAL
                        objCE = new tblRH_TAB_Tabuladores();
                        objCE.FK_Puesto = objTabuladorDTO.FK_Puesto;
                        objCE.tabuladorAutorizado = (int)EstatusGestionAutorizacionEnum.PENDIENTE;
                        objCE.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaCreacion = DateTime.Now;
                        objCE.registroActivo = true;
                        _context.tblRH_TAB_Tabuladores.Add(objCE);
                        _context.SaveChanges();

                        // SE OBTIENE LA LLAVE PRIMARIA QUE SE ACABA DE REGISTRAR
                        int FK_Tabulador = _context.tblRH_TAB_Tabuladores.Where(w => w.registroActivo).OrderByDescending(o => o.id).Select(s => s.id).FirstOrDefault();
                        if (FK_Tabulador <= 0)
                            throw new Exception("Ocurrió un error al registar el tabulador.");
                        #endregion

                        #region SE REGISTRA EL DETALLE DEL TABULADOR
                        List<tblRH_TAB_TabuladoresDet> lstCE_Det = new List<tblRH_TAB_TabuladoresDet>();
                        tblRH_TAB_TabuladoresDet objCE_Det = new tblRH_TAB_TabuladoresDet();
                        foreach (var item in lstTabuladoresDTO)
                        {
                            objCE_Det = new tblRH_TAB_TabuladoresDet();
                            objCE_Det.FK_Tabulador = FK_Tabulador;
                            objCE_Det.FK_LineaNegocio = item.FK_LineaNegocio;
                            objCE_Det.FK_Categoria = item.FK_Categoria;
                            objCE_Det.sueldoBase = item.sueldoBase;
                            objCE_Det.complemento = item.complemento;
                            objCE_Det.totalNominal = item.totalNominal;
                            objCE_Det.sueldoMensual = item.sueldoMensual;
                            objCE_Det.FK_EsquemaPago = item.FK_EsquemaPago;
                            objCE_Det.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objCE_Det.fechaCreacion = DateTime.Now;
                            objCE_Det.registroActivo = true;
                            lstCE_Det.Add(objCE_Det);
                        }
                        _context.tblRH_TAB_TabuladoresDet.AddRange(lstCE_Det);
                        _context.SaveChanges();
                        #endregion

                        #region SE CONSTRUYE CUERPO DEL CORREO
                        List<tblRH_TAB_CatEsquemaPago> lstEsquemaPago = _context.tblRH_TAB_CatEsquemaPago.Where(e => e.registroActivo).ToList();
                        tblRH_EK_Puestos objPuesto = _context.tblRH_EK_Puestos.Where(e => e.registroActivo && e.puesto == objTabuladorDTO.FK_Puesto).FirstOrDefault();

                        string cuerpo =
                                        @"<html>
                                            <head>
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
                                                        background-color: #ffcc5c;
                                                    }
                                                </style>
                                            </head>
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>

                                                    <p class=MsoNormal>
                                                        Buen día,<br><br>Se ha registrado un nuevo tabulador, se encuentra listo para su gestión.<br><br>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                        "Puesto: [" + objPuesto.puesto + "] " + objPuesto.descripcion + @".<o:p></o:p>
                                                    </p>
                                                    <br><br><br>
                                            ";
                        var lstGrpTabuladoresDet = lstTabuladoresDTO.GroupBy(e => e.FK_LineaNegocio).ToList();

                        foreach (var item in lstGrpTabuladoresDet)
                        {
                            cuerpo += @"
                                        <table>
                                            <thead>
                                                <tr>
                                                    <th>Linea Negocio</th>
                                                    <th>Categoria</th>
                                                    <th>Sueldo Base</th>
                                                    <th>Complemento</th>
                                                    <th>Total Nominal</th>
                                                    <th>Sueldo Mensual</th>
                                                    <th>Esquema de Pago</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            ";

                            foreach (var itemDet in item)
                            {
                                cuerpo += "<tr>" +
                                    "<td>" + lstLineaNegocio.FirstOrDefault(e => e.id == itemDet.FK_LineaNegocio).concepto + "</td>" +
                                    "<td>" + lstCategorias.FirstOrDefault(e => e.id == itemDet.FK_Categoria).concepto + "</td>" +
                                    "<td>" + itemDet.sueldoBase.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + itemDet.complemento.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + itemDet.totalNominal.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + itemDet.sueldoMensual.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + lstEsquemaPago.FirstOrDefault(e => e.id == itemDet.FK_EsquemaPago).concepto + "</td>" +
                                "</tr>";
                            }

                            cuerpo += "</tbody>" +
                                      "</table>" +
                                      "<br><br><br>";
                        }

                        #region TABLA AUTORIZANTES
                        cuerpo += @"
                                    <table>
                                        <thead>
                                            <tr>
                                                <th>Nombre</th>
                                                <th>Tipo</th>
                                                <th>Autorizo</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        ";

                        bool esPrimero = true;
                        //EstatusGestionAutorizacionEnum P 0 A 1 R 2 
                        foreach (var itemDet in lstGestionAutorizantesDTO)
                        {
                            tblP_Usuario objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == itemDet.FK_UsuarioAutorizacion);
                            string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                            cuerpo += "<tr>" +
                                        "<td>" + nombreCompleto + "</td>" +
                                        "<td>" + EnumHelper.GetDescription(itemDet.nivelAutorizante) + "</td>" +
                                        GetEstatusTabulador(0, esPrimero) +
                                    "</tr>";

                            if (esPrimero)
                            {
                                esPrimero = false;
                            }
                        }

                        cuerpo += "</tbody>" +
                                    "</table>" +
                                    "<br><br><br>";


                        #endregion

                        cuerpo += "<br><br><br>" +
                              "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                              "Capital Humano > Administración de personal > Configuración > Estructura Salarial > Gestión de tabuladores.<br><br>" +
                              "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                              "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias." +
                                " </body>" +
                              "</html>";
                        #endregion

                        #region SE REGISTRA A LOS AUTORIZANTES
                        List<tblRH_TAB_GestionAutorizantes> lstGestion = new List<tblRH_TAB_GestionAutorizantes>();
                        tblRH_TAB_GestionAutorizantes objGestion = new tblRH_TAB_GestionAutorizantes();
                        foreach (var item in lstGestionAutorizantesDTO)
                        {
                            objGestion = new tblRH_TAB_GestionAutorizantes();
                            objGestion.FK_Registro = FK_Tabulador;
                            objGestion.vistaAutorizacion = VistaAutorizacionEnum.ASIGNACION_TABULADORES;
                            objGestion.nivelAutorizante = item.nivelAutorizante;
                            objGestion.FK_UsuarioAutorizacion = item.FK_UsuarioAutorizacion;
                            objGestion.autorizado = (int)EstatusGestionAutorizacionEnum.PENDIENTE;
                            objGestion.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objGestion.fechaCreacion = DateTime.Now;
                            objGestion.registroActivo = true;
                            lstGestion.Add(objGestion);
                        }
                        _context.tblRH_TAB_GestionAutorizantes.AddRange(lstGestion);
                        _context.SaveChanges();

                        #region SE NOTIFICA A LOS AUTORIZANTES QUE SE HA REGISTRADO UNA ASIGNACIÓN DE TABULADOR PARA SER APROBADA
                        List<int> lstFK_UsuarioAutorizacion = lstGestionAutorizantesDTO.Select(s => s.FK_UsuarioAutorizacion).ToList();
                        List<string> lstCorreosNotificar = new List<string>();
                        for (int i = 0; i < lstFK_UsuarioAutorizacion.Count(); i++)
                        {
                            int FK_UsuarioAutorizacion = lstFK_UsuarioAutorizacion[i];
                            tblP_Usuario objUsuario = _context.tblP_Usuario.Where(w => w.id == FK_UsuarioAutorizacion).FirstOrDefault();
                            if (objUsuario == null)
                                throw new Exception("Ocurrió un error al notificar a los autorizantes.");

                            string correo = objUsuario.correo;
                            if (string.IsNullOrEmpty(correo))
                            {
                                string mensajeError = string.Format("El autorizante [{0} {1}], no cuenta con correo registrado. Favor de notificar a TI.", objUsuario.nombre.Trim(), objUsuario.apellidoPaterno.Trim());
                                throw new Exception(mensajeError);
                            }
                            lstCorreosNotificar.Add(correo.Trim());
                        }
                        lstCorreosNotificar.Add(_CORREO_DIANA_ALVAREZ);
                        lstCorreosNotificar.Add(_CORREO_MANUEL_CRUZ);
#if DEBUG
                        lstCorreosNotificar = new List<string>();
                        lstCorreosNotificar.Add(_CORREO_OMAR_NUNEZ);
#endif
                        GlobalUtils.sendEmail(string.Format("{0}: NUEVO TABULADOR PARA [{1}] {2}", PersonalUtilities.GetNombreEmpresa(), objPuesto.puesto, objPuesto.descripcion), cuerpo, lstCorreosNotificar);

                        #region Alerta SIGOPLAN
                        tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                        objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                        objNuevaAlerta.userRecibeID = lstGestionAutorizantesDTO[0].FK_UsuarioAutorizacion;
#if DEBUG
                        objNuevaAlerta.userRecibeID = _USUARIO_OMAR_NUNEZ_ID; //USUARIO ID: Omar Nuñez.
                        //objNuevaAlerta.userRecibeID = 3807; //USUARIO ID: OSKAR НАХУЙ БЛЯТЬ.
#endif
                        objNuevaAlerta.tipoAlerta = 2;
                        objNuevaAlerta.sistemaID = 16;
                        objNuevaAlerta.visto = false;
                        objNuevaAlerta.url = "/Administrativo/Tabuladores/GestionTabuladores";
                        objNuevaAlerta.objID = objCE.id;
                        objNuevaAlerta.obj = "AutorizacionTabulador";
                        objNuevaAlerta.msj = "Tabulador Pendiente de Autorizar - Puesto: " + "[" + objPuesto.puesto + "] ";
                        objNuevaAlerta.documentoID = 0;
                        objNuevaAlerta.moduloID = 0;
                        _context.tblP_Alerta.Add(objNuevaAlerta);
                        _context.SaveChanges();
                        #endregion

                        #endregion
                        #endregion
                        #endregion
                    }
                    else
                    {
                        #region NUEVO DETALLE EN TABULADOR AUTORIZADO
                        #region SE VERIFICA SI EL TABULADOR NO SE ENCUENTRA EN ESTATUS DE GESTIÓN PENDIENTE
                        tblRH_EK_Puestos objPuesto = new tblRH_EK_Puestos();
                        if (objCE.tabuladorAutorizado == EstatusGestionAutorizacionEnum.PENDIENTE)
                        {
                            objPuesto = _context.tblRH_EK_Puestos.Where(w => w.puesto == objCE.FK_Puesto && w.registroActivo).FirstOrDefault();
                            if (objPuesto == null) { throw new Exception("Ocurrió un error al obtener la información del puesto."); }
                            string mensajeError = string.Format("El puesto [{0}] {1}, ya cuenta con un tabulador en gestión con estatus pendiente. Por lo cual no se puede agregar hasta finalizar dicha gestión.",
                                objPuesto.puesto, !string.IsNullOrEmpty(objPuesto.descripcion) ? objPuesto.descripcion.Trim() : string.Empty);
                            throw new Exception(mensajeError);
                        }
                        #endregion

                        #region SE REGISTRA EL DETALLE DEL TABULADOR | SE REGISTRA A LOS AUTORIZANTES
                        List<tblRH_TAB_TabuladoresDet> lstCE_Det = new List<tblRH_TAB_TabuladoresDet>();
                        tblRH_TAB_TabuladoresDet objCE_Det = new tblRH_TAB_TabuladoresDet();
                        foreach (var item in lstTabuladoresDTO)
                        {
                            objCE_Det = new tblRH_TAB_TabuladoresDet();
                            objCE_Det.FK_Tabulador = objTabuladorDTO.FK_Tabulador;
                            objCE_Det.FK_LineaNegocio = item.FK_LineaNegocio;
                            objCE_Det.FK_Categoria = item.FK_Categoria;
                            objCE_Det.tabuladorDetAutorizado = (int)EstatusGestionAutorizacionEnum.PENDIENTE;
                            objCE_Det.sueldoBase = item.sueldoBase;
                            objCE_Det.complemento = item.complemento;
                            objCE_Det.totalNominal = item.totalNominal;
                            objCE_Det.sueldoMensual = item.sueldoMensual;
                            objCE_Det.FK_EsquemaPago = item.FK_EsquemaPago;
                            objCE_Det.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objCE_Det.fechaCreacion = DateTime.Now;
                            objCE_Det.registroActivo = true;
                            lstCE_Det.Add(objCE_Det);
                        }
                        _context.tblRH_TAB_TabuladoresDet.AddRange(lstCE_Det);
                        _context.SaveChanges();
                        #endregion

                        #region SE CONSTRUYE CUERPO DEL CORREO
                        List<tblRH_TAB_CatEsquemaPago> lstEsquemaPago = _context.tblRH_TAB_CatEsquemaPago.Where(e => e.registroActivo).ToList();
                        objPuesto = _context.tblRH_EK_Puestos.Where(e => e.registroActivo && e.puesto == objTabuladorDTO.FK_Puesto).FirstOrDefault();

                        string cuerpo =
                                        @"<html>
                                            <head>
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
                                                        background-color: #ffcc5c;
                                                    }
                                                </style>
                                            </head>
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>

                                                    <p class=MsoNormal>
                                                        Buen día,<br><br>Se ha registrado un nuevo tabulador, se encuentra listo para su gestión.<br><br>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                        "Puesto: [" + objPuesto.puesto + "] " + objPuesto.descripcion + @".<o:p></o:p>
                                                    </p>
                                                    <br><br><br>
                                            ";
                        var lstGrpTabuladoresDet = lstTabuladoresDTO.GroupBy(e => e.FK_LineaNegocio).ToList();

                        foreach (var item in lstGrpTabuladoresDet)
                        {
                            cuerpo += @"
                                        <table>
                                            <thead>
                                                <tr>
                                                    <th>Linea Negocio</th>
                                                    <th>Categoria</th>
                                                    <th>Sueldo Base</th>
                                                    <th>Complemento</th>
                                                    <th>Total Nominal</th>
                                                    <th>Sueldo Mensual</th>
                                                    <th>Esquema de Pago</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            ";

                            foreach (var itemDet in item)
                            {
                                cuerpo += "<tr>" +
                                    "<td>" + lstLineaNegocio.FirstOrDefault(e => e.id == itemDet.FK_LineaNegocio).concepto + "</td>" +
                                    "<td>" + lstCategorias.FirstOrDefault(e => e.id == itemDet.FK_Categoria).concepto + "</td>" +
                                    "<td>" + itemDet.sueldoBase.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + itemDet.complemento.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + itemDet.totalNominal.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + itemDet.sueldoMensual.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + lstEsquemaPago.FirstOrDefault(e => e.id == itemDet.FK_EsquemaPago).concepto + "</td>" +
                                "</tr>";
                            }

                            cuerpo += "</tbody>" +
                                      "</table>" +
                                      "<br><br><br>";
                        }


                        #region TABLA AUTORIZANTES

                        cuerpo += @"
                                    <table>
                                        <thead>
                                            <tr>
                                                <th>Nombre</th>
                                                <th>Tipo</th>
                                                <th>Autorizo</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        ";

                        bool esPrimero = true;
                        //EstatusGestionAutorizacionEnum P 0 A 1 R 2 
                        foreach (var itemDet in lstGestionAutorizantesDTO)
                        {
                            tblP_Usuario objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == itemDet.FK_UsuarioAutorizacion);
                            string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                            cuerpo += "<tr>" +
                                        "<td>" + nombreCompleto + "</td>" +
                                        "<td>" + EnumHelper.GetDescription(itemDet.nivelAutorizante) + "</td>" +
                                        GetEstatusTabulador(0, esPrimero) +
                                    "</tr>";

                            if (esPrimero)
                            {
                                esPrimero = false;
                            }
                        }

                        cuerpo += "</tbody>" +
                                    "</table>" +
                                    "<br><br><br>";


                        #endregion

                        cuerpo += "<br><br><br>" +
                              "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                              "Capital Humano > Administración de personal > Configuración > Estructura Salarial > Gestión de tabuladores.<br><br>" +
                              "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                              "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias." +
                                " </body>" +
                              "</html>";
                        #endregion

                        #region SE REGISTRA A LOS AUTORIZANTES
                        // SE OBTIENE LOS ID DEL DETALLE QUE SE ACABA DE REGISTRAR
                        List<int> lstFK_TabuladorDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.FK_Tabulador == objTabuladorDTO.FK_Tabulador &&
                                                                                                    w.tabuladorDetAutorizado == EstatusGestionAutorizacionEnum.PENDIENTE && w.registroActivo).Select(s => s.id).ToList();

                        List<tblRH_TAB_GestionAutorizantes> lstGestion = new List<tblRH_TAB_GestionAutorizantes>();
                        tblRH_TAB_GestionAutorizantes objGestion = new tblRH_TAB_GestionAutorizantes();
                        foreach (var itemDet in lstFK_TabuladorDet)
                        {
                            foreach (var itemGestion in lstGestionAutorizantesDTO)
                            {
                                objGestion = new tblRH_TAB_GestionAutorizantes();
                                objGestion.FK_Registro = objTabuladorDTO.FK_Tabulador;
                                objGestion.FK_TabuladorDet = itemDet;
                                objGestion.categoriaNueva = true;
                                objGestion.vistaAutorizacion = VistaAutorizacionEnum.ASIGNACION_TABULADORES;
                                objGestion.nivelAutorizante = itemGestion.nivelAutorizante;
                                objGestion.FK_UsuarioAutorizacion = itemGestion.FK_UsuarioAutorizacion;
                                objGestion.autorizado = (int)EstatusGestionAutorizacionEnum.PENDIENTE;
                                objGestion.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                                objGestion.fechaCreacion = DateTime.Now;
                                objGestion.registroActivo = true;
                                lstGestion.Add(objGestion);
                            }
                        }
                        _context.tblRH_TAB_GestionAutorizantes.AddRange(lstGestion);
                        _context.SaveChanges();

                        #region SE NOTIFICA A LOS AUTORIZANTES QUE SE HA REGISTRADO UNA ASIGNACIÓN DE TABULADOR PARA SER APROBADA
                        List<int> lstFK_UsuarioAutorizacion = lstGestionAutorizantesDTO.Select(s => s.FK_UsuarioAutorizacion).ToList();
                        List<string> lstCorreosNotificar = new List<string>();
                        for (int i = 0; i < lstFK_UsuarioAutorizacion.Count(); i++)
                        {
                            int FK_UsuarioAutorizacion = lstFK_UsuarioAutorizacion[i];
                            tblP_Usuario objUsuario = _context.tblP_Usuario.Where(w => w.id == FK_UsuarioAutorizacion).FirstOrDefault();
                            if (objUsuario == null)
                                throw new Exception("Ocurrió un error al notificar a los autorizantes.");

                            string correo = objUsuario.correo;
                            if (string.IsNullOrEmpty(correo))
                            {
                                string mensajeError = string.Format("El autorizante [{0} {1}], no cuenta con correo registrado. Favor de notificar a TI.", objUsuario.nombre.Trim(), objUsuario.apellidoPaterno.Trim());
                                throw new Exception(mensajeError);
                            }
                            lstCorreosNotificar.Add(correo.Trim());
                        }
                        lstCorreosNotificar.Add(_CORREO_DIANA_ALVAREZ);
                        lstCorreosNotificar.Add(_CORREO_MANUEL_CRUZ);
#if DEBUG
                        lstCorreosNotificar = new List<string>();
                        //lstCorreosNotificar.Add("miguel.buzani@construplan.com.mx");
                        lstCorreosNotificar.Add(_CORREO_OMAR_NUNEZ);
#endif
                        GlobalUtils.sendEmail(string.Format("{0}: NUEVO TABULADOR PARA [{1}] {2}", PersonalUtilities.GetNombreEmpresa(), objPuesto.puesto, objPuesto.descripcion), cuerpo, lstCorreosNotificar);
                        #endregion
                        #endregion
                        #endregion
                    }

                    dbContextTransaction.Commit();
                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha registrado con éxito.");

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objTabuladorDTO));
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(lstTabuladoresDTO));
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(lstGestionAutorizantesDTO));
                }
                catch (Exception e)
                {

                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "CEAsignacionTabulador", e, AccionEnum.CONSULTA, objTabuladorDTO.id, new
                    {
                        objTabuladorDTO = objTabuladorDTO,
                        lstTabuladoresDTO = lstTabuladoresDTO,
                        lstGestionAutorizantesDTO = lstGestionAutorizantesDTO
                    });
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);

                    GlobalUtils.sendEmail((PersonalUtilities.GetNombreEmpresa() + ": ERROR TABULADOR PUESTO:" + objTabuladorDTO.FK_Puesto + " FECHA: " + DateTime.Now.ToString()),
                                        (
                                        e.Message + "\n<br>************************* INICIO OBJ TABULADOR *************************" +
                                        JsonUtils.convertNetObjectToJson(objTabuladorDTO) + "\n<br>************************ INICIO OBJ TABULADOR DET ***********************" +
                                        JsonUtils.convertNetObjectToJson(lstTabuladoresDTO) + "\n<br>*********************** INICIO OBJ LST AUTORIZANTES ************************" +
                                        JsonUtils.convertNetObjectToJson(lstGestionAutorizantesDTO)

                                        ), new List<string> { "miguel.buzani@construplan.com.mx" });
                }
            }
            return resultado;
        }

        private string GetEstatusTabulador(int est, bool aut)
        {
            string estatusTabulador = string.Empty;
            try
            {
                if ((int)EstatusGestionAutorizacionEnum.PENDIENTE == (est) && aut)
                    estatusTabulador = "<td style='background-color: yellow;'>AUTORIZANDO</td>";
                else if ((int)EstatusGestionAutorizacionEnum.AUTORIZADO == (est))
                    estatusTabulador = "<td style='background-color: #82E0AA;'>AUTORIZADO</td>";
                else
                    if ((int)EstatusGestionAutorizacionEnum.RECHAZADO == (est))
                        estatusTabulador = "<td style='background-color: #EC7063;'>RECHAZADO</td>";
                    else
                        estatusTabulador = "<td style='background-color: #FAE5D3;'>PENDIENTE</td>";
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, new { est = est, aut = aut });
            }
            return estatusTabulador;
        }

        public Dictionary<string, object> EliminarTabuladoresPuesto(TabuladorDTO objFiltroDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado.Clear();
                try
                {
                    #region VALIDACIONES
                    string mensajeError = "Ocurrió un error al eliminar el registro.";
                    if (objFiltroDTO.id <= 0) { throw new Exception(mensajeError); }
                    #endregion

                    #region SE ELIMINA LOS TABULADORES DEL PUESTO Y SU REGISTRO PRINCIPAL

                    #region REGISTRO PRINCIPAL
                    tblRH_TAB_Tabuladores objEliminar = _context.tblRH_TAB_Tabuladores.Where(w => w.id == objFiltroDTO.id && w.registroActivo).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception(mensajeError);

                    objEliminar.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEliminar.fechaModificacion = DateTime.Now;
                    objEliminar.registroActivo = false;
                    _context.SaveChanges();
                    #endregion

                    #region DETALLE DE REGISTRO PRINCIPAL
                    List<tblRH_TAB_TabuladoresDet> lstEliminarDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.FK_Tabulador == objFiltroDTO.id && w.registroActivo).ToList();
                    foreach (var item in lstEliminarDet)
                    {
                        item.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        item.fechaModificacion = DateTime.Now;
                        item.registroActivo = false;
                    }
                    _context.SaveChanges();
                    #endregion

                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro principal y su detalle.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, objFiltroDTO.id, JsonUtils.convertNetObjectToJson(objFiltroDTO));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "EliminarTabuladoresPuesto", e, AccionEnum.ELIMINAR, objFiltroDTO.id, objFiltroDTO);
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetTabuladoresExistentes(TabuladorDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region VALIDACIONES
                if (objParamDTO.FK_Puesto <= 0) { throw new Exception("Ocurrió un error al verificar la existencia de tabuladores."); }
                if (objParamDTO.FK_LineaNegocio <= 0) { throw new Exception("Ocurrió un error al verificar la existencia de tabuladores."); }
                #endregion

                #region CATALOGOS
                List<tblRH_TAB_CatCategorias> lstCategorias = _context.tblRH_TAB_CatCategorias.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatEsquemaPago> lstEsquemasPagos = _context.tblRH_TAB_CatEsquemaPago.Where(w => w.registroActivo).ToList();
                #endregion

                #region SE OBTIENE LOS TABULADORES DE LA LINEA DE NEGOCIO
                tblRH_TAB_Tabuladores objTabulador = _context.tblRH_TAB_Tabuladores.Where(w => w.FK_Puesto == objParamDTO.FK_Puesto && w.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo).FirstOrDefault();
                List<tblRH_TAB_TabuladoresDet> lstTabuladorDet = new List<tblRH_TAB_TabuladoresDet>();
                if (objTabulador != null)
                    lstTabuladorDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.FK_Tabulador == objTabulador.id && w.tabuladorDetAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.FK_LineaNegocio == objParamDTO.FK_LineaNegocio && w.registroActivo).ToList();

                bool tabuladorExistente = false;
                TabuladorDetDTO objDTO = new TabuladorDetDTO();
                List<TabuladorDetDTO> lstDTO = new List<TabuladorDetDTO>();
                foreach (var item in lstTabuladorDet)
                {
                    tblRH_TAB_CatCategorias objCategoria = lstCategorias.Where(w => w.id == item.FK_Categoria && w.registroActivo).FirstOrDefault();
                    if (objCategoria == null) { throw new Exception("Ocurrió un error al obtener la categoría de un tabulador existente."); }

                    tblRH_TAB_CatEsquemaPago objEsquemaPago = lstEsquemasPagos.Where(w => w.id == item.FK_EsquemaPago && w.registroActivo).FirstOrDefault();
                    if (objEsquemaPago == null) { throw new Exception("Ocurrió un error al obtener el esquema de pago de un tabulador existente."); }

                    objDTO = new TabuladorDetDTO();
                    objDTO.categoriaDesc = !string.IsNullOrEmpty(objCategoria.concepto) ? objCategoria.concepto.Trim() : string.Empty;
                    objDTO.sueldoBase = (decimal)item.sueldoBase;
                    objDTO.complemento = (decimal)item.complemento;
                    objDTO.totalNominal = (decimal)item.totalNominal;
                    objDTO.sueldoMensual = (decimal)item.sueldoMensual;
                    objDTO.esquemaPagoDesc = !string.IsNullOrEmpty(objEsquemaPago.concepto) ? objEsquemaPago.concepto.Trim() : string.Empty;
                    lstDTO.Add(objDTO);
                    tabuladorExistente = true;
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("tabuladorExistente", tabuladorExistente);
                resultado.Add("lstDTO", lstDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetTabuladoresExistentes", e, AccionEnum.CONSULTA, objParamDTO.id, objParamDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        #region TABULADORES DET
        public Dictionary<string, object> GetListadoTabuladoresDet(TabuladorDetDTO objFiltroDTO)
        {
            resultado.Clear();
            try
            {
                #region VALIDACIONES
                string mensajeError = "Ocurrió un error al obtener el listado de tabuladores.";
                if (objFiltroDTO.FK_Tabulador <= 0) { throw new Exception(mensajeError); }
                #endregion

                #region SE OBTIENE LOS TABULADORES DEL PUESTO SELECCIONADO
                List<TabuladorDetDTO> lstTabuladoresDet = _context.Select<TabuladorDetDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT t1.id, t2.concepto AS lineaNegocioDesc, t3.concepto AS categoriaDesc, t1.sueldoBase, t1.complemento, t1.totalNominal, t1.sueldoMensual, t4.concepto AS esquemaPagoDesc
	                                    FROM tblRH_TAB_TabuladoresDet AS t1
	                                    INNER JOIN tblRH_TAB_CatLineaNegocio AS t2 ON t2.id = t1.FK_LineaNegocio
	                                    INNER JOIN tblRH_TAB_CatCategorias AS t3 ON t3.id = t1.FK_Categoria
	                                    INNER JOIN tblRH_TAB_CatEsquemaPago AS t4 ON t4.id = t1.FK_EsquemaPago
		                                    WHERE t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND t3.registroActivo = @registroActivo AND t4.registroActivo = @registroActivo
			                                    ORDER BY t1.FK_Tabulador",
                    parametros = new { registroActivo = true }
                }).ToList();

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("lstTabuladoresDet", lstTabuladoresDet);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetListadoTabuladoresDet", e, AccionEnum.CONSULTA, objFiltroDTO.FK_Tabulador, objFiltroDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarTabuladorDet(TabuladorDetDTO objFiltroDTO)
        {
            resultado.Clear();
            try
            {
                #region VALIDACIONES
                string mensajeError = "Ocurrió un error al obtener la información del tabulador.";
                if (objFiltroDTO.id <= 0) { throw new Exception(mensajeError); }
                #endregion

                #region SE OBTIENE LA INFORMACIÓN DEL TABULADOR A ACTUALIZAR
                tblRH_TAB_TabuladoresDet objTabuladorDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.id == objFiltroDTO.id && w.registroActivo).FirstOrDefault();
                if (objTabuladorDet == null)
                    throw new Exception(mensajeError);

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("objTabuladorDet", objTabuladorDet);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetDatosActualizarTabuladorDet", e, AccionEnum.CONSULTA, objFiltroDTO.id, objFiltroDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> ActualizarTabuladorDet(TabuladorDetDTO objFiltroDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado.Clear();
                try
                {
                    #region VALIDACIONES
                    string mensajeError = "Ocurrió un error al actualizar el registro.";
                    if (objFiltroDTO.id <= 0) { throw new Exception(mensajeError); }
                    if (objFiltroDTO.sueldoBase <= 0) { throw new Exception("Es necesario indicar el sueldo base."); }
                    if (objFiltroDTO.complemento <= 0) { throw new Exception("Es necesario indicar el complemento."); }
                    if (objFiltroDTO.totalNominal <= 0) { throw new Exception("Es necesario indicar el total nominal."); }
                    if (objFiltroDTO.sueldoMensual <= 0) { throw new Exception("Es necesario indicar el sueldo mensual."); }
                    if (objFiltroDTO.FK_LineaNegocio <= 0) { throw new Exception("Es necesario seleccionar la linea de negocio."); }
                    if (objFiltroDTO.FK_Categoria <= 0) { throw new Exception("Es necesario seleccionar la categoría."); }
                    if (objFiltroDTO.FK_EsquemaPago <= 0) { throw new Exception("Es necesario seleccionar el esquema de pago."); }
                    #endregion

                    #region ACTUALIZAR REGISTRO
                    tblRH_TAB_TabuladoresDet objActualizar = _context.tblRH_TAB_TabuladoresDet.Where(w => w.id == objFiltroDTO.id && w.registroActivo).FirstOrDefault();
                    if (objActualizar == null)
                        throw new Exception(mensajeError);

                    objActualizar.sueldoBase = (decimal)objFiltroDTO.sueldoBase;
                    objActualizar.complemento = (decimal)objFiltroDTO.complemento;
                    objActualizar.totalNominal = (decimal)objFiltroDTO.totalNominal;
                    objActualizar.sueldoMensual = (decimal)objFiltroDTO.sueldoMensual;
                    objActualizar.FK_LineaNegocio = objFiltroDTO.FK_LineaNegocio;
                    objActualizar.FK_Categoria = objFiltroDTO.FK_Categoria;
                    objActualizar.FK_EsquemaPago = objFiltroDTO.FK_EsquemaPago;
                    objActualizar.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objActualizar.fechaModificacion = DateTime.Now;
                    objActualizar.registroActivo = true;
                    _context.SaveChanges();
                    #endregion

                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha actualizado con éxito el registro.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objFiltroDTO.id, JsonUtils.convertNetObjectToJson(objFiltroDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "ActualizarTabuladorDet", e, AccionEnum.ACTUALIZAR, objFiltroDTO.id, objFiltroDTO);
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarTabuladorDet(TabuladorDetDTO objParamDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado.Clear();
                try
                {
                    #region VALIDACIONES
                    string mensajeError = "Ocurrió un error al eliminar el registro.";
                    if (objParamDTO.id <= 0) { throw new Exception(mensajeError); }

                    #region SE VERIFICA QUE EL TABULADOR NUNCA HAYA ESTADO EN USO
                    List<tblRH_EK_Tabulador_Historial> lstTabuladorHistorial = _context.tblRH_EK_Tabulador_Historial.Where(w => w.FK_TabuladorDet == objParamDTO.id && w.esActivo).ToList();
                    if (lstTabuladorHistorial.Count() > 0) { throw new Exception("No se puede eliminar el tabulador, ya que se encuentra en uso."); }

                    List<tblRH_REC_Requisicion> lstRecRequisiciones = _context.tblRH_REC_Requisicion.Where(w => w.idTabuladorDet == objParamDTO.id && w.registroActivo).ToList();
                    if (lstRecRequisiciones.Count() > 0) { throw new Exception("No se puede eliminar el tabulador, ya que se encuentra en uso."); }
                    #endregion
                    #endregion

                    #region SE ELIMINA EL REGISTRO SELECCIONADO
                    tblRH_TAB_TabuladoresDet objEliminar = _context.tblRH_TAB_TabuladoresDet.Where(w => w.id == objParamDTO.id && w.registroActivo).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception(mensajeError);

                    objEliminar.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEliminar.fechaModificacion = DateTime.Now;
                    objEliminar.registroActivo = false;
                    _context.SaveChanges();

                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, objParamDTO.id, JsonUtils.convertNetObjectToJson(objParamDTO));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "EliminarTabuladorDet", e, AccionEnum.ELIMINAR, objParamDTO.id, objParamDTO);
                    resultado.Clear();
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }
        #endregion
        #endregion

        #region GESTIÓN TABULADORES
        public Dictionary<string, object> GetGestionTabuladores(TabuladorDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region INIT / INFORMACIÓN PRECARGADA
                tblRH_TAB_Tabuladores objTabulador = new tblRH_TAB_Tabuladores();
                tblRH_TAB_TabuladoresDet objTabuladorDet = new tblRH_TAB_TabuladoresDet();
                List<tblRH_TAB_Tabuladores> lstTabuladoresForEach = new List<tblRH_TAB_Tabuladores>();

                bool puedeFirmarAdmin = false;
                if (vSesiones.sesionUsuarioDTO.id != _USUARIO_GERARDO_REINA_ID)
                    puedeFirmarAdmin = _context.tblP_Usuario.Any(w => w.id == vSesiones.sesionUsuarioDTO.id && w.perfilID == _PERFIL_ID_ADMINISTRADOR);

                List<tblRH_TAB_Tabuladores> lstTabuladoresInfo = _context.Select<tblRH_TAB_Tabuladores>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT * FROM tblRH_TAB_Tabuladores WHERE registroActivo = @registroActivo",
                    parametros = new { registroActivo = true }
                }).ToList();

                List<tblRH_TAB_TabuladoresDet> lstTabuladoresDetInfo = _context.Select<tblRH_TAB_TabuladoresDet>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT * FROM tblRH_TAB_TabuladoresDet WHERE registroActivo = @registroActivo",
                    parametros = new { registroActivo = true }
                }).ToList();

                List<tblRH_TAB_GestionAutorizantes> lstGestionTabuladoresInfo = _context.Select<tblRH_TAB_GestionAutorizantes>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT * FROM tblRH_TAB_GestionAutorizantes WHERE registroActivo = @registroActivo",
                    parametros = new { registroActivo = true }
                }).ToList();

                List<tblRH_TAB_CatLineaNegocio> lstLineaNegociosInfo = _context.Select<tblRH_TAB_CatLineaNegocio>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT * FROM tblRH_TAB_CatLineaNegocio WHERE registroActivo = @registroActivo",
                    parametros = new { registroActivo = true }
                }).ToList();

                List<tblRH_EK_Puestos> lstPuestosInfo = _context.Select<tblRH_EK_Puestos>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT * FROM tblRH_EK_Puestos WHERE registroActivo = @registroActivo",
                    parametros = new { registroActivo = true }
                }).ToList();
                #endregion

                #region SE OBTIENE LISTADO DE GESTIÓN DE TABULADORES
                lstGestionTabuladoresInfo = lstGestionTabuladoresInfo
                    .Where(w => w.autorizado == objParamDTO.tabuladorAutorizado && w.vistaAutorizacion == (int)VistaAutorizacionEnum.ASIGNACION_TABULADORES && w.registroActivo).ToList();

                foreach (var item in lstGestionTabuladoresInfo)
                {
                    if (item.FK_Registro > 0)
                    {
                        if (!lstTabuladoresForEach.Any(w => w.id == item.FK_Registro))
                        {
                            objTabulador = new tblRH_TAB_Tabuladores();
                            objTabulador = lstTabuladoresInfo.Where(w => w.id == item.FK_Registro && w.registroActivo).FirstOrDefault();
                            if (objTabulador != null)
                                lstTabuladoresForEach.Add(objTabulador);
                        }
                    }
                }

                List<TabuladorDTO> lstTabuladoresDTO = new List<TabuladorDTO>();
                TabuladorDTO objTabuladorDTO = new TabuladorDTO();
                foreach (var item in lstTabuladoresForEach)
                {
                    objTabuladorDTO = new TabuladorDTO();
                    List<tblRH_TAB_GestionAutorizantes> lstAutorizantes = lstGestionTabuladoresInfo.Where(w => w.vistaAutorizacion == VistaAutorizacionEnum.ASIGNACION_TABULADORES && w.FK_Registro == item.id)
                                                                                .OrderBy(e => e.nivelAutorizante).ToList();
                    List<tblRH_TAB_TabuladoresDet> lstTabuladorDet = lstTabuladoresDetInfo.Where(w => w.registroActivo && w.FK_Tabulador == item.id).ToList();

                    #region AUTORIZANTES
                    int? sigAuth = null;
                    foreach (var itemAuth in lstAutorizantes)
                    {
                        if (itemAuth.autorizado == EstatusGestionAutorizacionEnum.AUTORIZADO || itemAuth.autorizado == EstatusGestionAutorizacionEnum.RECHAZADO)
                            objTabuladorDTO.esFirmar = false;
                        else
                        {
                            if (sigAuth == null)
                                sigAuth = itemAuth.FK_UsuarioAutorizacion;

                            if (sigAuth.Value == vSesiones.sesionUsuarioDTO.id)
                            {
                                objTabuladorDTO.esFirmar = true;
                                break;
                            }
                            else
                                objTabuladorDTO.esFirmar = false;
                        }
                    }

                    if (puedeFirmarAdmin)
                        objTabuladorDTO.esFirmar = true;
                    #endregion

                    List<string> lstDescLineaNegocio = new List<string>();
                    bool tieneTabDetallePendiente = false;
                    List<int> lstFK_TabuladorDet = new List<int>();
                    foreach (var itemDet in lstTabuladorDet)
                    {
                        if (itemDet.FK_LineaNegocio > 0)
                        {
                            string descLineaNegocio = lstLineaNegociosInfo.FirstOrDefault(e => e.id == itemDet.FK_LineaNegocio).concepto;
                            if (!lstDescLineaNegocio.Contains(descLineaNegocio))
                                lstDescLineaNegocio.Add(descLineaNegocio);

                            lstFK_TabuladorDet.Add(itemDet.id);
                        }

                        if (itemDet.tabuladorDetAutorizado == EstatusGestionAutorizacionEnum.PENDIENTE)
                            tieneTabDetallePendiente = true;
                    }

                    objTabuladorDTO.lstDescLineaNegocio = lstDescLineaNegocio;
                    if (item.FK_Puesto > 0)
                    {
                        objTabuladorDTO.id = item.id;
                        objTabuladorDTO.puestoDesc = lstPuestosInfo.Where(w => w.puesto == item.FK_Puesto).Select(s => s.descripcion).FirstOrDefault();
                        objTabuladorDTO.tabuladorAutorizadoDesc = EnumHelper.GetDescription((EstatusGestionAutorizacionEnum)item.tabuladorAutorizado);
                        objTabuladorDTO.tabuladorAutorizado = VerificarEstatusTabulador(item.id, lstFK_TabuladorDet, lstTabuladoresInfo);
                        objTabuladorDTO.tabuladorDetAutorizado = tieneTabDetallePendiente;
                        objTabuladorDTO.comentarioRechazo = item.comentarioRechazo != null ? item.comentarioRechazo.ToString() : string.Empty;
                        objTabuladorDTO.FK_Puesto = item.FK_Puesto;
                        lstTabuladoresDTO.Add(objTabuladorDTO);
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("lstTabuladoresDTO", lstTabuladoresDTO);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamDTO.id, objParamDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> AutorizarRechazarTabulador(TabuladorDTO objParamDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado.Clear();
                try
                {
                    #region VALIDACIONES
                    if (objParamDTO.id <= 0) { throw new Exception("Ocurrió un error al realizar la acción."); }
                    if (objParamDTO.tabuladorAutorizado <= 0) { throw new Exception("Ocurrió un error al realizar la acción."); }
                    #endregion

                    #region CATALOGOS // INIT INFORMACIÓN
                    List<CategoriaDTO> lstCategoriasDTO = GetListaCategorias().ToList();
                    List<LineaNegocioDTO> lstLineasNegociosDTO = GetListaLineasNegocios().ToList();
                    List<EsquemaPagoDTO> lstEsquemasPagosDTO = GetListaEsquemasPagos().ToList();

                    //List<tblRH_TAB_GestionAutorizantes> lstAutorizantes = _context.tblRH_TAB_GestionAutorizantes.Where(w => w.FK_Registro == objParamDTO.id &&
                    //                                                                                                        w.vistaAutorizacion == VistaAutorizacionEnum.ASIGNACION_TABULADORES &&
                    //                                                                                                        w.registroActivo && !w.procesoFinalizado).ToList();
                    List<tblRH_TAB_GestionAutorizantes> lstAutorizantes = _context.Select<tblRH_TAB_GestionAutorizantes>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT autorizado, FK_UsuarioAutorizacion
	                                    FROM tblRH_TAB_GestionAutorizantes
		                                    WHERE FK_Registro = @FK_Tabulador AND vistaAutorizacion = @vistaAutorizacion AND registroActivo = @registroActivo AND procesoFinalizado = @procesoFinalizado
			                                    GROUP BY autorizado, FK_UsuarioAutorizacion",
                        parametros = new { FK_Tabulador = objParamDTO.id, vistaAutorizacion = VistaAutorizacionEnum.ASIGNACION_TABULADORES, registroActivo = true, procesoFinalizado = false }
                    }).ToList();

                    if (lstAutorizantes.Count() <= 0)
                        throw new Exception("Ocurrió un error al obtener el listado de firmantes.");

                    tblRH_TAB_Tabuladores objTabulador = _context.tblRH_TAB_Tabuladores.Where(w => w.id == objParamDTO.id && w.registroActivo).FirstOrDefault();
                    if (objTabulador == null)
                        throw new Exception("Ocurrió un error al obtener la información del tabulador.");

                    List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.FK_Tabulador == objParamDTO.id && w.registroActivo &&
                                                                                                                    w.tabuladorDetAutorizado == EstatusGestionAutorizacionEnum.PENDIENTE).ToList();
                    if (lstTabuladoresDet.Count() <= 0)
                        throw new Exception("Ocurrió un error al obtener la información del tabulador.");

                    tblRH_EK_Puestos objPuesto = _context.tblRH_EK_Puestos.Where(w => w.puesto == objTabulador.FK_Puesto && w.registroActivo).FirstOrDefault();
                    if (objPuesto == null)
                        throw new Exception("Ocurrió un error al obtener la información del puesto.");
                    #endregion

                    #region SE AUTORIZA/RECHAZA EL TABULADOR
                    List<tblRH_TAB_GestionAutorizantes> lstGestionUsuario = _context.tblRH_TAB_GestionAutorizantes.Where(w => w.vistaAutorizacion == VistaAutorizacionEnum.ASIGNACION_TABULADORES &&
                                                                                                                              w.FK_Registro == objParamDTO.id &&
                                                                                                                              w.FK_UsuarioAutorizacion == vSesiones.sesionUsuarioDTO.id &&
                                                                                                                              w.registroActivo).ToList();

                    int totalAutorizacionesRequeridas = lstAutorizantes.Where(w => w.autorizado != EstatusGestionAutorizacionEnum.RECHAZADO).Count();
                    int totalAutorizacionesRealizadas = lstAutorizantes.Where(w => w.autorizado == EstatusGestionAutorizacionEnum.AUTORIZADO).Count();

                    if (objParamDTO.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO)
                        totalAutorizacionesRealizadas++;

                    if (!AutorizarRechazarGestionTabulador(objTabulador, objParamDTO, lstTabuladoresDet, lstGestionUsuario, totalAutorizacionesRealizadas, totalAutorizacionesRequeridas))
                        throw new Exception(string.Format("Ocurrió un error al {0} el tabulador", objParamDTO.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO ? "autorizar" : "rechazar"));

                    EliminarAlertaGestionTabuladores(objParamDTO.id);

                    if (objParamDTO.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO)
                    {
                        List<string> lstCorreos = GetCorreosAutorizantes(lstGestionUsuario.Select(s => s.FK_UsuarioAutorizacion).ToList());
                        if (totalAutorizacionesRequeridas == totalAutorizacionesRealizadas)
                            // SE ENVIA CORREO INDICANDO QUE SE ENCUENTRA AUTORIZADO EL TABULADOR
                            EnviarCorreoGestionTabuladorAutorizado(lstCorreos, objPuesto,
                                SetCorreoHTMLGestionTabuladores(totalAutorizacionesRequeridas, totalAutorizacionesRealizadas, objPuesto.puesto, objPuesto.descripcion, lstTabuladoresDet, lstLineasNegociosDTO, lstCategoriasDTO,
                                                                    lstEsquemasPagosDTO, lstGestionUsuario, true));
                        else
                        {
                            // SE CREA ALERTA Y SE ENVIA CORREO AL SIGUIENTE AUTORIZANTE
                            foreach (var item in lstAutorizantes.Where(w => w.autorizado == EstatusGestionAutorizacionEnum.PENDIENTE))
                            {
                                if (item.FK_UsuarioAutorizacion != vSesiones.sesionUsuarioDTO.id)
                                {
                                    CrearAlertaGestionTabuladores(item.FK_UsuarioAutorizacion, objParamDTO.id, objPuesto.puesto);
                                    tblP_Usuario objUsuario = _context.tblP_Usuario.Where(w => w.id == item.FK_UsuarioAutorizacion && w.estatus).FirstOrDefault();
                                    if (objUsuario == null)
                                        throw new Exception("Ocurrió un error al obtener el correo del siguiente autorizante.<br>Favor de reportarlo a sistemas.");

                                    EnviarCorreoGestionTabuladorEnProceso(new List<string> { objUsuario.correo }, objPuesto,
                                                                            SetCorreoHTMLGestionTabuladores(totalAutorizacionesRequeridas,
                                                                                                            totalAutorizacionesRealizadas,
                                                                                                            objPuesto.puesto,
                                                                                                            objPuesto.descripcion,
                                                                                                            lstTabuladoresDet,
                                                                                                            lstLineasNegociosDTO,
                                                                                                            lstCategoriasDTO,
                                                                                                            lstEsquemasPagosDTO,
                                                                                                            lstAutorizantes,
                                                                                                            true));
                                    break;
                                }
                            }
                        }
                    }

                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, objParamDTO.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO ? "Se ha autorizado con éxito." : "Se ha rechazado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objParamDTO.id, JsonUtils.convertNetObjectToJson(objParamDTO));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "AutorizarRechazarTabulador", e, AccionEnum.ACTUALIZAR, objParamDTO.id, objParamDTO);
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        private List<string> GetCorreosAutorizantes(List<int> lstUsuariosID)
        {
            List<string> lstCorreos = new List<string>();
            try
            {
                List<tblP_Usuario> lstUsuarios = _context.tblP_Usuario.Where(w => lstUsuariosID.Contains(w.id) && w.estatus).ToList();
                if (lstUsuarios.Count() <= 0)
                    throw new Exception("Ocurrió un error al obtener el listado de correos de los autorizantes.");

                foreach (var item in lstUsuarios)
                {
                    if (!string.IsNullOrEmpty(item.correo))
                        lstCorreos.Add(item.correo);
                }
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, new { lstUsuariosID = lstUsuariosID });
            }
            return lstCorreos;
        }

        public Dictionary<string, object> GetLstAutorizantesTabulador(GestionAutorizanteDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                List<int> lstFK_TabuladorDet = new List<int>();
                if (_context.tblRH_TAB_Tabuladores.Any(w => w.id == objParamDTO.FK_Tabulador && w.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo))
                    lstFK_TabuladorDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.FK_Tabulador == objParamDTO.FK_Tabulador && w.registroActivo).Select(s => s.id).ToList();

                string strQuery = string.Format(@"SELECT FK_UsuarioAutorizacion, autorizado, nivelAutorizante 
	                                                    FROM tblRH_TAB_GestionAutorizantes 
		                                                    WHERE FK_Registro = @FK_Tabulador AND {0} registroActivo = @registroActivo AND procesoFinalizado = @procesoFinalizado
			                                                    GROUP BY FK_UsuarioAutorizacion, autorizado, nivelAutorizante 
				                                                    ORDER BY nivelAutorizante",
                                                                        lstFK_TabuladorDet.Count() > 0 ? string.Format("FK_TabuladorDet IN ({0}) AND", string.Join(",", lstFK_TabuladorDet)) : string.Empty);
                List<tblRH_TAB_GestionAutorizantes> lstAutorizantes = _context.Select<tblRH_TAB_GestionAutorizantes>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { FK_Tabulador = objParamDTO.FK_Tabulador, registroActivo = true, procesoFinalizado = false }
                }).ToList();

                GestionAutorizanteDTO objAutorizanteDTO = new GestionAutorizanteDTO();
                List<GestionAutorizanteDTO> lstAutorizantesDTO = new List<GestionAutorizanteDTO>();
                foreach (var item in lstAutorizantes)
                {
                    int usuarioYaEnLista = lstAutorizantesDTO.Where(w => w.FK_UsuarioAutorizacion == item.FK_UsuarioAutorizacion).Count();

                    if (usuarioYaEnLista <= 0)
                    {
                        tblP_Usuario objUsuario = _context.tblP_Usuario.Where(w => w.id == item.FK_UsuarioAutorizacion && w.estatus).FirstOrDefault();
                        if (objUsuario == null)
                            throw new Exception("Ocurrió un error al obtener el nombre del autorizante.");

                        objAutorizanteDTO = new GestionAutorizanteDTO();
                        objAutorizanteDTO.nombreAutorizante = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(string.Format("{0} {1} {2}", objUsuario.nombre, objUsuario.apellidoPaterno, objUsuario.apellidoMaterno));
                        objAutorizanteDTO.autorizado = item.autorizado;
                        lstAutorizantesDTO.Add(objAutorizanteDTO);
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("lstAutorizantes", lstAutorizantesDTO);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamDTO.id, objParamDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> GuardarComentarioRechazoTabulador(int idTabulador, string comentario)
        {
            resultado.Clear();

            using (var _ctx = new MainContext())
            {
                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var lstCorreosNotificar = new List<string>();
                        var objPlantilla = _ctx.tblRH_TAB_Tabuladores.FirstOrDefault(e => e.id == idTabulador);
                        var lstFirmas = _context.tblRH_TAB_GestionAutorizantes.Where(e => e.registroActivo && e.vistaAutorizacion == VistaAutorizacionEnum.ASIGNACION_TABULADORES && e.FK_Registro == objPlantilla.id).ToList();
                        string cuerpo = string.Empty;

                        objPlantilla.comentarioRechazo = comentario;

                        var objPuesto = _context.tblRH_EK_Puestos.FirstOrDefault(e => e.registroActivo && e.puesto == objPlantilla.FK_Puesto);

                        //foreach (var item in lstFirmas)
                        //{
                        //    var objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == item.FK_UsuarioAutorizacion);
                        //    //lstCorreosNotificar.Add(objUsuario.correo);
                        //}

                        List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.FK_Tabulador == idTabulador &&
                                                                                                    w.registroActivo).ToList();

                        List<tblRH_TAB_CatCategorias> lstCategorias = _context.tblRH_TAB_CatCategorias.Where(w => w.registroActivo).ToList();
                        List<tblRH_TAB_CatLineaNegocio> lstLineaNegocio = _context.tblRH_TAB_CatLineaNegocio.Where(w => w.registroActivo).ToList();
                        List<tblRH_TAB_CatEsquemaPago> lstEsquemaPago = _context.tblRH_TAB_CatEsquemaPago.Where(e => e.registroActivo).ToList();

                        #region CUERPO CORREO
                        cuerpo =
                                    @"<html>
                                            <head>
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
                                                        background-color: #ffcc5c;
                                                    }
                                                </style>
                                            </head>
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>

                                                    <p class=MsoNormal>
                                                        Buen día,<br><br>Se ha rechazado un tabulador. Motivo : " + comentario + "." + @".<br><br>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                    "Puesto: [" + objPuesto.puesto + "] " + objPuesto.descripcion + @".<o:p></o:p>
                                                    </p>
                                                    <br><br><br>
                                            ";
                        var lstGrpTabuladoresDet = lstTabuladoresDet.GroupBy(e => e.FK_LineaNegocio).ToList();

                        foreach (var item in lstGrpTabuladoresDet)
                        {
                            cuerpo += @"
                                            <table>
                                                <thead>
                                                    <tr>
                                                        <th>Linea Negocio</th>
                                                        <th>Categoria</th>
                                                        <th>Sueldo Base</th>
                                                        <th>Complemento</th>
                                                        <th>Total Nominal</th>
                                                        <th>Sueldo Mensual</th>
                                                        <th>Esquema de Pago</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                ";

                            foreach (var itemDet in item)
                            {
                                cuerpo += "<tr>" +
                                    "<td>" + lstLineaNegocio.FirstOrDefault(e => e.id == itemDet.FK_LineaNegocio).concepto + "</td>" +
                                    "<td>" + lstCategorias.FirstOrDefault(e => e.id == itemDet.FK_Categoria).concepto + "</td>" +
                                    "<td>" + itemDet.sueldoBase.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + itemDet.complemento.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + itemDet.totalNominal.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + itemDet.sueldoMensual.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                                    "<td>" + lstEsquemaPago.FirstOrDefault(e => e.id == itemDet.FK_EsquemaPago).concepto + "</td>" +
                                "</tr>";
                            }

                            cuerpo += "</tbody>" +
                                      "</table>" +
                                      "<br><br><br>";
                        }

                        #region TABLA AUTORIZANTES

                        cuerpo += @"
                                        <table>
                                            <thead>
                                                <tr>
                                                    <th>Nombre</th>
                                                    <th>Tipo</th>
                                                    <th>Autorizo</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            ";

                        bool esAuth = false;
                        //EstatusGestionAutorizacionEnum P 0 A 1 R 2 
                        foreach (var itemDet in lstFirmas)
                        {
                            tblP_Usuario objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == itemDet.FK_UsuarioAutorizacion);

                            if (vSesiones.sesionUsuarioDTO.id == itemDet.FK_UsuarioAutorizacion)
                            {
                                itemDet.autorizado = EstatusGestionAutorizacionEnum.RECHAZADO;
                                itemDet.fechaFirma = DateTime.Now;
                            }

                            string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                            cuerpo += "<tr>" +
                                        "<td>" + nombreCompleto + "</td>" +
                                        "<td>" + EnumHelper.GetDescription(itemDet.nivelAutorizante) + "</td>" +
                                        GetEstatusTabulador((int)itemDet.autorizado, false) +
                                    "</tr>";

                        }

                        cuerpo += "</tbody>" +
                                    "</table>" +
                                    "<br><br><br>";


                        #endregion

                        cuerpo += "<br><br><br>" +
                              "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                              "Capital Humano > Administración de personal > Configuración > Estructura Salarial > Gestión de tabuladores.<br><br>" +
                              "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                              "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias." +
                            " </body>" +
                          "</html>";

                        #endregion

                        lstCorreosNotificar.Add(_CORREO_DIANA_ALVAREZ);
                        lstCorreosNotificar.Add(_CORREO_MANUEL_CRUZ);
#if DEBUG
                        lstCorreosNotificar = new List<string> { _CORREO_OMAR_NUNEZ };
#endif
                        GlobalUtils.sendEmail(string.Format("{0}: TABULADOR RECHAZADO EN EL PUESTO [{1}] {2}", PersonalUtilities.GetNombreEmpresa(), objPuesto.puesto, objPuesto.descripcion), cuerpo, lstCorreosNotificar);

                        _ctx.SaveChanges();
                        dbContextTransaction.Commit();
                        resultado.Clear();
                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Clear();
                        resultado.Add(MESSAGE, e.Message);
                        resultado.Add(SUCCESS, false);
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetDetalleRelTabulador(TabuladorDetDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region VALIDACIONES
                if (objParamDTO.FK_Tabulador <= 0) { throw new Exception("Ocurrió un error al obtener el detalle del tabulador."); }
                #endregion

                #region SE OBTIENE CATALOGOS
                List<tblRH_TAB_CatLineaNegocio> lstLineaNegociosInfo = _context.tblRH_TAB_CatLineaNegocio.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatCategorias> lstCategoriasInfo = _context.tblRH_TAB_CatCategorias.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatEsquemaPago> lstEsquemaPagoInfo = _context.tblRH_TAB_CatEsquemaPago.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_Tabuladores> lstTabuladoresInfo = _context.tblRH_TAB_Tabuladores.Where(w => w.registroActivo).ToList();
                #endregion

                #region SE OBTIENE EL DETALLE DEL TABULADOR
                List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet = _context.Select<tblRH_TAB_TabuladoresDet>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT t2.id, t2.FK_Tabulador, t2.FK_LineaNegocio, t2.FK_Categoria, t2.tabuladorDetAutorizado, t2.sueldoBase, t2.complemento, t2.totalNominal, t2.sueldoMensual, t2.FK_EsquemaPago
	                                    FROM tblRH_TAB_GestionAutorizantes AS t1
	                                    INNER JOIN tblRH_TAB_TabuladoresDet AS t2 ON t2.FK_Tabulador = t1.FK_Registro
		                                    WHERE t1.FK_Registro = @FK_Tabulador AND t1.registroActivo = @registroActivo AND t1.autorizado = @autorizado AND 
                                                  t2.registroActivo = @registroActivo AND t2.tabuladorDetAutorizado = @autorizado
			                                            GROUP BY t2.id, t2.FK_Tabulador, t2.FK_LineaNegocio, t2.FK_Categoria, t2.tabuladorDetAutorizado, t2.sueldoBase, t2.complemento, 
                                                                 t2.totalNominal, t2.sueldoMensual, t2.FK_EsquemaPago",
                    parametros = new { FK_Tabulador = objParamDTO.FK_Tabulador, registroActivo = true, autorizado = objParamDTO.tabuladorDetAutorizado }
                }).ToList();

                List<TabuladorDetDTO> lstTabuladorDetDTO = new List<TabuladorDetDTO>();
                TabuladorDetDTO objTabuladorDetDTO = new TabuladorDetDTO();
                foreach (var item in lstTabuladoresDet)
                {
                    tblRH_TAB_CatLineaNegocio objLineaNegocio = lstLineaNegociosInfo.Where(w => w.id == item.FK_LineaNegocio && w.registroActivo).FirstOrDefault();
                    if (objLineaNegocio == null) { throw new Exception("No se encuentra la línea de negocio."); }
                    tblRH_TAB_CatCategorias objCategoria = lstCategoriasInfo.Where(w => w.id == item.FK_Categoria && w.registroActivo).FirstOrDefault();
                    if (objCategoria == null) { throw new Exception("No se encuentra la categoría."); }
                    tblRH_TAB_CatEsquemaPago objEsquemaPago = lstEsquemaPagoInfo.Where(w => w.id == item.FK_EsquemaPago && w.registroActivo).FirstOrDefault();
                    if (objEsquemaPago == null) { throw new Exception("No se encuentra el esquema de pago."); }

                    objTabuladorDetDTO = new TabuladorDetDTO();
                    objTabuladorDetDTO.id = item.id;
                    objTabuladorDetDTO.FK_Tabulador = item.FK_Tabulador;
                    objTabuladorDetDTO.lineaNegocioDesc = objLineaNegocio.concepto.Trim();
                    objTabuladorDetDTO.categoriaDesc = objCategoria.concepto.Trim();

                    #region SE VERIFICA EL ESTATUS DE TODOS LOS FIRMANTES
                    item.tabuladorDetAutorizado = VerificarEstatusTabulador(objParamDTO.FK_Tabulador, new List<int> { item.id }, lstTabuladoresInfo);

                    string colorBoton = string.Empty;
                    switch (item.tabuladorDetAutorizado)
                    {
                        case EstatusGestionAutorizacionEnum.PENDIENTE:
                            colorBoton = "warning";
                            break;
                        case EstatusGestionAutorizacionEnum.AUTORIZADO:
                            colorBoton = "success";
                            break;
                        case EstatusGestionAutorizacionEnum.RECHAZADO:
                            colorBoton = "danger";
                            break;
                    }
                    objTabuladorDetDTO.tabuladorDetAutorizadoDesc = string.Format("<button class='btn btn-xs btn-{0}'>{1}</button>", colorBoton, EnumHelper.GetDescription((EstatusGestionAutorizacionEnum)item.tabuladorDetAutorizado));
                    #endregion

                    objTabuladorDetDTO.sueldoBase = (decimal)item.sueldoBase;
                    objTabuladorDetDTO.complemento = (decimal)item.complemento;
                    objTabuladorDetDTO.totalNominal = (decimal)item.totalNominal;
                    objTabuladorDetDTO.sueldoMensual = (decimal)item.sueldoMensual;
                    objTabuladorDetDTO.esquemaPagoDesc = objEsquemaPago.concepto.Trim();
                    lstTabuladorDetDTO.Add(objTabuladorDetDTO);
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("lstTabuladorDet", lstTabuladorDetDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetDetalleRelTabulador", e, AccionEnum.CONSULTA, objParamDTO.id, objParamDTO);
                resultado.Clear();
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        /// <summary>
        /// Caso "Autorizar/Rechazar tabulador pendiente":
        ///     Si desean autorizar o rechazar un tabulador, el cual su registro principal es estatus "PENDIENTE", se aplica la acción al registro principal, a su detalle y a la gestión de firmas.
        /// Caso "Autorizar/Rechazar tabulador previamente autorizado":
        ///     Si desean autorizar o rechazar el detalle nuevo de un tabulador ya previamente autorizado, se aplica la acción solamente al nuevo detalle del tabulador y a la gestión de firmas. 
        ///     
        /// Nota: 
        ///     El tabulador principal y su detalle, solamente se autoriza cuando todos los firmantes hayan realizado las autorizaciones correspondientes, mientras tanto, continua en pendiente.
        /// </summary>
        /// <param name="objTabulador"></param>
        /// <param name="objParamDTO"></param>
        /// <param name="lstTabuladoresDet"></param>
        /// <param name="lstGestionUsuario"></param>
        /// <param name="totalAutorizacionesRealizadas"></param>
        /// <param name="totalAutorizacionesRequeridas"></param>
        /// <returns></returns>
        private bool AutorizarRechazarGestionTabulador(tblRH_TAB_Tabuladores objTabulador, TabuladorDTO objParamDTO, List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet, List<tblRH_TAB_GestionAutorizantes> lstGestionUsuario,
                                                        int totalAutorizacionesRealizadas, int totalAutorizacionesRequeridas)
        {
            bool exitoAccion = true;
            try
            {
                if (objTabulador.tabuladorAutorizado == EstatusGestionAutorizacionEnum.PENDIENTE)
                {
                    #region TABULADOR NUEVO CON ESTATUS PENDIENTE
                    if (objParamDTO.tabuladorAutorizado == EstatusGestionAutorizacionEnum.RECHAZADO)
                    {
                        #region SE RECHAZA EL TABULADOR (REGISTRO PRINCIPAL)
                        // ENCABEZADO TABULADOR
                        objTabulador.tabuladorAutorizado = EstatusGestionAutorizacionEnum.RECHAZADO;
                        objTabulador.comentarioRechazo = objParamDTO.comentarioRechazo;
                        objTabulador.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objTabulador.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();

                        // DETALLE TABULADOR
                        foreach (var item in lstTabuladoresDet)
                        {
                            item.tabuladorDetAutorizado = EstatusGestionAutorizacionEnum.RECHAZADO;
                            item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                            item.fechaModificacion = DateTime.Now;
                        }
                        _context.SaveChanges();

                        // SE INDICA RECHAZADO EN LA GESTIÓN DE FIRMAS
                        foreach (var item in lstGestionUsuario)
                        {
                            item.autorizado = EstatusGestionAutorizacionEnum.RECHAZADO;
                            item.fechaFirma = DateTime.Now;
                            item.comentario = objParamDTO.comentarioRechazo;
                            item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                            item.fechaModificacion = DateTime.Now;
                        }
                        _context.SaveChanges();

                        // SE INDICA COMO PROCESO FINALIZADO
                        List<tblRH_TAB_GestionAutorizantes> lstGestionUsuarios =
                            _context.tblRH_TAB_GestionAutorizantes.Where(w => w.vistaAutorizacion == VistaAutorizacionEnum.ASIGNACION_TABULADORES &&
                                                                                w.FK_Registro == objParamDTO.id && !w.procesoFinalizado && w.registroActivo).ToList();
                        foreach (var item in lstGestionUsuarios)
                            item.procesoFinalizado = true;

                        _context.SaveChanges();
                        #endregion
                    }
                    else if (objParamDTO.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO)
                    {
                        #region SE AUTORIZA TABULADOR
                        if (totalAutorizacionesRequeridas == totalAutorizacionesRealizadas)
                        {
                            // ENCABEZADO TABULADOR
                            objTabulador.tabuladorAutorizado = EstatusGestionAutorizacionEnum.AUTORIZADO;
                            objTabulador.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objTabulador.fechaModificacion = DateTime.Now;
                            _context.SaveChanges();

                            // DETALLE TABULADOR
                            foreach (var item in lstTabuladoresDet)
                            {
                                item.tabuladorDetAutorizado = EstatusGestionAutorizacionEnum.AUTORIZADO;
                                item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                item.fechaModificacion = DateTime.Now;
                            }
                            _context.SaveChanges();
                        }

                        // SE INDICA LA AUTORIZACIÓN EN LA GESTIÓN DE FIRMAS
                        foreach (var item in lstGestionUsuario)
                        {
                            item.autorizado = EstatusGestionAutorizacionEnum.AUTORIZADO;
                            item.fechaFirma = DateTime.Now;
                            item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                            item.fechaModificacion = DateTime.Now;
                        }
                        _context.SaveChanges();

                        // SE INDICA COMO PROCESO FINALIZADO
                        if (totalAutorizacionesRequeridas == totalAutorizacionesRealizadas)
                        {
                            List<tblRH_TAB_GestionAutorizantes> lstGestionUsuarios =
                                _context.tblRH_TAB_GestionAutorizantes.Where(w => w.vistaAutorizacion == VistaAutorizacionEnum.ASIGNACION_TABULADORES &&
                                                                                  w.FK_Registro == objParamDTO.id && !w.procesoFinalizado && w.registroActivo).ToList();
                            foreach (var item in lstGestionUsuarios)
                                item.procesoFinalizado = true;

                            _context.SaveChanges();
                        }
                        #endregion
                    }
                    #endregion
                }
                else if (objTabulador.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO)
                {
                    #region TABULADOR YA EXISTENTE PREVIAMENTE AUTORIZADO
                    if (objParamDTO.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO)
                    {
                        #region SE AUTORIZA EL DETALLE DEL TABULADOR NUEVO
                        if (totalAutorizacionesRequeridas == totalAutorizacionesRealizadas)
                        {
                            // DETALLE TABULADOR
                            foreach (var item in lstTabuladoresDet)
                            {
                                item.tabuladorDetAutorizado = EstatusGestionAutorizacionEnum.AUTORIZADO;
                                item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                item.fechaModificacion = DateTime.Now;
                            }
                            _context.SaveChanges();
                        }

                        // SE INDICA LA AUTORIZACIÓN EN LA GESTIÓN DE FIRMAS
                        foreach (var item in lstGestionUsuario)
                        {
                            item.autorizado = EstatusGestionAutorizacionEnum.AUTORIZADO;
                            item.fechaFirma = DateTime.Now;
                            item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                            item.fechaModificacion = DateTime.Now;
                        }
                        _context.SaveChanges();

                        // SE INDICA COMO PROCESO FINALIZADO
                        if (totalAutorizacionesRequeridas == totalAutorizacionesRealizadas)
                        {
                            List<tblRH_TAB_GestionAutorizantes> lstGestionUsuarios =
                                _context.tblRH_TAB_GestionAutorizantes.Where(w => w.vistaAutorizacion == VistaAutorizacionEnum.ASIGNACION_TABULADORES &&
                                                                                  w.FK_Registro == objParamDTO.id && !w.procesoFinalizado && w.registroActivo).ToList();
                            foreach (var item in lstGestionUsuarios)
                                item.procesoFinalizado = true;

                            _context.SaveChanges();
                        }
                        #endregion
                    }
                    else if (objParamDTO.tabuladorAutorizado == EstatusGestionAutorizacionEnum.RECHAZADO)
                    {
                        #region SE RECHAZA EL DETALLE DEL TABULADOR NUEVO
                        // DETALLE TABULADOR
                        foreach (var item in lstTabuladoresDet)
                        {
                            item.tabuladorDetAutorizado = EstatusGestionAutorizacionEnum.RECHAZADO;
                            item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                            item.fechaModificacion = DateTime.Now;
                        }
                        _context.SaveChanges();

                        // SE INDICA LA AUTORIZACIÓN EN LA GESTIÓN DE FIRMAS
                        foreach (var item in lstGestionUsuario)
                        {
                            item.autorizado = EstatusGestionAutorizacionEnum.RECHAZADO;
                            item.fechaFirma = DateTime.Now;
                            item.comentario = objParamDTO.comentarioRechazo;
                            item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                            item.fechaModificacion = DateTime.Now;
                        }
                        _context.SaveChanges();

                        // SE INDICA COMO PROCESO FINALIZADO
                        if (totalAutorizacionesRequeridas == totalAutorizacionesRealizadas)
                        {
                            List<tblRH_TAB_GestionAutorizantes> lstGestionUsuarios =
                                _context.tblRH_TAB_GestionAutorizantes.Where(w => w.vistaAutorizacion == VistaAutorizacionEnum.ASIGNACION_TABULADORES &&
                                                                                  w.FK_Registro == objParamDTO.id && !w.procesoFinalizado && w.registroActivo).ToList();
                            foreach (var item in lstGestionUsuarios)
                                item.procesoFinalizado = true;

                            _context.SaveChanges();
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ACTUALIZAR, 0,
                    new
                    {
                        objTabulador = objTabulador,
                        objParamDTO = objParamDTO,
                        lstTabuladoresDet = lstTabuladoresDet,
                        lstGestionUsuario = lstGestionUsuario,
                        totalAutorizacionesRealizadas = totalAutorizacionesRealizadas,
                        totalAutorizacionesRequeridas = totalAutorizacionesRequeridas
                    });
                exitoAccion = false;
            }
            return exitoAccion;
        }

        /// <summary>
        /// Crea alerta al usuario logueado, indicando que tiene un tabulador pendiente por autorizar o rechazar
        /// </summary>
        /// <param name="FK_UsuarioRecibe"></param>
        /// <param name="FK_Tabulador"></param>
        /// <param name="puesto"></param>
        /// <returns>Verdadero si se registra con éxito, de lo contrario, falso.</returns>
        private bool CrearAlertaGestionTabuladores(int FK_UsuarioRecibe, int FK_Tabulador, int FK_Puesto)
        {
            bool exitoAlertaCreada = true;
            try
            {
                #region CREAR ALERTA
                tblP_Alerta objAlerta = new tblP_Alerta();
                objAlerta.userEnviaID = vSesiones.sesionUsuarioDTO.id;
                objAlerta.userRecibeID = FK_UsuarioRecibe;
#if DEBUG
                objAlerta.userRecibeID = _USUARIO_OMAR_NUNEZ_ID;
#endif
                objAlerta.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                objAlerta.sistemaID = _SISTEMA;
                objAlerta.visto = false;
                objAlerta.url = "/Administrativo/Tabuladores/GestionTabuladores";
                objAlerta.objID = FK_Tabulador;
                objAlerta.obj = "AutorizacionTabulador";
                objAlerta.msj = string.Format("Tabulador pendiente de autorizar - Puesto: [{0}]", FK_Puesto);
                objAlerta.documentoID = 0;
                objAlerta.moduloID = 0;
                _context.tblP_Alerta.Add(objAlerta);
                _context.SaveChanges();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.AGREGAR, 0, new { FK_UsuarioRecibe = FK_UsuarioRecibe, FK_Tabulador = FK_Tabulador, FK_Puesto = FK_Puesto });
                exitoAlertaCreada = false;
            }
            return exitoAlertaCreada;
        }

        /// <summary>
        /// Elimina la alerta del usuario logueado, en caso que haya autorizado o rechazado el tabulador que se encontraba en gestión.
        /// </summary>
        /// <param name="FK_Tabulador"></param>
        /// <returns>Verdadero si se registra con éxito, de lo contrario, falso.</returns>
        private bool EliminarAlertaGestionTabuladores(int FK_Tabulador)
        {
            bool exitoAlertaEliminada = true;
            try
            {
                tblP_Alerta objAlerta = _context.tblP_Alerta.Where(w => w.userRecibeID == vSesiones.sesionUsuarioDTO.id && w.sistemaID == _SISTEMA && !w.visto && w.objID == FK_Tabulador).FirstOrDefault();
                objAlerta.visto = true;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.ELIMINAR, 0, new { FK_Tabulador = FK_Tabulador });
                exitoAlertaEliminada = false;
            }
            return exitoAlertaEliminada;
        }

        /// <summary>
        /// Se envia correo a los firmantes, Diana Alvarez y a Manuel Cruz, indicando que el tabulador ya se encuentra 100% autorizado.
        /// </summary>
        /// <param name="lstCorreos"></param>
        /// <param name="tabuladorAutorizado"></param>
        /// <param name="objPuesto"></param>
        /// <param name="correoHTML"></param>
        /// <returns>Verdadero si se envia con éxito, de lo contrario, falso.</returns>
        private bool EnviarCorreoGestionTabuladorAutorizado(List<string> lstCorreos, tblRH_EK_Puestos objPuesto, string correoHTML)
        {
            bool exitoEnviarCorreo = true;
            try
            {
                lstCorreos.Add(_CORREO_DIANA_ALVAREZ);
                lstCorreos.Add(_CORREO_MANUEL_CRUZ);
#if DEBUG
                lstCorreos = new List<string> { _CORREO_OMAR_NUNEZ };
#endif
                GlobalUtils.sendEmail(string.Format(("{0}: Tabulador autorizado en el puesto [{1}] {2}"), PersonalUtilities.GetNombreEmpresa(), objPuesto.puesto, objPuesto.descripcion), correoHTML, lstCorreos);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CORREO, 0, new { lstCorreos = lstCorreos, objPuesto = objPuesto, correoHTML = correoHTML });
                exitoEnviarCorreo = false;
            }
            return exitoEnviarCorreo;
        }

        private bool EnviarCorreoGestionTabuladorEnProceso(List<string> lstCorreos, tblRH_EK_Puestos objPuesto, string correoHTML)
        {
            bool exitoEnviarCorreo = true;
            try
            {
                lstCorreos.Add(_CORREO_DIANA_ALVAREZ);
                lstCorreos.Add(_CORREO_MANUEL_CRUZ);
#if DEBUG
                lstCorreos = new List<string> { _CORREO_OMAR_NUNEZ };
#endif
                GlobalUtils.sendEmail(string.Format(("{0}: Tabulador pendiente de autorizar [{1}] {2}"), PersonalUtilities.GetNombreEmpresa(), objPuesto.puesto, objPuesto.descripcion), correoHTML, lstCorreos);
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CORREO, 0, new { lstCorreos = lstCorreos, objPuesto = objPuesto, correoHTML = correoHTML });
                exitoEnviarCorreo = false;
            }
            return exitoEnviarCorreo;
        }

        /// <summary>
        /// Se construye variable con el cuerpo de HTML, estructurando a los firmantes y el estatus de dicha firma sobre el tabulador pendiente, para poder enviar este cuerpo por correo a los usuarios correspondientes.
        /// </summary>
        /// <param name="totalAutorizacionesRequeridas"></param>
        /// <param name="totalAutorizacionesPendientes"></param>
        /// <param name="puesto"></param>
        /// <param name="descripcionPuesto"></param>
        /// <param name="lstTabuladoresDet"></param>
        /// <param name="lstLineaNegocio"></param>
        /// <param name="lstCategorias"></param>
        /// <param name="lstEsquemaPago"></param>
        /// <param name="lstFirmantes"></param>
        /// <param name="tabuladorAutorizado"></param>
        /// <returns>Retorna variable string, la cual tiene el cuerpo completo del correo a enviar.</returns>
        private string SetCorreoHTMLGestionTabuladores(int totalAutorizacionesRequeridas, int totalAutorizacionesRealizadas, int puesto, string descripcionPuesto, List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet,
                                                        List<LineaNegocioDTO> lstLineaNegocio, List<CategoriaDTO> lstCategorias, List<EsquemaPagoDTO> lstEsquemaPago,
                                                        List<tblRH_TAB_GestionAutorizantes> lstFirmantes, bool tabuladorAutorizado)
        {
            string correoHTML = string.Empty;
            try
            {
                #region HEAD
                string mensajeCuerpo = totalAutorizacionesRequeridas == totalAutorizacionesRealizadas ? "Se ha autorizado un tabulador por todos los firmantes." : "Se ha autorizado un tabulador.";
                string body = @"<html>
                                                <head>
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
                                                            background-color: #ffcc5c;
                                                        }
                                                    </style>
                                                </head>
                                                <body lang=ES-MX link='#0563C1' vlink='#954F72'>
                                                    <p class=MsoNormal>Buen día,<br><br>" + mensajeCuerpo + "<br><br></p>" +
                                                    "<p class=MsoNormal style='font-weight:bold;'>Puesto: [" + puesto + "] " + descripcionPuesto + ".<o:p></o:p></p><br><br><br>";
                #endregion

                #region BODY
                var lstGrpTabuladoresDet = lstTabuladoresDet.GroupBy(g => g.FK_LineaNegocio).ToList();
                foreach (var item in lstGrpTabuladoresDet)
                {
                    body += @"<table>
                                    <thead>
                                        <tr>
                                            <th>Linea Negocio</th>
                                            <th>Categoria</th>
                                            <th>Sueldo Base</th>
                                            <th>Complemento</th>
                                            <th>Total Nominal</th>
                                            <th>Sueldo Mensual</th>
                                            <th>Esquema de Pago</th>
                                        </tr>
                                    </thead>
                                <tbody>";

                    foreach (var itemDet in item)
                    {
                        body += string.Format(@"<tr>
                                                    <td>{0}</td>
                                                    <td>{1}</td>
                                                    <td>{2}</td>
                                                    <td>{3}</td>
                                                    <td>{4}</td>
                                                    <td>{5}</td>
                                                    <td>{6}</td>
                                                  </tr>",
                                                    lstLineaNegocio.FirstOrDefault(f => f.id == itemDet.FK_LineaNegocio).concepto ?? "--",
                                                    lstCategorias.FirstOrDefault(f => f.id == itemDet.FK_Categoria).concepto ?? "--",
                                                    itemDet.sueldoBase.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) ?? "--",
                                                    itemDet.complemento.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) ?? "--",
                                                    itemDet.totalNominal.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) ?? "--",
                                                    itemDet.sueldoMensual.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) ?? "--",
                                                    lstEsquemaPago.FirstOrDefault(e => e.id == itemDet.FK_EsquemaPago).concepto ?? "--");
                    }
                    body += "</tbody></table><br><br><br>";
                }

                #region TABLA AUTORIZANTES
                body += @"<table>
                            <thead>
                                <tr>
                                    <th>Nombre</th>
                                    <th>Tipo</th>
                                    <th>Autorizo</th>
                                </tr>
                            </thead>
                        <tbody>";

                tblP_Usuario objUsuario = new tblP_Usuario();
                foreach (var itemFirmante in lstFirmantes)
                {
                    objUsuario = new tblP_Usuario();
                    objUsuario = _context.tblP_Usuario.Where(w => w.id == itemFirmante.FK_UsuarioAutorizacion && w.estatus).FirstOrDefault();
                    if (objUsuario == null)
                        throw new Exception("Ocurrió un error al obtener la información de un firmante.");

                    body += string.Format(@"<tr>
                                                <td>{0}</td>
                                                <td>{1}</td>
                                                    {2}
                                            </tr>", PersonalUtilities.NombreCompletoPrimerLetraMayuscula(PersonalUtilities.NombreCompleto(objUsuario.nombre, objUsuario.apellidoPaterno, objUsuario.apellidoMaterno)),
                                                    EnumHelper.GetDescription(itemFirmante.nivelAutorizante),
                                                    GetEstatusTabulador((int)itemFirmante.autorizado, tabuladorAutorizado));
                }

                body += "</tbody></table><br><br><br>";
                #endregion
                #endregion

                #region FOOTER
                body += "<br><br><br>" +
                        "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                            "Capital Humano > Administración de personal > Configuración > Estructura Salarial > Gestión de tabuladores.<br><br>" +
                            "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                            "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias." +
                        "</body>" +
                    "</html>";

                correoHTML = body;
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CORREO, 0,
                    new
                    {
                        totalAutorizacionesRequeridas = totalAutorizacionesRequeridas,
                        totalAutorizacionesRealizadas = totalAutorizacionesRealizadas,
                        puesto = puesto,
                        descripcionPuesto = descripcionPuesto,
                        lstTabuladoresDet = lstTabuladoresDet,
                        lstLineaNegocio = lstLineaNegocio,
                        lstCategorias = lstCategorias,
                        lstEsquemaPago = lstEsquemaPago,
                        lstFirmantes = lstFirmantes,
                        tabuladorAutorizado = tabuladorAutorizado
                    });
            }
            return correoHTML;
        }
        #endregion

        #region PLANTILLA DE PERSONAL
        public Dictionary<string, object> GetTabuladoresAutorizados(TabuladorDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region CATALOGOS
                List<tblRH_EK_Puestos> lstPuestos = _context.tblRH_EK_Puestos.Where(w => w.registroActivo).ToList();
                List<tblRH_EK_Tipos_Nomina> lstTipoNominas = _context.tblRH_EK_Tipos_Nomina.Where(w => w.esActivo == true).ToList();
                List<tblRH_TAB_CatAreaDepartamento> lstAreasDepartamentos = _context.tblRH_TAB_CatAreaDepartamento.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatCategorias> lstCategorias = _context.tblRH_TAB_CatCategorias.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatEsquemaPago> lstEsquemasPagos = _context.tblRH_TAB_CatEsquemaPago.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_PlantillasPersonal> lstPlantillas = _context.tblRH_TAB_PlantillasPersonal.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_PlantillasPersonalDet> lstPlantillasDet = _context.tblRH_TAB_PlantillasPersonalDet.Where(w => w.registroActivo).ToList();
                #endregion

                #region VALIDACIONES
                if (string.IsNullOrEmpty(objParamDTO.cc)) { throw new Exception("Es necesario seleccionar cc."); }
                if (objParamDTO.FK_LineaNegocio <= 0) { throw new Exception("Es necesario seleccionar una línea de negocio."); }
                if (objParamDTO.lstPuestos.Count <= 0) { throw new Exception("Es necesario seleccionar al menos un puesto."); }

                // SE VERIFICA SI EL PUESTO YA CUENTA CON PLANTILLA AUTORIZADA O PENDIENTE, EN BASE AL CC Y LINEA DE NEGOCIO
                List<tblRH_TAB_PlantillasPersonal> lstPlantillasPersonal = _context.tblRH_TAB_PlantillasPersonal.Where(w => w.cc == objParamDTO.cc && w.FK_LineaNegocio == objParamDTO.FK_LineaNegocio && w.plantillaAutorizada == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo).ToList();
                List<int> lstFK_PlantillaPersonal = lstPlantillasPersonal.Select(s => s.id).ToList();
                List<tblRH_TAB_PlantillasPersonalDet> lstPlantillasPersonalDet = _context.tblRH_TAB_PlantillasPersonalDet.Where(w => lstFK_PlantillaPersonal.Contains(w.FK_Plantilla) && w.registroActivo).ToList();

                tblRH_EK_Puestos objPuesto = new tblRH_EK_Puestos();
                foreach (var item in lstPlantillasPersonalDet)
                {
                    objPuesto = lstPuestos.Where(w => w.puesto == item.FK_Puesto).FirstOrDefault();
                    if (objPuesto == null) { throw new Exception("Ocurrió un error al obtener la información del puesto."); }

                    tblRH_TAB_PlantillasPersonal objPlantilla = lstPlantillasPersonal.Where(w => w.id == item.FK_Plantilla).FirstOrDefault();
                    if (objPlantilla != null)
                    {
                        string mensajeError = string.Empty;
                        if (objPlantilla.plantillaAutorizada == EstatusGestionAutorizacionEnum.AUTORIZADO)
                            mensajeError = string.Format("El puesto [{0}] {1}, ya cuenta con una plantilla autorizada.", objPuesto.puesto, objPuesto.descripcion);
                        else if (objPlantilla.plantillaAutorizada == EstatusGestionAutorizacionEnum.PENDIENTE)
                            mensajeError = string.Format("El puesto [{0}] {1}, cuenta con una plantilla con estatus pendiente de autorizar.", objPuesto.puesto, objPuesto.descripcion);

                        throw new Exception(mensajeError);
                    }
                }
                #endregion

                #region SE OBTIENE LISTADO DE TABULADORES AUTORIZADOS
                List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.tabuladorDetAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.FK_LineaNegocio == objParamDTO.FK_LineaNegocio && w.registroActivo).ToList();
                List<int> lstTabuladoresDetID = lstTabuladoresDet.Select(s => s.FK_Tabulador).ToList();
                List<tblRH_TAB_Tabuladores> lstTabuladores = _context.tblRH_TAB_Tabuladores.Where(w => w.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && lstTabuladoresDetID.Contains(w.id) && objParamDTO.lstPuestos.Contains(w.FK_Puesto) && w.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo).ToList();

                List<TabuladorDetDTO> lstTabuladoresDTO = new List<TabuladorDetDTO>();
                TabuladorDetDTO objTabuladorDTO = new TabuladorDetDTO();
                foreach (var item in lstTabuladores)
                {
                    // SE OBTIENE EL DETALLE DEL TABULADOR
                    List<tblRH_TAB_TabuladoresDet> lstTabDet = lstTabuladoresDet.Where(w => w.FK_Tabulador == item.id).ToList();
                    objTabuladorDTO = new TabuladorDetDTO();
                    foreach (var itemDet in lstTabuladoresDet.Where(w => w.FK_Tabulador == item.id).ToList())
                    {
                        #region INFORMACIÓN GENERAL DEL PUESTO
                        // SE OBTIENE EL PUESTO
                        objPuesto = lstPuestos.Where(w => w.puesto == item.FK_Puesto && w.registroActivo).FirstOrDefault();
                        if (objPuesto == null)
                            throw new Exception("Ocurrió un error al obtener la descripción del puesto.");

                        // SE OBTIENE EL TIPO DE NOMINA
                        tblRH_EK_Tipos_Nomina objTipoNomina = lstTipoNominas.Where(w => w.tipo_nomina == objPuesto.FK_TipoNomina).FirstOrDefault();
                        if (objTipoNomina == null)
                            throw new Exception("Ocurrió un error al obtener el tipo de nómina del puesto.");

                        // SE OBTIENE EL ÁREA / DEPARTAMENTO
                        tblRH_TAB_CatAreaDepartamento objAreaDepartamento = lstAreasDepartamentos.Where(w => w.id == objPuesto.FK_AreaDepartamento).FirstOrDefault();
                        if (objAreaDepartamento == null)
                            throw new Exception("Ocurrió un error al obtener el área / departamento del puesto.");

                        // SE OBTIENE LA CATEGORIA
                        tblRH_TAB_CatCategorias objCategoria = lstCategorias.Where(w => w.id == itemDet.FK_Categoria).FirstOrDefault();
                        if (objCategoria == null)
                            throw new Exception("Ocurrió un error al obtener la categoría del puesto.");

                        // SE OBTIENE EL ESQUEMA DE PAGO
                        tblRH_TAB_CatEsquemaPago objEsquemaPago = lstEsquemasPagos.Where(w => w.id == itemDet.FK_EsquemaPago).FirstOrDefault();
                        if (objEsquemaPago == null)
                            throw new Exception("Ocurrió un error al obtener el esquema de pago del puesto.");
                        #endregion

                        // SE ARMA PUESTO CON SUS TABULADORES
                        if (objParamDTO.lstPersonalNecesarioDTO != null)
                        {
                            string cantPersonalNecesario = (objParamDTO.lstPersonalNecesarioDTO.Where(w => w.FK_Puesto == item.FK_Puesto).Select(s => s.cantPersonalNecesario).FirstOrDefault()).ToString();
                            objTabuladorDTO.personalNecesario = string.Format(@"<input type='text' class='form-control personalNecesario' style='padding: 40% 0%; 
                                                                                text-align: center; font-size: 31px; color: black;' id='txtCEPersonalNecesario{0}' value='{1}'>",
                                                                                item.FK_Puesto, cantPersonalNecesario);
                        }
                        else
                        {
                            objTabuladorDTO.personalNecesario = string.Format(@"<input type='text' class='form-control personalNecesario' style='padding: 40% 0%; 
                                                                                text-align: center; font-size: 31px; color: black;' id='txtCEPersonalNecesario{0}'>",
                                                                                item.FK_Puesto);
                        }
                        objTabuladorDTO.idPuesto = item.FK_Puesto;
                        objTabuladorDTO.puestoDesc = !string.IsNullOrEmpty(objPuesto.descripcion) ? objPuesto.descripcion.Trim() : string.Empty;
                        objTabuladorDTO.tipoNominaDesc = !string.IsNullOrEmpty(objTipoNomina.descripcion) ? objTipoNomina.descripcion.Trim() : string.Empty;
                        objTabuladorDTO.areaDepartamentoDesc = !string.IsNullOrEmpty(objAreaDepartamento.concepto) ? objAreaDepartamento.concepto.Trim() : string.Empty;
                        objTabuladorDTO.soloDescPuesto = objPuesto.descripcion;
                        objTabuladorDTO.categoriaDesc += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button><br>", !string.IsNullOrEmpty(objCategoria.concepto) ? objCategoria.concepto.Trim() : string.Empty);
                        objTabuladorDTO.sueldoBaseString += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button><br>", itemDet.sueldoBase.ToString("C"));
                        objTabuladorDTO.complementoString += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button><br>", itemDet.complemento.ToString("C"));
                        objTabuladorDTO.totalNominalString += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button><br>", itemDet.totalNominal.ToString("C"));
                        objTabuladorDTO.sueldoMensualString += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button><br>", itemDet.sueldoMensual.ToString("C"));
                        objTabuladorDTO.esquemaPagoDescString += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button><br>", !string.IsNullOrEmpty(objEsquemaPago.concepto) ? objEsquemaPago.concepto.Trim() : string.Empty);
                    }
                    lstTabuladoresDTO.Add(objTabuladorDTO);
                }

                lstTabuladoresDTO = lstTabuladoresDTO.OrderBy(e => e.soloDescPuesto).ToList();

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("lstTabuladoresDTO", lstTabuladoresDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetTabuladoresPendientes", e, AccionEnum.CONSULTA, objParamDTO.id, objParamDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboFiltroCC_PlantillaPersonal(TabuladorDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region FILL COMBO CC CON LINEA DE NEGOCIO
                List<string> lstCC = _context.tblC_Nom_CatalogoCC.Where(w => w.estatus).Select(s => s.cc).ToList();
                List<string> lstCC_NoDisponibles = _context.tblRH_TAB_PlantillasPersonal.Where(w => (w.plantillaAutorizada == EstatusGestionAutorizacionEnum.AUTORIZADO ||
                                                                                                     w.plantillaAutorizada == EstatusGestionAutorizacionEnum.PENDIENTE) && w.registroActivo).Select(s => s.cc).ToList();
                lstCC = lstCC.Where(w => !lstCC_NoDisponibles.Contains(w)).ToList();
                List<tblC_Nom_CatalogoCC> lstCCDisponibles = _context.tblC_Nom_CatalogoCC.Where(w => lstCC.Contains(w.cc) && w.estatus).ToList();

                List<tblC_Nom_CatalogoCC> lstCCConLineaNegocios = new List<tblC_Nom_CatalogoCC>();
                foreach (var item in lstCCDisponibles)
                {
                    List<tblRH_TAB_CatLineaNegocioDet> cantLN = _context.tblRH_TAB_CatLineaNegocioDet.Where(w => w.cc == item.cc && w.registroActivo).ToList();
                    if (cantLN.Count() > 0)
                    {
                        tblC_Nom_CatalogoCC objCC = lstCCDisponibles.Where(w => w.cc == item.cc).FirstOrDefault();
                        int cant = lstCCConLineaNegocios.Where(w => w.cc == item.cc).Count();
                        if (cant <= 0)
                            lstCCConLineaNegocios.Add(objCC);
                    }
                }

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstCCConLineaNegocios)
                {
                    if (!string.IsNullOrEmpty(item.cc) && !string.IsNullOrEmpty(item.ccDescripcion))
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = item.cc.ToString();
                        objComboDTO.Text = string.Format("[{0}] {1}", item.cc.Trim(), item.ccDescripcion.Trim());
                        lstComboDTO.Add(objComboDTO);
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboFiltroCC", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Clear();
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> CrearSolicitudPlantilla(TabuladorDTO objParamDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado.Clear();
                try
                {
                    #region VALIDACIONES
                    if (string.IsNullOrEmpty(objParamDTO.cc)) { throw new Exception("Es necesario seleccionar un CC."); }
                    if (objParamDTO.FK_LineaNegocio <= 0) { throw new Exception("Es necesario seleccionar una línea de negocio."); }
                    if (objParamDTO.fechaInicio == null) { throw new Exception("Es necesario indicar la fecha inicio."); }
                    if (objParamDTO.fechaFin == null) { throw new Exception("Es necesario indicar la fecha fin."); }

                    foreach (var item in objParamDTO.lstPersonalNecesario)
                        if (item <= 0) { throw new Exception("Es necesario indicar el personal necesario para cada puesto (1 o mas)."); }

                    foreach (var item in objParamDTO.lstPuestosID)
                        if (item <= 0) { throw new Exception("Ocurrió un error al registrar la solicitud."); }

                    bool tieneAutorizante = false;
                    foreach (var item in objParamDTO.lstGestionAutorizantesDTO)
                    {
                        if (item.FK_UsuarioAutorizacion > 0)
                            tieneAutorizante = true;
                    }
                    if (!tieneAutorizante) { throw new Exception("Es necesario indicar al menos un autorizante."); }
                    #endregion

                    #region SE REGISTRA SOLICITUD DE PLANTILLA
                    // SE REGISTRA SOLICITUD DE PLANTILLA
                    tblRH_TAB_PlantillasPersonal objRegistrarPlantilla = new tblRH_TAB_PlantillasPersonal();
                    objRegistrarPlantilla.cc = objParamDTO.cc.Trim();
                    objRegistrarPlantilla.FK_LineaNegocio = objParamDTO.FK_LineaNegocio;
                    objRegistrarPlantilla.plantillaAutorizada = (int)EstatusGestionAutorizacionEnum.PENDIENTE;
                    objRegistrarPlantilla.fechaInicio = objParamDTO.fechaInicio;
                    objRegistrarPlantilla.fechaFin = objParamDTO.fechaFin;
                    objRegistrarPlantilla.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objRegistrarPlantilla.fechaCreacion = DateTime.Now;
                    objRegistrarPlantilla.registroActivo = true;
                    _context.tblRH_TAB_PlantillasPersonal.Add(objRegistrarPlantilla);
                    _context.SaveChanges();

                    // SE OBTIENE ID DEL REGISTRO PRINCIPAL
                    int FK_Plantilla = _context.tblRH_TAB_PlantillasPersonal.Where(w => w.registroActivo).Select(s => s.id).OrderByDescending(o => o).FirstOrDefault();

                    // SE REGISTRA EL DETALLE DE LA SOLICITUD
                    List<tblRH_TAB_PlantillasPersonalDet> lstRegistrarDet = new List<tblRH_TAB_PlantillasPersonalDet>();
                    tblRH_TAB_PlantillasPersonalDet objRegistrarDet = new tblRH_TAB_PlantillasPersonalDet();
                    for (int i = 0; i < objParamDTO.lstPuestosID.Count(); i++)
                    {
                        if (objParamDTO.lstPersonalNecesario[i] > 0)
                        {
                            objRegistrarDet = new tblRH_TAB_PlantillasPersonalDet();
                            objRegistrarDet.FK_Plantilla = FK_Plantilla;
                            objRegistrarDet.FK_Puesto = objParamDTO.lstPuestosID[i];
                            objRegistrarDet.personalNecesario = objParamDTO.lstPersonalNecesario[i];
                            objRegistrarDet.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objRegistrarDet.fechaCreacion = DateTime.Now;
                            objRegistrarDet.registroActivo = true;
                            lstRegistrarDet.Add(objRegistrarDet);
                        }
                    }
                    _context.tblRH_TAB_PlantillasPersonalDet.AddRange(lstRegistrarDet);
                    _context.SaveChanges();

                    // SE REGISTRA A LOS AUTORIZANTES
                    List<tblRH_TAB_GestionAutorizantes> lstGestion = new List<tblRH_TAB_GestionAutorizantes>();
                    tblRH_TAB_GestionAutorizantes objGestion = new tblRH_TAB_GestionAutorizantes();
                    foreach (var item in objParamDTO.lstGestionAutorizantesDTO)
                    {
                        objGestion = new tblRH_TAB_GestionAutorizantes();
                        objGestion.FK_Registro = FK_Plantilla;
                        objGestion.vistaAutorizacion = VistaAutorizacionEnum.PLANTILLAS_PERSONAL;
                        objGestion.nivelAutorizante = item.nivelAutorizante;
                        objGestion.FK_UsuarioAutorizacion = item.FK_UsuarioAutorizacion;
                        objGestion.autorizado = (int)EstatusGestionAutorizacionEnum.PENDIENTE;
                        objGestion.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objGestion.fechaCreacion = DateTime.Now;
                        objGestion.registroActivo = true;
                        lstGestion.Add(objGestion);
                    }
                    _context.tblRH_TAB_GestionAutorizantes.AddRange(lstGestion);
                    _context.SaveChanges();

                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha registrado la solicitud con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, objParamDTO.id, JsonUtils.convertNetObjectToJson(objParamDTO));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.AGREGAR, objParamDTO.id, objParamDTO);
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> NotificarPlantilla(string ccPlantilla, bool esAuthCompleta)
        {
            resultado.Clear();
            try
            {
                tblRH_TAB_PlantillasPersonal objPlantilla = _context.tblRH_TAB_PlantillasPersonal.Where(w => w.registroActivo && w.cc == ccPlantilla).FirstOrDefault();
                List<tblRH_TAB_GestionAutorizantes> lstAutorizantes = _context.tblRH_TAB_GestionAutorizantes.Where(w => w.registroActivo && w.vistaAutorizacion == VistaAutorizacionEnum.PLANTILLAS_PERSONAL &&
                                                                                                                        w.FK_Registro == objPlantilla.id).ToList();

                var lstAutorizantesOrdenAutorizantes = new List<tblRH_TAB_GestionAutorizantes>();
                lstAutorizantesOrdenAutorizantes.AddRange(lstAutorizantes.Where(w => w.nivelAutorizante == NivelAutorizanteEnum.RESPONSABLE_CC));
                lstAutorizantesOrdenAutorizantes.AddRange(lstAutorizantes.Where(w => w.nivelAutorizante == NivelAutorizanteEnum.DIRECTOR_LINEA_NEGOCIOS));
                lstAutorizantesOrdenAutorizantes.AddRange(lstAutorizantes.Where(w => w.nivelAutorizante == NivelAutorizanteEnum.ALTA_DIRECCION));
                lstAutorizantesOrdenAutorizantes.AddRange(lstAutorizantes.Where(w => w.nivelAutorizante == NivelAutorizanteEnum.CAPITAL_HUMANO));

                #region SE NOTIFICA A LOS AUTORIZANTES QUE SE HA REGISTRADO UNA PLANTILLA NUEVA PARA SER AUTORIZADA
                List<string> lstCorreosNotificar = new List<string>();
                List<string> lstCorreosNotificarRestantes = new List<string>();

                bool esSigAuth = false;
                int numSig = 0;

                foreach (var item in lstAutorizantesOrdenAutorizantes)
                {
                    int FK_UsuarioAutorizacion = item.FK_UsuarioAutorizacion;
                    tblP_Usuario objUsuario = _context.tblP_Usuario.Where(w => w.id == FK_UsuarioAutorizacion).FirstOrDefault();
                    if (objUsuario == null)
                        throw new Exception("Ocurrió un error al notificar a los autorizantes.");

                    string correo = objUsuario.correo;
                    if (string.IsNullOrEmpty(correo))
                    {
                        string mensajeError = string.Format("El autorizante [{0} {1}], no cuenta con correo registrado. Favor de notificar a TI.", objUsuario.nombre.Trim(), objUsuario.apellidoPaterno.Trim());
                        throw new Exception(mensajeError);
                    }

                    if (item.autorizado == EstatusGestionAutorizacionEnum.PENDIENTE && esSigAuth && numSig == 0)
                    {
                        lstCorreosNotificarRestantes.Add(correo);
                        numSig++;
                    }

                    if (item.autorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && vSesiones.sesionUsuarioDTO.id == item.FK_UsuarioAutorizacion)
                    {
                        esSigAuth = true;
                    }
                }

                lstCorreosNotificar.Add(_CORREO_DIANA_ALVAREZ);
                lstCorreosNotificar.Add(_CORREO_MANUEL_CRUZ);
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia)
                    lstCorreosNotificar.Add(_CORREO_ALEXANDRA_GOMEZ);
#if DEBUG
                lstCorreosNotificar = new List<string>();
                lstCorreosNotificar.Add(_CORREO_OMAR_NUNEZ);
#endif
                var objCC = _context.tblC_Nom_CatalogoCC.FirstOrDefault(e => e.estatus && e.cc == objPlantilla.cc.Trim());
                var downloadPDF = vSesiones.downloadPDF;
                var tipoFormato = "PlantillaPersonal.pdf";

                string asuntoCorreo = ("NUEVA PLANTILLA PARA EL CC [" + objCC.cc + "] " + objCC.ccDescripcion);
                string cuerpoCorreo = "Buen día,<br><br>Se ha registrado una nueva plantilla, se encuentra listo para su gestión.<br><br>" +
                                                     htmlCorreo(lstAutorizantesOrdenAutorizantes) +
                                                     "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                                                     "Capital Humano > Administración de personal > Configuración > Estructura Salarial > Gestión de plantillas.<br><br>" +
                                                     "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                                                     "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias.";

                if (esAuthCompleta)
                {
                    asuntoCorreo = ("PLANTILLA AUTORIZADA PARA EL CC [" + objCC.cc + "] " + objCC.ccDescripcion);
                    cuerpoCorreo = "Buen día,<br><br>Se ha autorizado una plantilla en el CC " + objCC.cc + " " + objCC.ccDescripcion + "por todos los firmantes.<br><br>" +
                                                  htmlCorreo(lstAutorizantesOrdenAutorizantes) +
                                        "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                                        "Capital Humano > Administración de personal > Configuración > Estructura Salarial > Gestión de plantillas.<br><br>" +
                                        "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                                        "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias.";
                }
                else
                {
                    #region Alerta SIGOPLAN
                    //PRIMER AUTORIZANTE
                    var objReponsableCC = lstAutorizantes.FirstOrDefault(e => e.nivelAutorizante == NivelAutorizanteEnum.RESPONSABLE_CC);

                    tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                    objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                    objNuevaAlerta.userRecibeID = objReponsableCC.FK_UsuarioAutorizacion;
#if DEBUG
                    objNuevaAlerta.userRecibeID = _USUARIO_OMAR_NUNEZ_ID; // OMAR NUÑEZ
#endif
                    objNuevaAlerta.tipoAlerta = 2;
                    objNuevaAlerta.sistemaID = 16;
                    objNuevaAlerta.visto = false;
                    objNuevaAlerta.url = "/Administrativo/Tabuladores/GestionPlantillasPersonal";
                    objNuevaAlerta.objID = objPlantilla.id;
                    objNuevaAlerta.obj = "AutorizacionPlantillaTabulador";
                    objNuevaAlerta.msj = string.Format("Plantilla Pendiente de Autorizar - CC: {0}", objPlantilla.cc);
                    objNuevaAlerta.documentoID = 0;
                    objNuevaAlerta.moduloID = 0;
                    _context.tblP_Alerta.Add(objNuevaAlerta);
                    _context.SaveChanges();
                    #endregion //ALERTA SIGPLAN

                    if (!esSigAuth)
                    {
                        tblP_Usuario objUsuario = _context.tblP_Usuario.Where(w => w.id == objReponsableCC.FK_UsuarioAutorizacion).FirstOrDefault();
                        if (objUsuario == null)
                            throw new Exception("Ocurrió un error al notificar a los autorizantes.");
                        lstCorreosNotificar.Add(objUsuario.correo);
                    }
                    else
                    {
                        lstCorreosNotificar.AddRange(lstCorreosNotificarRestantes);
                    }

#if DEBUG
                    lstCorreosNotificar = new List<string>();
                    lstCorreosNotificar.Add(_CORREO_OMAR_NUNEZ);
#endif
                    asuntoCorreo = ("AUTORIZACIÓN O NUEVA PLANTILLA PARA EL CC[" + objCC.cc + "] " + objCC.ccDescripcion);
                    cuerpoCorreo = "Buen día,<br><br>Se ha registrado una nueva plantilla, se encuentra en proceso de autorización .<br><br>" +
                                                         htmlCorreo(lstAutorizantesOrdenAutorizantes) +
                                                         "<br><br>Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                                                         "Capital Humano > Administración de personal > Configuración > Estructura Salarial > Gestión de plantillas.<br><br>" +
                                                         "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                                                         "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias.";
                }

                GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asuntoCorreo), cuerpoCorreo, lstCorreosNotificar.Distinct().ToList(), downloadPDF, tipoFormato);

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "NotificarPlantilla", e, AccionEnum.CORREO, 0, 0);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public string htmlCorreo(List<tblRH_TAB_GestionAutorizantes> lstAutorizadores)
        {


            string html = "";
            contextSigoplan db = new contextSigoplan();
            //html += "<style>h3 {text-align: center;}table.dataTable tbody tr td, table thead tr th, table.dataTable, .dataTables_scrollBody {";
            //html += "border: 1px solid}table.dataTable thead {font-size: 18px;background-color:  white;color: black;}";
            //html += "</style>";

            html += "<style>";
            html += "table, th, td {";
            html += "border: 1px solid grey;";
            html += "border-collapse: collapse;";
            html += "}";
            html += "th {";
            html += "text-align: center;";
            html += "}";
            html += "td {";
            html += "text-align: center;";
            html += "}";
            html += "</style>";


            html += "<br><table style='width:100%'>";
            html += "<thead>";
            html += "<tr>";

            html += "<th>Nombre Autorizador</th>";
            html += "<th>Tipo</th>";
            html += "<th>Autorizó</th>";

            html += "</tr>";
            html += "</thead>";
            html += "<tbody>";

            foreach (var item in lstAutorizadores)
            {
                var tipo = "";
                var status = "";
                var color = "";
                int autorizante = item.FK_UsuarioAutorizacion;
                switch (item.autorizado)
                {
                    case EstatusGestionAutorizacionEnum.AUTORIZADO:
                        status = "AUTORIZADO";
                        color = "#82e0aa";
                        break;
                    case EstatusGestionAutorizacionEnum.PENDIENTE:
                        status = "PENDIENTE";
                        color = "#f08024";
                        break;
                    case EstatusGestionAutorizacionEnum.RECHAZADO:
                        status = "RECHAZADO";
                        color = "#bd1111";
                        break;
                }
                switch (item.nivelAutorizante)
                {
                    case NivelAutorizanteEnum.ALTA_DIRECCION:
                        tipo = "ALTA_DIRECCION";
                        break;
                    case NivelAutorizanteEnum.CAPITAL_HUMANO:
                        tipo = "CAPITAL_HUMANO";
                        break;
                    case NivelAutorizanteEnum.DIRECTOR_LINEA_NEGOCIOS:
                        tipo = "DIRECTOR_LINEA_NEGOCIOS";
                        break;
                    case NivelAutorizanteEnum.GERENTE_SUBDIRECTOR_DIRECTOAREA:
                        tipo = "GERENTE_SUBDIRECTOR_DIRECTOAREA";
                        break;
                    case NivelAutorizanteEnum.RESPONSABLE_CC:
                        tipo = "RESPONSABLE_CC";
                        break;
                    case NivelAutorizanteEnum.SOLICITANTE:
                        tipo = "SOLICITANTE";
                        break;
                }
                html += "<tr>";
                html += "<td>" + _context.tblP_Usuario.Where(r => r.id == autorizante).Select(y => y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno).FirstOrDefault() + "</td>";
                html += "<td>" + tipo + "</td>";
                html += "<td style='background-color:" + color + ";'>" + status + "</td>";
                html += "</tr>";

            }

            html += "</tbody>";
            html += "</table>";
            html += "</div>";

            return html;
        }

        public Dictionary<string, object> FillCboFiltroPuestos_PlantillaPersonal(TabuladorDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region FILL COMBO PUESTOS RELACIONADOS A LA LINEA DE NEGOCIO
                List<int> lstFK_Tabuladores = _context.tblRH_TAB_TabuladoresDet.Where(w => w.tabuladorDetAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.FK_LineaNegocio == objParamDTO.FK_LineaNegocio && w.registroActivo).Select(s => s.FK_Tabulador).ToList();
                List<int> lstFK_Puestos = _context.tblRH_TAB_Tabuladores.Where(w => w.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && lstFK_Tabuladores.Contains(w.id) && w.registroActivo).Select(s => s.FK_Puesto).ToList();
                List<tblRH_EK_Puestos> lstPuestos = _context.tblRH_EK_Puestos.Where(w => lstFK_Puestos.Contains(w.puesto) && w.registroActivo).OrderBy(o => o.descripcion).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstPuestos)
                {
                    tblRH_EK_Puestos objPuesto = lstPuestos.Where(w => w.puesto == item.puesto).FirstOrDefault();
                    if (objPuesto != null)
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = objPuesto.puesto.ToString();
                        objComboDTO.Text = !string.IsNullOrEmpty(objPuesto.descripcion) ? ("[" + objPuesto.puesto + "] " + objPuesto.descripcion.Trim()) : string.Empty;
                        lstComboDTO.Add(objComboDTO);
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboFiltroPuestos_PlantillaPersonal", e, AccionEnum.FILLCOMBO, objParamDTO.id, objParamDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboFiltroLineaNegocios_PlantillaPersonal(TabuladorDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region FILL CBO
                List<int> FK_LineaNegocio = _context.tblRH_TAB_CatLineaNegocioDet.Where(w => w.cc == objParamDTO.cc && w.registroActivo).Select(s => s.FK_LineaNegocio).ToList();
                List<tblRH_TAB_CatLineaNegocio> lstCatNegocios = _context.tblRH_TAB_CatLineaNegocio.Where(w => w.registroActivo).OrderBy(o => o.concepto).ToList();
                if (FK_LineaNegocio.Count > 0)
                    lstCatNegocios = lstCatNegocios.Where(w => FK_LineaNegocio.Contains(w.id)).ToList();

                if (FK_LineaNegocio.Count() <= 0)
                {
                    tblC_Nom_CatalogoCC objCC = _context.tblC_Nom_CatalogoCC.Where(w => w.cc == objParamDTO.cc && w.estatus).FirstOrDefault();
                    if (objCC == null) { throw new Exception("Ocurrió un error al obtener la información del CC."); }
                    string mensajeError = string.Format("El CC [{0}] {1}, no cuenta con una línea de negocio asignada.", objCC.cc.Trim(), objCC.ccDescripcion.Trim());
                    throw new Exception(mensajeError);
                }

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstCatNegocios)
                {
                    if (item.id > 0 && !string.IsNullOrEmpty(item.concepto))
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = item.id.ToString();
                        objComboDTO.Text = item.concepto.Trim();
                        lstComboDTO.Add(objComboDTO);
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboLineaNegocios", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Clear();
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #region GESTIÓN PLANTILLAS PERSONAL
        public Dictionary<string, object> GetGestionPlantillasPersonal(PlantillaPersonalDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region CATALOGOS
                List<tblC_Nom_CatalogoCC> lstCC = _context.tblC_Nom_CatalogoCC.Where(w => w.estatus).ToList();
                List<tblRH_EK_Puestos> lstPuestos = _context.tblRH_EK_Puestos.Where(w => w.registroActivo).ToList();
                List<tblRH_EK_Empleados> lstEmpleados = _context.tblRH_EK_Empleados.Where(w => w.estatus_empleado == "A").ToList();
                List<tblRH_EK_Plantilla_Aditiva> lstPlantillaAditivas = _context.tblRH_EK_Plantilla_Aditiva.Where(w => w.estatus == "A").ToList();
                List<tblRH_TAB_GestionAutorizantes> lstGestionAutorizantes = _context.tblRH_TAB_GestionAutorizantes.Where(w => w.vistaAutorizacion == VistaAutorizacionEnum.PLANTILLAS_PERSONAL && w.registroActivo).OrderBy(e => e.nivelAutorizante).ToList();
                #endregion

                #region SE OBTIENE LISTADO DE GESTIÓN DE PLANTILLAS DE PERSONAL
                List<tblRH_TAB_PlantillasPersonal> lstPlantillas = _context.tblRH_TAB_PlantillasPersonal.Where(w => w.plantillaAutorizada == objParamDTO.plantillaAutorizada && w.registroActivo).ToList();
                List<tblRH_TAB_PlantillasPersonalDet> lstPlantillasDet = _context.tblRH_TAB_PlantillasPersonalDet.Where(w => w.registroActivo).ToList();

                List<PlantillaPersonalDTO> lstPlantillasPersonalDTO = new List<PlantillaPersonalDTO>();
                PlantillaPersonalDTO objPlantillaPersonalDTO = new PlantillaPersonalDTO();
                foreach (var item in lstPlantillas)
                {
                    tblC_Nom_CatalogoCC objCC = lstCC.Where(w => w.cc == item.cc).FirstOrDefault();
                    tblRH_TAB_PlantillasPersonalDet objPlantillaDet = lstPlantillasDet.Where(w => w.FK_Plantilla == item.id).FirstOrDefault();
                    List<tblRH_TAB_PlantillasPersonalDet> lstPlantillaDet = lstPlantillasDet.Where(w => w.FK_Plantilla == item.id).ToList();
                    var lstPlantillaDet_puestos = lstPlantillaDet.Select(x => x.FK_Puesto).ToList();
                    List<tblRH_EK_Plantilla_Aditiva> lstAditivas = lstPlantillaAditivas.Where(x => x.cc == item.cc && x.estatus == "A" && lstPlantillaDet_puestos.Contains(x.puesto)).ToList();

                    if (objPlantillaDet != null)
                    {
                        int puestoID = objPlantillaDet.FK_Puesto;
                        tblRH_EK_Puestos objPuesto = lstPuestos.Where(w => w.puesto == puestoID).FirstOrDefault();
                        if (objCC != null && objPuesto != null)
                        {
                            objPlantillaPersonalDTO = new PlantillaPersonalDTO();
                            List<tblRH_TAB_GestionAutorizantes> lstAutorizantes = lstGestionAutorizantes.Where(w => w.registroActivo && w.vistaAutorizacion == VistaAutorizacionEnum.PLANTILLAS_PERSONAL && w.FK_Registro == item.id).OrderBy(e => e.nivelAutorizante).ToList();

                            var lstAutorizantesOrdered = new List<tblRH_TAB_GestionAutorizantes>();

                            //INSERTAR EN EL ORDEN CORRECTO LOS AUTORIZANTES
                            lstAutorizantesOrdered.AddRange(lstAutorizantes.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.RESPONSABLE_CC));
                            lstAutorizantesOrdered.AddRange(lstAutorizantes.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.DIRECTOR_LINEA_NEGOCIOS));
                            lstAutorizantesOrdered.AddRange(lstAutorizantes.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.ALTA_DIRECCION));
                            lstAutorizantesOrdered.AddRange(lstAutorizantes.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.CAPITAL_HUMANO));

                            int? sigAuth = null;

                            foreach (var itemAuth in lstAutorizantesOrdered)
                            {
                                if (itemAuth.autorizado == EstatusGestionAutorizacionEnum.AUTORIZADO || itemAuth.autorizado == EstatusGestionAutorizacionEnum.RECHAZADO)
                                    objPlantillaPersonalDTO.esFirmar = false;
                                else
                                {
                                    if (sigAuth == null)
                                        sigAuth = itemAuth.FK_UsuarioAutorizacion;

                                    if (sigAuth.Value == vSesiones.sesionUsuarioDTO.id)
                                    {
                                        objPlantillaPersonalDTO.esFirmar = true;
                                        break;
                                    }
                                    else
                                        objPlantillaPersonalDTO.esFirmar = false;
                                }
                            }

                            if (!string.IsNullOrEmpty(objCC.cc) && !string.IsNullOrEmpty(objCC.ccDescripcion))
                            {
                                objPlantillaPersonalDTO.id = item.id;
                                objPlantillaPersonalDTO.cc = string.Format("[{0}] {1}", objCC.cc.Trim(), objCC.ccDescripcion.Trim());
                                objPlantillaPersonalDTO.codeCC = objCC.cc;
                                objPlantillaPersonalDTO.puestoDesc = objPuesto.descripcion.Trim();
                                objPlantillaPersonalDTO.plantillaAutorizada = item.plantillaAutorizada;
                                objPlantillaPersonalDTO.comentarioRechazo = item.comentarioRechazo;
                                objPlantillaPersonalDTO.personalNecesario = lstPlantillaDet.Sum(x => x.personalNecesario) + lstAditivas.Select(x => x.cantidad).Sum();

                                var lstPuestosPlantilla = lstPlantillaDet.Select(x => x.FK_Puesto).ToList();

                                objPlantillaPersonalDTO.personalExistente = lstEmpleados.Where(x => x.estatus_empleado == "A" && x.cc_contable == item.cc && (x.puesto != null ? lstPuestosPlantilla.Contains((int)x.puesto) : false)).ToList().Count();
                                objPlantillaPersonalDTO.porContratar =
                                    (objPlantillaPersonalDTO.personalNecesario - objPlantillaPersonalDTO.personalExistente) >= 0 ? (objPlantillaPersonalDTO.personalNecesario - objPlantillaPersonalDTO.personalExistente) : 0;
                                lstPlantillasPersonalDTO.Add(objPlantillaPersonalDTO);
                            }
                        }
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("lstPlantillasPersonalDTO", lstPlantillasPersonalDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetGestionPlantillasPersonal", e, AccionEnum.CONSULTA, objParamDTO.id, objParamDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> AutorizarRechazarPlantillaPersonal(PlantillaPersonalDTO objParamDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado.Clear();
                try
                {
                    #region VALIDACIONES
                    if (objParamDTO.id <= 0) { throw new Exception("Ocurrió un error al realizar la acción."); }
                    if (objParamDTO.plantillaAutorizada <= 0) { throw new Exception("Ocurrió un error al realizar la acción."); }
                    #endregion

                    #region SE AUTORIZA/RECHAZA LA PLANTILLA

                    var lstFirmas = _context.tblRH_TAB_GestionAutorizantes.Where(e => e.registroActivo && e.vistaAutorizacion == VistaAutorizacionEnum.PLANTILLAS_PERSONAL && e.FK_Registro == objParamDTO.id).ToList();
                    tblRH_TAB_PlantillasPersonal objPlantilla = _context.tblRH_TAB_PlantillasPersonal.FirstOrDefault(e => e.registroActivo && e.id == objParamDTO.id);
                    var objCC = _context.tblC_Nom_CatalogoCC.FirstOrDefault(e => e.cc == objPlantilla.cc);

                    var lstAutorizantesOrdered = new List<tblRH_TAB_GestionAutorizantes>();

                    //INSERTAR EN EL ORDEN CORRECTO LOS AUTORIZANTES

                    lstAutorizantesOrdered.AddRange(lstFirmas.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.RESPONSABLE_CC));
                    lstAutorizantesOrdered.AddRange(lstFirmas.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.DIRECTOR_LINEA_NEGOCIOS));
                    lstAutorizantesOrdered.AddRange(lstFirmas.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.ALTA_DIRECCION));
                    lstAutorizantesOrdered.AddRange(lstFirmas.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.CAPITAL_HUMANO));

                    int totalAuth = 0;
                    bool notifyNextAuth = false;

                    foreach (var item in lstAutorizantesOrdered)
                    {

                        #region AGREGAR ALERTA PARA EL SIGUIENTE AUTORIZANTE
                        if (notifyNextAuth && objParamDTO.plantillaAutorizada == EstatusGestionAutorizacionEnum.AUTORIZADO)
                        {
                            #region Alerta SIGOPLAN
                            tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                            objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                            objNuevaAlerta.userRecibeID = item.FK_UsuarioAutorizacion;
#if DEBUG
                            objNuevaAlerta.userRecibeID = _USUARIO_OMAR_NUNEZ_ID; //USUARIO ID: Omar Nuñez.
                            //objNuevaAlerta.userRecibeID = 3807; //USUARIO ID: OSKAR НАХУЙ БЛЯТЬ.
#endif
                            objNuevaAlerta.tipoAlerta = 2;
                            objNuevaAlerta.sistemaID = 16;
                            objNuevaAlerta.visto = false;
                            objNuevaAlerta.url = "/Administrativo/Tabuladores/GestionPlantillasPersonal";
                            objNuevaAlerta.objID = objPlantilla.id;
                            objNuevaAlerta.obj = "AutorizacionPlantillaTabulador";
                            objNuevaAlerta.msj = "Plantilla Pendiente de Autorizar - CC: " + objPlantilla.cc;
                            objNuevaAlerta.documentoID = 0;
                            objNuevaAlerta.moduloID = 0;
                            _context.tblP_Alerta.Add(objNuevaAlerta);
                            _context.SaveChanges();
                            #endregion //ALERTA SIGPLAN

                            //NOTIFICADA
                            notifyNextAuth = false;
                        }

                        #endregion

                        if (item.autorizado == EstatusGestionAutorizacionEnum.AUTORIZADO)
                        {
                            totalAuth++;
                        }
                        else
                        {
                            if (item.FK_UsuarioAutorizacion == vSesiones.sesionUsuarioDTO.id)
                            {
                                if (objParamDTO.plantillaAutorizada == EstatusGestionAutorizacionEnum.AUTORIZADO)
                                {
                                    totalAuth++;
                                    notifyNextAuth = true;

                                    var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == item.FK_UsuarioAutorizacion && e.visto == false && e.objID == objPlantilla.id && e.obj == "AutorizacionPlantillaTabulador");
                                    if (objAlerta != null)
                                    {
                                        objAlerta.visto = true;
                                        _context.SaveChanges();
                                    }
                                }
                                else
                                {
                                    var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == item.FK_UsuarioAutorizacion && e.visto == false && e.objID == objPlantilla.id && e.obj == "AutorizacionPlantillaTabulador");
                                    if (objAlerta != null)
                                    {
                                        objAlerta.visto = true;
                                        _context.SaveChanges();
                                    }
                                }

                                item.autorizado = objParamDTO.plantillaAutorizada;
                                item.fechaModificacion = objParamDTO.fechaModificacion;
                                item.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                                item.fechaFirma = DateTime.Now;
                                _context.SaveChanges();
                            }
                        }
                    }

                    bool esAuthCompleta = false;
                    if (totalAuth == lstFirmas.Count())
                    {
                        esAuthCompleta = true;
                        objPlantilla.plantillaAutorizada = objParamDTO.plantillaAutorizada;
                        objPlantilla.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objPlantilla.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();
                    }
                    else
                    {
                        if (objParamDTO.plantillaAutorizada == EstatusGestionAutorizacionEnum.RECHAZADO)
                        {
                            objPlantilla.plantillaAutorizada = objParamDTO.plantillaAutorizada;
                            objPlantilla.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objPlantilla.fechaModificacion = DateTime.Now;
                            _context.SaveChanges();
                        }
                    }

                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add("esAuthCompleta", esAuthCompleta);
                    resultado.Add(MESSAGE, objParamDTO.plantillaAutorizada == EstatusGestionAutorizacionEnum.AUTORIZADO ? "Se ha autorizado con éxito." : "Se ha rechazado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objParamDTO.id, JsonUtils.convertNetObjectToJson(objParamDTO));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "AutorizarRechazarPlantillasPersonal", e, AccionEnum.ACTUALIZAR, objParamDTO.id, objParamDTO);
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetLstAutorizantesPlantilla(int idPlantilla)
        {
            resultado.Clear();

            using (var _ctx = new MainContext())
            {
                try
                {
                    List<tblRH_TAB_GestionAutorizantes> lstAutorizantes = _context.tblRH_TAB_GestionAutorizantes.Where(w => w.vistaAutorizacion == VistaAutorizacionEnum.PLANTILLAS_PERSONAL && w.FK_Registro == idPlantilla && w.registroActivo).OrderBy(e => e.nivelAutorizante).ToList();
                    GestionAutorizanteDTO objAutorizanteDTO = new GestionAutorizanteDTO();
                    List<GestionAutorizanteDTO> lstAutorizantesDTO = new List<GestionAutorizanteDTO>();
                    foreach (var item in lstAutorizantes)
                    {
                        int usuarioYaEnLista = lstAutorizantesDTO.Where(w => w.FK_UsuarioAutorizacion == item.FK_UsuarioAutorizacion).Count();

                        if (usuarioYaEnLista <= 0)
                        {
                            tblP_Usuario objUsuario = _context.tblP_Usuario.Where(w => w.id == item.FK_UsuarioAutorizacion && w.estatus).FirstOrDefault();
                            if (objUsuario == null)
                                throw new Exception("Ocurrió un error al obtener el nombre del autorizante.");

                            objAutorizanteDTO = new GestionAutorizanteDTO();
                            objAutorizanteDTO.id = item.id;
                            objAutorizanteDTO.nombreAutorizante = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(string.Format("{0} {1} {2}", objUsuario.nombre, objUsuario.apellidoPaterno, objUsuario.apellidoMaterno));
                            objAutorizanteDTO.FK_Registro = item.FK_Registro;
                            objAutorizanteDTO.vistaAutorizacion = item.vistaAutorizacion;
                            objAutorizanteDTO.nivelAutorizante = item.nivelAutorizante;
                            objAutorizanteDTO.FK_UsuarioAutorizacion = item.FK_UsuarioAutorizacion;
                            objAutorizanteDTO.autorizado = item.autorizado;
                            objAutorizanteDTO.comentario = item.comentario;
                            objAutorizanteDTO.FK_UsuarioCreacion = item.FK_UsuarioCreacion;
                            objAutorizanteDTO.FK_UsuarioModificacion = item.FK_UsuarioModificacion;
                            objAutorizanteDTO.fechaCreacion = item.fechaCreacion;
                            objAutorizanteDTO.fechaModificacion = item.fechaModificacion;
                            lstAutorizantesDTO.Add(objAutorizanteDTO);
                        }
                    }

                    //INSERTAR EN EL ORDEN CORRECTO LOS AUTORIZANTES
                    var lstAutorizantesOrdenAutorizantes = new List<GestionAutorizanteDTO>();
                    lstAutorizantesOrdenAutorizantes = new List<GestionAutorizanteDTO>();
                    lstAutorizantesOrdenAutorizantes.AddRange(lstAutorizantesDTO.Where(w => w.nivelAutorizante == NivelAutorizanteEnum.RESPONSABLE_CC));
                    lstAutorizantesOrdenAutorizantes.AddRange(lstAutorizantesDTO.Where(w => w.nivelAutorizante == NivelAutorizanteEnum.DIRECTOR_LINEA_NEGOCIOS));
                    lstAutorizantesOrdenAutorizantes.AddRange(lstAutorizantesDTO.Where(w => w.nivelAutorizante == NivelAutorizanteEnum.ALTA_DIRECCION));
                    lstAutorizantesOrdenAutorizantes.AddRange(lstAutorizantesDTO.Where(w => w.nivelAutorizante == NivelAutorizanteEnum.CAPITAL_HUMANO));

                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, lstAutorizantesOrdenAutorizantes);
                }
                catch (Exception e)
                {
                    resultado.Clear();
                    resultado.Add(MESSAGE, e);
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarComentarioRechazoPlantilla(int idPlantilla, string comentario)
        {
            resultado.Clear();

            using (var _ctx = new MainContext())
            {
                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var objPlantilla = _ctx.tblRH_TAB_PlantillasPersonal.FirstOrDefault(e => e.id == idPlantilla);
                        objPlantilla.comentarioRechazo = comentario;

                        _ctx.SaveChanges();
                        dbContextTransaction.Commit();
                        resultado.Clear();
                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Clear();
                        resultado.Add(MESSAGE, e.Message);
                        resultado.Add(SUCCESS, false);
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetPlantillaDetalle(int plantilla_id)
        {
            resultado.Clear();
            try
            {
                var registroPlantilla = _context.tblRH_TAB_PlantillasPersonal.FirstOrDefault(x => x.id == plantilla_id);
                var listaDetalle = _context.tblRH_TAB_PlantillasPersonalDet.Where(x => x.FK_Plantilla == plantilla_id && x.registroActivo).ToList();
                var listaAditivas = _context.tblRH_EK_Plantilla_Aditiva.Where(x => x.cc == registroPlantilla.cc && x.estatus == "A").ToList();
                var listaEmpleados = _context.tblRH_EK_Empleados.Where(x => x.cc_contable == registroPlantilla.cc && x.estatus_empleado == "A").ToList();
                var listaPuestos = _context.tblRH_EK_Puestos.Where(x => x.registroActivo).ToList();
                var data = new List<PlantillaPersonalDTO>();

                foreach (var det in listaDetalle)
                {
                    var listaAditivasPuesto = listaAditivas.Where(x => x.puesto == det.FK_Puesto).ToList();
                    var listaEmpleadosPuesto = listaEmpleados.Where(x => x.puesto == det.FK_Puesto).ToList();

                    data.Add(new PlantillaPersonalDTO
                    {
                        puestoDesc = listaPuestos.Where(x => x.puesto == det.FK_Puesto).Select(x => "[" + x.puesto + "] " + x.descripcion).FirstOrDefault(),
                        personalNecesario = det.personalNecesario + listaAditivasPuesto.Sum(x => x.cantidad),
                        personalExistente = listaEmpleadosPuesto.Count(),
                        porContratar = (det.personalNecesario + listaAditivasPuesto.Sum(x => x.cantidad) - listaEmpleadosPuesto.Count()) >= 0 ? (det.personalNecesario + listaAditivasPuesto.Sum(x => x.cantidad) - listaEmpleadosPuesto.Count()) : 0
                    });
                }

                resultado.Clear();
                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetPlantillaDetalle", e, AccionEnum.CONSULTA, plantilla_id, plantilla_id);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> GetDetallePlantillaTabuladores(PlantillaPersonalDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region VALIDACIONES
                if (string.IsNullOrEmpty(objParamDTO.cc)) { throw new Exception("Ocurrió un error al obtener el detalle de la plantilla."); }
                #endregion

                #region CATALOGOS
                List<tblRH_TAB_CatAreaDepartamento> lstAreasDepartamentos = _context.tblRH_TAB_CatAreaDepartamento.Where(w => w.registroActivo).ToList();
                List<tblRH_EK_Tipos_Nomina> lstTipoNominas = _context.tblRH_EK_Tipos_Nomina.Where(w => w.esActivo == true).ToList();
                List<tblRH_TAB_CatCategorias> lstCategorias = _context.tblRH_TAB_CatCategorias.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatEsquemaPago> lstEsquemasPagos = _context.tblRH_TAB_CatEsquemaPago.Where(w => w.registroActivo).ToList();
                #endregion

                #region SE OBTIENE LAS PLANTILLAS CON SUS TABULADORES A AUTORIZAR (SE AUTORIZA LA PLANTILLA, NO EL TABULADOR).
                int FK_Plantilla = _context.tblRH_TAB_PlantillasPersonal.Where(w => w.cc == objParamDTO.cc && w.registroActivo).Select(s => s.id).FirstOrDefault();
                if (FK_Plantilla <= 0)
                    throw new Exception("Ocurrió un error al obtener el detalle de la plantilla.");

                List<tblRH_TAB_PlantillasPersonalDet> lstPlantillasPersonalDet = _context.tblRH_TAB_PlantillasPersonalDet.Where(w => w.FK_Plantilla == FK_Plantilla && w.registroActivo).ToList();
                List<int> lstFK_Puestos = lstPlantillasPersonalDet.Where(w => w.FK_Plantilla == FK_Plantilla && w.registroActivo).Select(s => s.FK_Puesto).ToList();
                if (lstFK_Puestos.Count() <= 0)
                    throw new Exception("Ocurrió un error al obtener el detalle de la plantilla.");

                List<tblRH_EK_Puestos> lstPuestos = _context.tblRH_EK_Puestos.Where(w => lstFK_Puestos.Contains(w.puesto) && w.registroActivo).ToList();

                List<tblRH_TAB_Tabuladores> lstTabuladores = _context.tblRH_TAB_Tabuladores.Where(w => lstFK_Puestos.Contains(w.FK_Puesto) && w.registroActivo).ToList();
                List<int> lstFK_Tabuladores = lstTabuladores.Select(s => s.id).ToList();

                tblRH_TAB_CatLineaNegocioDet objLineaNegocio = _context.tblRH_TAB_CatLineaNegocioDet.Where(w => w.cc == objParamDTO.cc && w.registroActivo).FirstOrDefault();

                List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(w => lstFK_Tabuladores.Contains(w.FK_Tabulador) && w.registroActivo && w.FK_LineaNegocio == objLineaNegocio.FK_LineaNegocio).ToList();
                List<PlantillaPersonalDTO> lstDetalleDTO = new List<PlantillaPersonalDTO>();
                PlantillaPersonalDTO objDetalleDTO = new PlantillaPersonalDTO();
                foreach (var item in lstTabuladores)
                {
                    tblRH_EK_Puestos objPuesto = lstPuestos.Where(w => w.puesto == item.FK_Puesto).FirstOrDefault();
                    tblRH_TAB_CatAreaDepartamento objAreaDepartamento = lstAreasDepartamentos.Where(w => w.id == objPuesto.FK_AreaDepartamento).FirstOrDefault();
                    tblRH_TAB_PlantillasPersonalDet objPersonalNecesario = lstPlantillasPersonalDet.Where(w => w.FK_Puesto == item.FK_Puesto).FirstOrDefault();
                    tblRH_EK_Tipos_Nomina objTipoNomina = lstTipoNominas.Where(w => w.tipo_nomina == objPuesto.FK_TipoNomina).FirstOrDefault();

                    objDetalleDTO = new PlantillaPersonalDTO();
                    objDetalleDTO.puestoDesc = SetPuestoEmpleado(objPuesto);
                    objDetalleDTO.departamentoDesc = objAreaDepartamento.concepto.Trim();
                    objDetalleDTO.nominaDesc = objTipoNomina.descripcion.Trim();
                    objDetalleDTO.personalNecesario = objPersonalNecesario.personalNecesario;

                    foreach (var objTabuladorDet in lstTabuladoresDet.Where(w => w.FK_Tabulador == item.id).ToList())
                    {
                        tblRH_TAB_CatCategorias objCategoria = lstCategorias.Where(w => w.id == objTabuladorDet.FK_Categoria).FirstOrDefault();
                        tblRH_TAB_CatEsquemaPago objEsquemaPago = lstEsquemasPagos.Where(w => w.id == objTabuladorDet.FK_EsquemaPago).FirstOrDefault();
                        objDetalleDTO.categoriaDesc += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button><br>", objCategoria.concepto);
                        objDetalleDTO.sueldoBaseDesc += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button><br>", objTabuladorDet.sueldoBase.ToString("C"));
                        objDetalleDTO.complementoDesc += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button><br>", objTabuladorDet.complemento.ToString("C"));
                        objDetalleDTO.totalNominalDesc += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button><br>", objTabuladorDet.totalNominal.ToString("C"));
                        objDetalleDTO.sueldoMensualDesc += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button><br>", objTabuladorDet.sueldoMensual.ToString("C"));
                        objDetalleDTO.esquemaPagoDesc += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button><br>", objEsquemaPago.concepto.Trim());
                    }

                    lstDetalleDTO.Add(objDetalleDTO);
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("lstDetalleDTO", lstDetalleDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetDetallePlantillaTabuladores", e, AccionEnum.CONSULTA, objParamDTO.id, objParamDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        #endregion

        #region MODIFICACIÓN
        public Dictionary<string, object> FillCboTipoModificaciones()
        {
            resultado.Clear();
            try
            {
                #region FILL COMBO TIPO MODIFICACIONES
                List<ComboDTO> lstComboDTO = new List<ComboDTO>();

                // INCREMENTO ANUAL A EMPLEADOS ACTIVOS
                int tipoModificacion = (int)TipoModificacionEnum.INCREMENTO_ANUAL_A_EMPLEADOS_ACTIVOS;
                ComboDTO objComboDTO = new ComboDTO();
                objComboDTO.Value = tipoModificacion.ToString();
                objComboDTO.Text = EnumHelper.GetDescription((TipoModificacionEnum)tipoModificacion);
                lstComboDTO.Add(objComboDTO);

                // MODIFICACION A PUESTOS
                tipoModificacion = (int)TipoModificacionEnum.MODIFICACION_A_PUESTOS;
                objComboDTO = new ComboDTO();
                objComboDTO.Value = tipoModificacion.ToString();
                objComboDTO.Text = EnumHelper.GetDescription((TipoModificacionEnum)tipoModificacion);
                lstComboDTO.Add(objComboDTO);

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboTipoModificaciones", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Clear();
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetTabuladoresModificacion(TabuladorDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region VALIDACIONES
                if (objParamDTO.FK_LineaNegocio <= 0) { throw new Exception("Es necesario seleccionar una línea de negocio."); }
                #endregion

                #region SE OBTIENE LISTADO DE TABULADORES AUTORIZADOS PARA SU MODIFICACION
                List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.tabuladorDetAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO &&
                    w.FK_LineaNegocio == objParamDTO.FK_LineaNegocio && w.registroActivo).ToList();

                List<int> lstTabuladoresDetID = lstTabuladoresDet.Select(s => s.FK_Tabulador).ToList();
                List<tblRH_TAB_Tabuladores> lstTabuladores = _context.tblRH_TAB_Tabuladores.Where(w => lstTabuladoresDetID.Contains(w.id) && w.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo).ToList();
                if (objParamDTO.lstFK_Puestos != null)
                    lstTabuladores = lstTabuladores.Where(w => objParamDTO.lstFK_Puestos.Contains(w.FK_Puesto)).ToList();

                if (lstTabuladores.Count() <= 0)
                    throw new Exception("No se encuentran tabuladores registrados en base a los filtros seleccionados.");

                #region CATALOGOS
                List<tblRH_EK_Puestos> lstPuestos = _context.tblRH_EK_Puestos.Where(w => w.FK_AreaDepartamento > 0 && w.FK_NivelMando > 0 && w.FK_Sindicato > 0 && w.FK_TipoNomina > 0 && w.registroActivo).ToList();
                List<tblRH_EK_Tipos_Nomina> lstTipoNominas = _context.tblRH_EK_Tipos_Nomina.Where(w => w.esActivo == true).ToList();
                List<tblRH_TAB_CatAreaDepartamento> lstAreasDepartamentos = _context.tblRH_TAB_CatAreaDepartamento.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatCategorias> lstCategorias = _context.tblRH_TAB_CatCategorias.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatEsquemaPago> lstEsquemasPagos = _context.tblRH_TAB_CatEsquemaPago.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_PlantillasPersonal> lstPlantillas = _context.tblRH_TAB_PlantillasPersonal.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_PlantillasPersonalDet> lstPlantillasDet = _context.tblRH_TAB_PlantillasPersonalDet.Where(w => w.registroActivo).ToList();
                #endregion

                List<TabuladorDetDTO> lstTabuladoresDTO = new List<TabuladorDetDTO>();
                TabuladorDetDTO objTabuladorDTO = new TabuladorDetDTO();
                int contadorIndex = 1;
                foreach (var item in lstTabuladores.Where(w => lstPuestos.Select(s => s.puesto).Contains(w.FK_Puesto)).ToList())
                {
                    #region SE VERIFICA SI EL PUESTO TIENE YA PLANTILLA PENDIENTE O AUTORIZADA
                    bool tienePlantillaAutorizada = false;
                    tblRH_TAB_PlantillasPersonalDet objPlantillaDet = lstPlantillasDet.Where(w => w.FK_Puesto == item.FK_Puesto && w.registroActivo).FirstOrDefault();
                    if (objPlantillaDet != null)
                    {
                        tblRH_TAB_PlantillasPersonal objPlantilla = lstPlantillas.Where(w => w.id == objPlantillaDet.FK_Plantilla && w.registroActivo).FirstOrDefault();
                        switch (objPlantilla.plantillaAutorizada)
                        {
                            case EstatusGestionAutorizacionEnum.AUTORIZADO:
                                tienePlantillaAutorizada = true;
                                break;
                        }
                    }
                    #endregion

                    if (tienePlantillaAutorizada)
                    {
                        // SE OBTIENE EL DETALLE DEL TABULADOR
                        List<tblRH_TAB_TabuladoresDet> lstTabDet = lstTabuladoresDet.Where(w => w.FK_Tabulador == item.id).ToList();
                        objTabuladorDTO = new TabuladorDetDTO();
                        foreach (var itemDet in lstTabuladoresDet.Where(w => w.FK_Tabulador == item.id).ToList())
                        {
                            #region INFORMACIÓN GENERAL DEL PUESTO
                            // SE OBTIENE EL PUESTO
                            tblRH_EK_Puestos objPuesto = lstPuestos.Where(w => w.puesto == item.FK_Puesto).FirstOrDefault();
                            if (objPuesto == null)
                                throw new Exception("Ocurrió un error al obtener la descripción del puesto.");

                            // SE OBTIENE EL TIPO DE NOMINA
                            tblRH_EK_Tipos_Nomina objTipoNomina = lstTipoNominas.Where(w => w.tipo_nomina == objPuesto.FK_TipoNomina).FirstOrDefault();
                            if (objTipoNomina == null)
                                throw new Exception("Ocurrió un error al obtener el tipo de nómina del puesto.");

                            // SE OBTIENE EL ÁREA / DEPARTAMENTO
                            tblRH_TAB_CatAreaDepartamento objAreaDepartamento = lstAreasDepartamentos.Where(w => w.id == objPuesto.FK_AreaDepartamento).FirstOrDefault();
                            if (objAreaDepartamento == null)
                                throw new Exception("Ocurrió un error al obtener el área / departamento del puesto.");

                            // SE OBTIENE LA CATEGORIA
                            tblRH_TAB_CatCategorias objCategoria = lstCategorias.Where(w => w.id == itemDet.FK_Categoria).FirstOrDefault();
                            if (objCategoria == null)
                                throw new Exception("Ocurrió un error al obtener la categoría del puesto.");

                            // SE OBTIENE EL ESQUEMA DE PAGO
                            tblRH_TAB_CatEsquemaPago objEsquemaPago = lstEsquemasPagos.Where(w => w.id == itemDet.FK_EsquemaPago).FirstOrDefault();
                            if (objEsquemaPago == null)
                                throw new Exception("Ocurrió un error al obtener el esquema de pago del puesto.");
                            #endregion

                            // SE ARMA PUESTO CON SUS TABULADORES
                            objTabuladorDTO.id = itemDet.FK_Tabulador;
                            objTabuladorDTO.idPuesto = item.FK_Puesto;
                            objTabuladorDTO.puestoDesc = !string.IsNullOrEmpty(objPuesto.descripcion) ? objPuesto.descripcion.Trim() : string.Empty;
                            objTabuladorDTO.soloDescPuesto = objPuesto.descripcion;
                            objTabuladorDTO.FK_Categoria = objCategoria.id;
                            objTabuladorDTO.categoriaDesc += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button><br>", !string.IsNullOrEmpty(objCategoria.concepto) ? objCategoria.concepto.Trim() : string.Empty);
                            objTabuladorDTO.tipoNominaDesc = !string.IsNullOrEmpty(objTipoNomina.descripcion) ? objTipoNomina.descripcion.Trim() : string.Empty;

                            objTabuladorDTO.sueldoBaseStringActual += string.Format("<div class='row'><div class='col-lg-12'><input type='text' onkeypress='return false' id='txtCE_Actual_SueldoBase_{0}' class='form-control sueldoBase' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, itemDet.sueldoBase.ToString("C"), itemDet.id, item.FK_Puesto, itemDet.FK_Tabulador);
                            objTabuladorDTO.complementoStringActual += string.Format("<div class='row'><div class='col-lg-12'><input type='text' onkeypress='return false' id='txtCE_Actual_Complemento_{0}' class='form-control complemento' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, itemDet.complemento.ToString("C"), itemDet.id, item.FK_Puesto, itemDet.FK_Tabulador);
                            objTabuladorDTO.totalNominalStringActual += string.Format("<div class='row'><div class='col-lg-12'><input type='text' onkeypress='return false' id='txtCE_Actual_TotalNominal_{0}' class='form-control totalNominal' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, itemDet.totalNominal.ToString("C"), itemDet.id, item.FK_Puesto, itemDet.FK_Tabulador);
                            objTabuladorDTO.sueldoMensualStringActual += string.Format("<div class='row'><div class='col-lg-12'><input type='text' onkeypress='return false' id='txtCE_Actual_SueldoMensual_{0}' class='form-control sueldoMensual' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, itemDet.sueldoMensual.ToString("C"), itemDet.id, item.FK_Puesto, itemDet.FK_Tabulador);

                            objTabuladorDTO.categoriaDescModificacion += SetFillComboCategorias(contadorIndex, objCategoria.id, itemDet.id, item.FK_Puesto, itemDet.FK_Tabulador, lstCategorias);
                            objTabuladorDTO.sueldoBaseStringModificacion += string.Format("<div class='row'><div class='col-lg-12'><input type='text' onkeypress='return false' id='txtCE_Modificacion_SueldoBase_{0}' class='form-control sueldoBase' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, itemDet.sueldoBase.ToString("C"), itemDet.id, item.FK_Puesto, itemDet.FK_Tabulador);
                            objTabuladorDTO.complementoStringModificacion += string.Format("<div class='row'><div class='col-lg-12'><input type='text' onkeypress='return false' id='txtCE_Modificacion_Complemento_{0}' class='form-control complemento' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, itemDet.complemento.ToString("C"), itemDet.id, item.FK_Puesto, itemDet.FK_Tabulador);
                            objTabuladorDTO.totalNominalStringModificacion += string.Format("<div class='row'><div class='col-lg-12'><input type='text' id='txtCE_Modificacion_TotalNominal_{0}' class='form-control totalNominal' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, itemDet.totalNominal.ToString("C"), itemDet.id, item.FK_Puesto, itemDet.FK_Tabulador);
                            objTabuladorDTO.sueldoMensualStringModificacion += string.Format("<div class='row'><div class='col-lg-12'><input type='text' onkeypress='return false' id='txtCE_Modificacion_SueldoMensual_{0}' class='form-control sueldoMensual' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, itemDet.sueldoMensual.ToString("C"), itemDet.id, item.FK_Puesto, itemDet.FK_Tabulador);

                            objTabuladorDTO.aumentoPorc += string.Format("<div class='row'><div class='col-lg-12'><input type='text' id='txtCE_Modificacion_Porc_{0}' class='form-control aumentoPorc' FK_TabuladorDet='{1}' FK_Puesto='{2}' FK_Tabulador='{3}' tipoEsquemaPago='{4}' FK_LineaNegocio='{5}' FK_AreaDepartamento='{6}'></div></div>", contadorIndex, itemDet.id, item.FK_Puesto, itemDet.FK_Tabulador, objEsquemaPago.concepto, itemDet.FK_LineaNegocio, objAreaDepartamento.id);
                            objTabuladorDTO.contadorIndex = contadorIndex;
                            contadorIndex++;
                        }
                        lstTabuladoresDTO.Add(objTabuladorDTO);
                    }
                }

                lstTabuladoresDTO = lstTabuladoresDTO.OrderBy(e => e.soloDescPuesto).ToList();

                resultado.Clear();
                resultado.Add("lstTabuladoresModificacionDTO", lstTabuladoresDTO);
                resultado.Add("contadorIndex", contadorIndex);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetTabuladoresModificacion", e, AccionEnum.CONSULTA, objParamDTO.id, objParamDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public string SetFillComboCategorias(int contadorIndex, int FK_Categoria, int FK_TabuladorDet, int FK_Puesto, int FK_Tabulador, List<tblRH_TAB_CatCategorias> lstCategorias)
        {
            string options = string.Empty;
            options += string.Format("<option value=''>--</option>");
            foreach (var item in lstCategorias)
            {
                tblRH_TAB_CatCategorias objCategoria = lstCategorias.Where(w => w.id == item.id).FirstOrDefault();
                options += string.Format("<option value='{0}' {2}>{1}</option>", objCategoria.id, objCategoria.concepto, objCategoria.id == FK_Categoria ? "selected" : "");
            }

            return string.Format("<div class='row'><div class='col-lg-12'><select id='cboCE_Actual_Categoria_{0}' style='margin-bottom: 3px !important;' class='form-control categoria' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'>{5}</select></div></div>", contadorIndex, FK_Categoria, FK_TabuladorDet, FK_Puesto, FK_Tabulador, options);
        }

        public string SetFillComboEsquemaPagos(int contadorIndex, int FK_EsquemaPago, int FK_TabuladorDet, int FK_Puesto, int FK_Tabulador, List<tblRH_TAB_CatEsquemaPago> lstEsquemasPagos)
        {
            string options = string.Empty;
            options += string.Format("<option value=''>--</option>");
            foreach (var item in lstEsquemasPagos)
            {
                tblRH_TAB_CatEsquemaPago objEsquemaPago = lstEsquemasPagos.Where(w => w.id == item.id).FirstOrDefault();
                options += string.Format("<option value='{0}' {2}>{1}</option>", objEsquemaPago.id, objEsquemaPago.concepto, objEsquemaPago.id == FK_EsquemaPago ? "selected" : "");
            }

            return string.Format("<div class='row'><div class='col-lg-12'><select id='cboCE_Actual_EsquemPago_{0}' style='margin-bottom: 3px;' class='form-control esquemaPago' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'>{5}</select></div></div>", contadorIndex, FK_EsquemaPago, FK_TabuladorDet, FK_Puesto, FK_Tabulador, options);
        }

        public Dictionary<string, object> FillCboFiltroPuestos_Modificacion(TabuladorDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region FILL COMBO PUESTOS CON TABULADORES AUTORIZADOS Y SE FILTRA EN BASE AL AREA/DEPARTAMENTO SELECCIONADO
                List<tblRH_EK_Empleados> lstEmpleados = _context.tblRH_EK_Empleados.Where(w => w.estatus_empleado == "A" && w.esActivo).ToList();
                if (objParamDTO.lstCC != null)
                {
                    if (objParamDTO.lstCC.Count() > 0)
                        lstEmpleados = lstEmpleados.Where(w => objParamDTO.lstCC.Contains(w.cc_contable)).ToList();
                }

                List<int> lstPuestos_ID = new List<int>();
                foreach (var item in lstEmpleados.Select(s => s.puesto).ToList())
                {
                    if (item > 0)
                    {
                        int puesto = Convert.ToInt32(item);
                        lstPuestos_ID.Add(puesto);
                    }
                }

                List<tblRH_EK_Puestos> lstPuestos = _context.tblRH_EK_Puestos.Where(w => lstPuestos_ID.Contains(w.puesto) && w.registroActivo).OrderBy(o => o.descripcion).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstPuestos)
                {
                    tblRH_EK_Puestos objPuesto = lstPuestos.Where(w => w.puesto == item.puesto).FirstOrDefault();
                    if (objPuesto != null)
                    {
                        if (objPuesto.puesto > 0 && !string.IsNullOrEmpty(objPuesto.descripcion))
                        {
                            objComboDTO = new ComboDTO();
                            objComboDTO.Value = objPuesto.puesto.ToString();
                            objComboDTO.Text = string.Format("[{0}] {1}", objPuesto.puesto, objPuesto.descripcion.Trim());
                            lstComboDTO.Add(objComboDTO);
                        }
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboFiltroPuestos_Modificacion", e, AccionEnum.FILLCOMBO, objParamDTO.id, objParamDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CrearModificacion(GestionModificacionTabuladorDTO objParamDTO, List<GestionModificacionTabuladorDetDTO> lstParamDTO, List<GestionAutorizanteDTO> lstGestionAutorizantesDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado.Clear();
                try
                {
                    #region VALIDACIONES
                    if (objParamDTO.tipoModificacion <= 0) { throw new Exception("Es necesario seleccionar el tipo de modificación."); }
                    if (objParamDTO.lstFK_LineaNegocio == null) { throw new Exception("Es necesario seleccionar al menos una línea de negocio."); }
                    if (lstGestionAutorizantesDTO == null) { throw new Exception("Es necesario indicar al menos un autorizante."); }

                    bool tieneAutorizante = false;
                    foreach (var item in lstGestionAutorizantesDTO)
                    {
                        if (item.FK_UsuarioAutorizacion > 0)
                            tieneAutorizante = true;
                    }
                    if (!tieneAutorizante) { throw new Exception("Es necesario indicar al menos un autorizante."); }
                    #endregion

                    #region SE CREA EL REGISTRO PRINCIPAL EN BASE A LOS FILTROS
                    // SE CREA REGISTRO PRINCIPAL DE LA MODIFICACIÓN
                    tblRH_TAB_GestionModificacionTabulador objGestionModificacion = new tblRH_TAB_GestionModificacionTabulador();
                    objGestionModificacion.tipoModificacion = objParamDTO.tipoModificacion;
                    objGestionModificacion.estatus = objParamDTO.estatus;
                    objGestionModificacion.modificacionAutorizada = EstatusGestionAutorizacionEnum.PENDIENTE;
                    objGestionModificacion.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objGestionModificacion.fechaCreacion = DateTime.Now;
                    objGestionModificacion.registroActivo = true;
                    _context.tblRH_TAB_GestionModificacionTabulador.Add(objGestionModificacion);
                    _context.SaveChanges();

                    // SE OBTIENE LA LLAVE PRINICIPAL QUE SE ACABA DE CREAR
                    int FK_IncrementoAnual = _context.tblRH_TAB_GestionModificacionTabulador.Where(w => w.registroActivo).Select(s => s.id).OrderByDescending(o => o).FirstOrDefault();
                    if (FK_IncrementoAnual <= 0)
                        throw new Exception("Ocurrió un error al obtener la información del registro principal.");
                    #endregion

                    string mensajeErrorRegistro = string.Empty;
                    GestionModificacionTabuladorDetDTO objModificacionDetDTO = new GestionModificacionTabuladorDetDTO();
                    tblRH_TAB_CatTipoModificacion objTipoModificacion = _context.tblRH_TAB_CatTipoModificacion.Where(w => w.id == (int)objParamDTO.tipoModificacion && w.registroActivo).FirstOrDefault();

                    if (objParamDTO.tipoModificacion == TipoModificacionEnum.INCREMENTO_ANUAL_A_EMPLEADOS_ACTIVOS)
                    {
                        #region INCREMENTO ANUAL A EMPLEADOS ACTIVOS
                        mensajeErrorRegistro = IncrementoEmpleadosActivos(FK_IncrementoAnual, objParamDTO, lstParamDTO);
                        if (!string.IsNullOrEmpty(mensajeErrorRegistro))
                            throw new Exception(mensajeErrorRegistro);
                        #endregion
                    }
                    else if (objParamDTO.tipoModificacion == TipoModificacionEnum.MODIFICACION_A_PUESTOS)
                    {
                        #region INCREMENTO AL PUESTO (TABULADOR)
                        mensajeErrorRegistro = ModificacionPuestos(FK_IncrementoAnual, objParamDTO, lstParamDTO);
                        if (!string.IsNullOrEmpty(mensajeErrorRegistro))
                            throw new Exception(mensajeErrorRegistro);
                        #endregion
                    }

                    #region SE REGISTRA LA GESTIÓN PARA SU AUTORIZACIÓN
                    List<tblRH_TAB_GestionAutorizantes> lstGestion = new List<tblRH_TAB_GestionAutorizantes>();
                    tblRH_TAB_GestionAutorizantes objGestion = new tblRH_TAB_GestionAutorizantes();
                    foreach (var item in lstGestionAutorizantesDTO)
                    {
                        objGestion = new tblRH_TAB_GestionAutorizantes();
                        objGestion.FK_Registro = FK_IncrementoAnual;
                        objGestion.vistaAutorizacion = VistaAutorizacionEnum.MODIFICACION_TABULADORES;
                        objGestion.nivelAutorizante = item.nivelAutorizante;
                        objGestion.FK_UsuarioAutorizacion = item.FK_UsuarioAutorizacion;
                        objGestion.autorizado = (int)EstatusGestionAutorizacionEnum.PENDIENTE;
                        objGestion.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objGestion.fechaCreacion = DateTime.Now;
                        objGestion.registroActivo = true;
                        lstGestion.Add(objGestion);
                    }
                    _context.tblRH_TAB_GestionAutorizantes.AddRange(lstGestion);
                    _context.SaveChanges();
                    #endregion

                    #region SE CREA LAS ALERTAS A LOS AUTORIZANTES
                    tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                    //foreach (var item in lstGestionAutorizantesDTO)

                    var primerAuth = lstGestionAutorizantesDTO.FirstOrDefault();

                    objNuevaAlerta = new tblP_Alerta();
                    objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                    objNuevaAlerta.userRecibeID = primerAuth.FK_UsuarioAutorizacion;
#if DEBUG
                    objNuevaAlerta.userRecibeID = _USUARIO_OMAR_NUNEZ_ID; //USUARIO ID: Omar Nuñez.
#endif
                    objNuevaAlerta.tipoAlerta = 2;
                    objNuevaAlerta.sistemaID = 16;
                    objNuevaAlerta.visto = false;
                    objNuevaAlerta.url = "/Administrativo/Tabuladores/GestionModificacion";
                    objNuevaAlerta.objID = FK_IncrementoAnual;
                    objNuevaAlerta.obj = "AutorizarModificacion";
                    objNuevaAlerta.msj = "Modificación Pendiente de Autorizar";
                    objNuevaAlerta.documentoID = 0;
                    objNuevaAlerta.moduloID = 0;
                    _context.tblP_Alerta.Add(objNuevaAlerta);
                    _context.SaveChanges();

                    #endregion

                    #region SE NOTIFICA A LOS AUTORIZANTES QUE SE HA REGISTRADO UNA MODIFICACIÓN DE TABULADOR LISTADO PARA SER AUTORIZADO
                    List<int> lstFK_UsuarioAutorizacion = lstGestionAutorizantesDTO.Select(s => s.FK_UsuarioAutorizacion).ToList();
                    List<string> lstCorreosNotificar = new List<string>();
                    for (int i = 0; i < lstFK_UsuarioAutorizacion.Count(); i++)
                    {
                        int FK_UsuarioAutorizacion = lstFK_UsuarioAutorizacion[i];
                        tblP_Usuario objUsuario = _context.tblP_Usuario.Where(w => w.id == FK_UsuarioAutorizacion).FirstOrDefault();
                        if (objUsuario == null)
                            throw new Exception("Ocurrió un error al notificar a los autorizantes.");

                        string correo = objUsuario.correo;
                        if (string.IsNullOrEmpty(correo))
                        {
                            string mensajeError = string.Format("El autorizante [{0} {1}], no cuenta con correo registrado. Favor de notificar a TI.", objUsuario.nombre.Trim(), objUsuario.apellidoPaterno.Trim());
                            throw new Exception(mensajeError);
                        }

                        lstCorreosNotificar.Add(correo.Trim());
                    }

                    #region CORREO

                    #region REPORTE

                    string strModLineasNegocio = string.Empty;
                    string lstLineasNegocio = string.Empty;
                    ReportDocument rptCV = new ReportDocument();

                    if (objParamDTO.tipoModificacion == TipoModificacionEnum.MODIFICACION_A_PUESTOS)
                    {
                        rptCV = new rptRepTabuladoresModificacion();
                        //string path = Path.Combine(RutaServidor, "rptRepTabuladoresModificacion.rpt");
                        //rptCV.Load(path);

                        //var objParamsDTO = Session["objParamsPDF"] as TabuladorDTO;
                        TabuladorDTO objParamsDTO = new TabuladorDTO();
                        objParamsDTO.FK_IncrementoAnual = FK_IncrementoAnual;
                        objParamsDTO.lstFK_LineaNegocio = objParamDTO.lstFK_LineaNegocio;

                        var dictRepTabuladores = GetTabuladoresModificacionReportePDF((objParamsDTO));
                        List<RepTabuladoresModificacionDTO> lstTabuladores = dictRepTabuladores["lstTabPuestos"] as List<RepTabuladoresModificacionDTO>;

                        List<string> lstDescLN = dictRepTabuladores["lstLineasDeNegocios"] as List<string>;
                        strModLineasNegocio = lstDescLN != null && lstDescLN.Count() > 0 ? (string.Join(", ", lstDescLN)) : "";


                        //LIMPIAR DICCIONARIO
                        resultado.Clear();

                        objParamsDTO.lstDescLineaNegocio = new List<string>();
                        objParamsDTO.lstDescLineaNegocio.Add("LN");

                        objParamsDTO.lstDescCC = new List<string>();
                        objParamsDTO.lstDescCC.Add("DESC");

                        objParamsDTO.lstReporteAutorizantesDTO = new List<RepAutorizantesTABDTO>();
                        RepAutorizantesTABDTO obj = new RepAutorizantesTABDTO();
                        objParamsDTO.lstReporteAutorizantesDTO.Add(obj);

                        lstLineasNegocio = string.Join(", ", objParamsDTO.FK_LineaNegocio);
                        string lstCC = string.Join(", ", objParamsDTO.lstDescCC);

                        rptCV.Database.Tables[0].SetDataSource(getInfoEnca("Reporte de Modificacion de Tabuladores", "Dirección de Capital Humano"));
                        rptCV.Database.Tables[1].SetDataSource(lstTabuladores);
                        rptCV.Database.Tables[2].SetDataSource(objParamsDTO.lstReporteAutorizantesDTO);

                        rptCV.SetParameterValue("lstLineasNegocio", lstLineasNegocio);
                        rptCV.SetParameterValue("lstCC", lstCC);
                        rptCV.SetParameterValue("año", objParamsDTO.añoReporte);
                        rptCV.SetParameterValue("fecha", DateTime.Now.ToString("dd/MM/yyyy h:mm tt"));
                    }
                    else
                    {
                        rptCV = new rptRepTabuladoresModificacionEmpleados();
                        //string path = Path.Combine(RutaServidor, "rptRepTabuladoresModificacionEmpleados.rpt");
                        //rptCV.Load(path);

                        //var objParamsDTO = Session["objParamsPDF"] as TabuladorDTO;
                        TabuladorDTO objParamsDTO = new TabuladorDTO();
                        objParamsDTO.FK_IncrementoAnual = FK_IncrementoAnual;
                        objParamsDTO.lstFK_LineaNegocio = objParamDTO.lstFK_LineaNegocio;

                        var dictRepTabuladores = GetTabuladoresModificacionReportePDF((objParamsDTO));
                        List<RepTabuladoresModificacionDTO> lstTabuladores = dictRepTabuladores["lstTabPuestos"] as List<RepTabuladoresModificacionDTO>;

                        List<string> lstDescLN = dictRepTabuladores["lstLineasDeNegocios"] as List<string>;
                        strModLineasNegocio = lstDescLN != null && lstDescLN.Count() > 0 ? (string.Join(", ", lstDescLN)) : "";


                        //LIMPIAR DICCIONARIO
                        resultado.Clear();

                        objParamsDTO.lstDescLineaNegocio = new List<string>();
                        objParamsDTO.lstDescLineaNegocio.Add("LN");

                        objParamsDTO.lstDescCC = new List<string>();
                        objParamsDTO.lstDescCC.Add("DESC");

                        objParamsDTO.lstReporteAutorizantesDTO = new List<RepAutorizantesTABDTO>();
                        RepAutorizantesTABDTO obj = new RepAutorizantesTABDTO();
                        objParamsDTO.lstReporteAutorizantesDTO.Add(obj);

                        lstLineasNegocio = string.Join(", ", objParamsDTO.FK_LineaNegocio);
                        string lstCC = string.Join(", ", objParamsDTO.lstDescCC);

                        rptCV.Database.Tables[0].SetDataSource(getInfoEnca("Reporte de Modificacion de Tabuladores", "Dirección de Capital Humano"));
                        rptCV.Database.Tables[1].SetDataSource(lstTabuladores);
                        rptCV.Database.Tables[2].SetDataSource(objParamsDTO.lstReporteAutorizantesDTO);

                        rptCV.SetParameterValue("lstLineasNegocio", lstLineasNegocio);
                        rptCV.SetParameterValue("lstCC", lstCC);
                        rptCV.SetParameterValue("año", objParamsDTO.añoReporte);
                        rptCV.SetParameterValue("fecha", DateTime.Now.ToString("dd/MM/yyyy h:mm tt"));
                    }
                    Stream stream = rptCV.ExportToStream(ExportFormatType.PortableDocFormat);
                    #endregion

                    string cuerpo =
                                @"<html>
                                    <head>
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
                                                background-color: #ffcc5c;
                                            }
                                        </style>
                                    </head>
                                    <body lang=ES-MX link='#0563C1' vlink='#954F72'>
                                            <p class=MsoNormal>
                                                Buen día,<br><br>" + "Se ha autorizado una Modificacion de Tabuladores"
                                                + @".<br><br>
                                            </p>
                                            <p class=MsoNormal style='font-weight:bold;'>" +
                                                "Modificación: " + objTipoModificacion.concepto + @".<o:p></o:p>
                                            </p>
                                            <br><br><br>";

                    #region TABLA AUTORIZANTES
                    cuerpo += @"<table>
                                    <thead>
                                        <tr>
                                            <th>Nombre</th>
                                            <th>Tipo</th>
                                            <th>Autorizo</th>
                                        </tr>
                                    </thead>
                                    <tbody>";

                    bool esPrimero = true;
                    //EstatusGestionAutorizacionEnum P 0 A 1 R 2 
                    if (lstGestionAutorizantesDTO != null)
                    {
                        foreach (var itemDet in lstGestionAutorizantesDTO)
                        {
                            tblP_Usuario objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == itemDet.FK_UsuarioAutorizacion);
                            string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                            cuerpo += "<tr>" +
                                        "<td>" + nombreCompleto + "</td>" +
                                        "<td>" + EnumHelper.GetDescription(itemDet.nivelAutorizante) + "</td>" +
                                        GetEstatusTabulador(0, esPrimero) +
                                    "</tr>";

                            if (esPrimero)
                                esPrimero = false;
                        }
                    }

                    cuerpo += "</tbody>" +
                                "</table>" +
                                "<br><br><br>";
                    #endregion

                    cuerpo += "<br><br><br>" +
                          "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                          "Capital Humano > Administración de personal > Configuración > Estructura Salarial > Gestión de Reportes.<br><br>" +
                          "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                          "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias." +
                        " </body>" +
                      "</html>";
                    #endregion

                    lstCorreosNotificar.Add(_CORREO_DIANA_ALVAREZ);
                    lstCorreosNotificar.Add(_CORREO_MANUEL_CRUZ);
#if DEBUG
                    lstCorreosNotificar = new List<string>();
                    lstCorreosNotificar.Add(_CORREO_OMAR_NUNEZ);
#endif
                    List<byte[]> downloadPDFs = new List<byte[]>();
                    using (var streamReader = new MemoryStream())
                    {
                        stream.CopyTo(streamReader);
                        downloadPDFs.Add(streamReader.ToArray());

                        GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: MODIFICACIÓN TABULADOR FECHA: {1}", PersonalUtilities.GetNombreEmpresa(), DateTime.Now.ToString("dd/MM/yyyy")), cuerpo,
                                                                lstCorreosNotificar, downloadPDFs, "Modificacion.pdf");
                    }
                    #endregion

                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha registrado con éxito.");
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "CrearModificacion", e, AccionEnum.AGREGAR, 0, lstParamDTO);
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public string IncrementoEmpleadosActivos(int FK_IncrementoAnual, GestionModificacionTabuladorDTO objParamDTO, List<GestionModificacionTabuladorDetDTO> lstParamDTO)
        {
            string mensajeErrorRegistro = string.Empty;
            try
            {
                #region SE LE INCREMENTA EL SUELDO DE SU ULTIMO HISTORIAL AL EMPLEADO
                foreach (var FK_Puesto in objParamDTO.lstFK_Puestos)
                {
                    // SE OBTIENE LISTADO DE EMPLEADOS CON EL PUESTO DEL FOREACH
                    List<int> lstClaveEmpleados = lstParamDTO.Select(s => s.clave_empleado).ToList();
                    List<tblRH_EK_Empleados> lstEmpleados = _context.tblRH_EK_Empleados.Where(w => w.puesto == FK_Puesto && lstClaveEmpleados.Contains(w.clave_empleado) && w.estatus_empleado == "A" && w.esActivo).ToList();
                    if (lstEmpleados.Count() <= 0)
                    {
                        mensajeErrorRegistro = string.Format("No se encuentra empleados con el puesto: {0}.", FK_Puesto);
                        throw new Exception(mensajeErrorRegistro);
                    }

                    foreach (var objEmpleado in lstEmpleados)
                    {
                        #region SE REGISTRA EL INCREMENTO ANUAL POR EMPLEADO PARA SU GESTIÓN
                        tblRH_EK_Tabulador_Historial objTabuladorHistorial = _context.tblRH_EK_Tabulador_Historial.Where(w => w.clave_empleado == objEmpleado.clave_empleado).OrderByDescending(o => o.fechaAplicaCambio).OrderByDescending(o => o.id).FirstOrDefault();
                        if (objTabuladorHistorial == null)
                        {
                            mensajeErrorRegistro = string.Format("No se encuentra el tabulador historial del empleado: {0}.", objEmpleado.clave_empleado);
                            throw new Exception(mensajeErrorRegistro);
                        }
                        else if (objEmpleado.clave_empleado > 0)
                        {
                            // SE OBTIENE EL PORCENTAJE A AUMENTAR EN BASE AL DETALLE TABULADOR QUE TENGA ASIGNADO EL EMPLEADO
                            int porcTabulador = lstParamDTO.Where(w => w.clave_empleado == objEmpleado.clave_empleado).Select(s => s.porcentajeIncremento).FirstOrDefault();
                            decimal salario_base = lstParamDTO.Where(w => w.clave_empleado == objEmpleado.clave_empleado).Select(s => s.salario_base).FirstOrDefault();
                            decimal complemento = lstParamDTO.Where(w => w.clave_empleado == objEmpleado.clave_empleado).Select(s => s.complemento).FirstOrDefault();
                            int FK_Categoria = lstParamDTO.Where(w => w.clave_empleado == objEmpleado.clave_empleado).Select(s => s.FK_Categoria).FirstOrDefault();
                            int FK_LineaNegocio = lstParamDTO.Where(w => w.clave_empleado == objEmpleado.clave_empleado).Select(s => s.FK_LineaNegocio).FirstOrDefault();

                            tblRH_TAB_GestionModificacionTabuladorDet objCEModificacionDetDTO = new tblRH_TAB_GestionModificacionTabuladorDet();
                            objCEModificacionDetDTO.FK_IncrementoAnual = FK_IncrementoAnual;
                            objCEModificacionDetDTO.FK_Puesto = FK_Puesto;
                            objCEModificacionDetDTO.porcentajeIncremento = porcTabulador;
                            objCEModificacionDetDTO.clave_empleado = objEmpleado.clave_empleado;
                            objCEModificacionDetDTO.tabulador = objTabuladorHistorial.tabulador > 0 ? Convert.ToInt32(objTabuladorHistorial.tabulador) : 0;
                            objCEModificacionDetDTO.tabulador_anterior = objTabuladorHistorial.tabulador_anterior > 0 ? Convert.ToInt32(objTabuladorHistorial.tabulador_anterior) : 0;

                            // SE OBTIENE EL FK_Tabulador DEL PUESTO y el FK_TabuladorDet EN BASE A LA CATEGORIA
                            tblRH_TAB_Tabuladores objTabulador = _context.tblRH_TAB_Tabuladores.Where(w => w.FK_Puesto == FK_Puesto && w.registroActivo).FirstOrDefault();
                            if (objTabulador == null)
                                throw new Exception("Ocurrió un error al obtener la información del tabulador.");

                            tblRH_TAB_TabuladoresDet objTabuladorDet = new tblRH_TAB_TabuladoresDet();
                            if (FK_Categoria > 0)
                                objTabuladorDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.FK_Tabulador == objTabulador.id && w.FK_Categoria == FK_Categoria).FirstOrDefault();

                            // SE OBTIENE EL TIPO DE NOMINA DEL PUESTO
                            int FK_TipoNomina = _context.tblRH_EK_Puestos.Where(w => w.puesto == FK_Puesto && w.registroActivo).Select(s => s.FK_TipoNomina).FirstOrDefault();
                            if (FK_TipoNomina <= 0)
                                throw new Exception("Ocurrió un error al obtener el tipo de nómina.");

                            objCEModificacionDetDTO.FK_Tabulador = objTabulador.id > 0 ? objTabulador.id : 0;
                            objCEModificacionDetDTO.FK_TabuladorDet = objTabuladorDet != null ? objTabuladorDet.id : 0;

                            objCEModificacionDetDTO.FK_Categoria = FK_Categoria;
                            objCEModificacionDetDTO.FK_LineaNegocio = FK_LineaNegocio;
                            objCEModificacionDetDTO.fechaAplicaCambio = objParamDTO.fechaAplicaCambio;
                            objCEModificacionDetDTO.hora = DateTime.Now.TimeOfDay;

                            objCEModificacionDetDTO.salario_base = (decimal)salario_base;
                            objCEModificacionDetDTO.complemento = (decimal)complemento;
                            objCEModificacionDetDTO.suma = (decimal)salario_base + (decimal)complemento;

                            if (FK_TipoNomina == (int)TipoNominaEnum.quincenal)
                                objCEModificacionDetDTO.totalMensual = ((decimal)objCEModificacionDetDTO.suma * (decimal)2);
                            else if (FK_TipoNomina == (int)TipoNominaEnum.semanal)
                                objCEModificacionDetDTO.totalMensual = (((decimal)objCEModificacionDetDTO.suma / 7) * (decimal)30.4);

                            objCEModificacionDetDTO.registroAplicado = EstatusGestionAutorizacionEnum.PENDIENTE;
                            objCEModificacionDetDTO.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objCEModificacionDetDTO.fechaCreacion = DateTime.Now;
                            objCEModificacionDetDTO.registroActivo = true;
                            _context.tblRH_TAB_GestionModificacionTabuladorDet.Add(objCEModificacionDetDTO);
                            _context.SaveChanges();

                            // SE REGISTRA BITACORA
                            SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objCEModificacionDetDTO));
                        }
                        else
                            throw new Exception("No se encuentra el detalle del tabulador de los empleados en base al puesto.");
                        #endregion
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "IncrementoAnualEmpleados", e, AccionEnum.AGREGAR, 0, 0);
                mensajeErrorRegistro = e.Message;
                return mensajeErrorRegistro;
            }
            return mensajeErrorRegistro;
        }

        public string ModificacionPuestos(int FK_IncrementoAnual, GestionModificacionTabuladorDTO objParamDTO, List<GestionModificacionTabuladorDetDTO> lstParamDTO)
        {
            string mensajeErrorRegistro = string.Empty;
            try
            {
                #region SE REGISTRA LA MODIFICACIÓN DE LOS PUESTOS EN TABLA DE GESTIÓN PARA SU AUTORIZACIÓN
                List<int> lstTabuladoresDetFK = lstParamDTO.Select(s => s.FK_TabuladorDet).ToList();
                List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(w => lstTabuladoresDetFK.Contains(w.id) && w.registroActivo).ToList();

                List<tblRH_TAB_GestionModificacionTabuladorDet> lstCEModificacionDet = new List<tblRH_TAB_GestionModificacionTabuladorDet>();
                tblRH_TAB_GestionModificacionTabuladorDet objCEModificacionDet = new tblRH_TAB_GestionModificacionTabuladorDet();
                foreach (var item in lstParamDTO)
                {
                    // SE OBTIENE EL PORCENTAJE A AUMENTAR EN BASE AL DETALLE TABULADOR QUE TENGA ASIGNADO EL EMPLEADO
                    objCEModificacionDet = new tblRH_TAB_GestionModificacionTabuladorDet();
                    objCEModificacionDet.FK_IncrementoAnual = FK_IncrementoAnual;
                    objCEModificacionDet.FK_Puesto = item.FK_Puesto;
                    objCEModificacionDet.porcentajeIncremento = item.porcentajeIncremento;
                    objCEModificacionDet.clave_empleado = item.clave_empleado;
                    objCEModificacionDet.tabulador = item.tabulador > 0 ? Convert.ToInt32(item.tabulador) : 0;
                    objCEModificacionDet.tabulador_anterior = item.tabulador_anterior > 0 ? Convert.ToInt32(item.tabulador_anterior) : 0;
                    objCEModificacionDet.FK_Tabulador = item.FK_Tabulador;
                    objCEModificacionDet.FK_TabuladorDet = item.FK_TabuladorDet;
                    objCEModificacionDet.FK_Categoria = item.FK_Categoria;
                    objCEModificacionDet.tabuladorDetAutorizado = lstTabuladoresDet.Where(w => w.id == item.FK_TabuladorDet).Select(s => s.tabuladorDetAutorizado).FirstOrDefault();
                    objCEModificacionDet.FK_LineaNegocio = item.FK_LineaNegocio;
                    objCEModificacionDet.FK_EsquemaPago = item.FK_EsquemaPago;
                    objCEModificacionDet.fecha_cambio = null;
                    objCEModificacionDet.fechaAplicaCambio = objParamDTO.fechaAplicaCambio;
                    objCEModificacionDet.hora = DateTime.Now.TimeOfDay;

                    objCEModificacionDet.salario_base = (decimal)item.salario_base;
                    objCEModificacionDet.complemento = (decimal)item.complemento;
                    objCEModificacionDet.suma = (decimal)objCEModificacionDet.salario_base + (decimal)objCEModificacionDet.complemento;
                    objCEModificacionDet.totalMensual = (decimal)item.totalMensual;
                    objCEModificacionDet.bono_zona = 0;

                    objCEModificacionDet.motivoCambio = 0;
                    objCEModificacionDet.registroAplicado = EstatusGestionAutorizacionEnum.PENDIENTE;
                    objCEModificacionDet.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objCEModificacionDet.fechaCreacion = DateTime.Now;
                    objCEModificacionDet.registroActivo = true;
                    lstCEModificacionDet.Add(objCEModificacionDet);
                }
                _context.tblRH_TAB_GestionModificacionTabuladorDet.AddRange(lstCEModificacionDet);
                _context.SaveChanges();

                // SE REGISTRA BITACORA
                SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(lstCEModificacionDet));
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "ModificacionPuestos", e, AccionEnum.AGREGAR, 0, 0);
                mensajeErrorRegistro = e.Message;
                return mensajeErrorRegistro;
            }
            return mensajeErrorRegistro;
        }

        public string ModificacionSoloTabuladores(int FK_IncrementoAnual, GestionModificacionTabuladorDTO objParamDTO, List<GestionModificacionTabuladorDetDTO> lstParamDTO)
        {
            string mensajeErrorRegistro = string.Empty;
            try
            {
                #region MODIFICACIÓN SOLO A TABULADORES
                foreach (var item in lstParamDTO)
                {
                    // SE REGISTRA EL DETALLE DEL NUEVO TABULADOR
                    tblRH_TAB_GestionModificacionTabuladorDet objCETabulador = new tblRH_TAB_GestionModificacionTabuladorDet();
                    objCETabulador.FK_IncrementoAnual = FK_IncrementoAnual;
                    objCETabulador.FK_Puesto = item.FK_Puesto;
                    objCETabulador.porcentajeIncremento = item.porcentajeIncremento;
                    objCETabulador.tabulador = item.tabulador;
                    objCETabulador.tabulador_anterior = item.tabulador_anterior;
                    objCETabulador.FK_Tabulador = item.FK_Tabulador;
                    objCETabulador.FK_TabuladorDet = item.FK_TabuladorDet;
                    objCETabulador.FK_Categoria = item.FK_Categoria;
                    objCETabulador.fechaAplicaCambio = objParamDTO.fechaAplicaCambio;
                    objCETabulador.hora = DateTime.Now.TimeOfDay;

                    objCETabulador.salario_base = (decimal)item.salario_base;
                    objCETabulador.complemento = (decimal)item.complemento;
                    objCETabulador.suma = (decimal)objCETabulador.salario_base + (decimal)objCETabulador.complemento;

                    objCETabulador.registroAplicado = EstatusGestionAutorizacionEnum.PENDIENTE;
                    objCETabulador.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objCETabulador.fechaCreacion = DateTime.Now;
                    objCETabulador.registroActivo = true;
                    _context.tblRH_TAB_GestionModificacionTabuladorDet.Add(objCETabulador);
                    _context.SaveChanges();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objCETabulador));
                }
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "ModificacionSoloTabuladores", e, AccionEnum.AGREGAR, 0, 0);
                return mensajeErrorRegistro;
            }
            return mensajeErrorRegistro;
        }

        public Dictionary<string, object> GetTabuladoresEmpleadosActivos(TabuladorDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region CATALOGOS
                List<tblRH_EK_Tipos_Nomina> lstTipoNominas = _context.tblRH_EK_Tipos_Nomina.Where(w => w.esActivo == true).ToList();
                List<tblRH_TAB_CatAreaDepartamento> lstAreasDepartamentos = _context.tblRH_TAB_CatAreaDepartamento.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatCategorias> lstCategorias = _context.tblRH_TAB_CatCategorias.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatEsquemaPago> lstEsquemaPagos = _context.tblRH_TAB_CatEsquemaPago.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_Tabuladores> lstTabuladores = _context.tblRH_TAB_Tabuladores.Where(w => w.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo).ToList();
                List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.tabuladorDetAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo).ToList();
                List<tblRH_TAB_CatLineaNegocioDet> lstLineaNegociosDet = _context.tblRH_TAB_CatLineaNegocioDet.Where(w => w.registroActivo).ToList();
                List<tblRH_EK_Puestos> lstPuestos = _context.tblRH_EK_Puestos.Where(w => w.registroActivo).ToList();
                #endregion

                #region SE OBTIENE LISTADO DE EMPLEADOS ACTIVOS
                List<tblRH_EK_Empleados> lstEmpleadosActivos = _context.tblRH_EK_Empleados.Where(w => w.estatus_empleado == "A" && w.esActivo).ToList();
                List<int> lstEmpleadosActivos_ClaveEmpleado = lstEmpleadosActivos.Select(s => s.clave_empleado).ToList();
                List<tblRH_EK_Tabulador_Historial> lstEmpleadoTabHistorial = _context.tblRH_EK_Tabulador_Historial.Where(w => lstEmpleadosActivos_ClaveEmpleado.Contains(w.clave_empleado) && w.esActivo).ToList();

                #region FILTROS
                if (objParamDTO.lstFK_LineaNegocio != null)
                {
                    if (objParamDTO.lstFK_LineaNegocio.Count() > 0)
                    {
                        List<string> lstCC = lstLineaNegociosDet.Where(w => objParamDTO.lstFK_LineaNegocio.Contains(w.FK_LineaNegocio)).Select(s => s.cc).ToList();
                        lstEmpleadosActivos = lstEmpleadosActivos.Where(w => lstCC.Contains(w.cc_contable)).ToList();
                    }
                }

                if (objParamDTO.lstFK_AreaDepartamento != null)
                {
                    if (objParamDTO.lstFK_AreaDepartamento.Count() > 0)
                    {
                        lstPuestos = lstPuestos.Where(w => objParamDTO.lstFK_AreaDepartamento.Contains(w.FK_AreaDepartamento)).ToList();
                        List<int> lstPuestosID = lstPuestos.Select(s => s.puesto).ToList();
                        lstEmpleadosActivos = lstEmpleadosActivos.Where(w => lstPuestosID.Contains(Convert.ToInt32(w.puesto))).ToList();
                    }
                }

                if (objParamDTO.lstCC != null)
                {
                    if (objParamDTO.lstCC.Count() > 0)
                        lstEmpleadosActivos = lstEmpleadosActivos.Where(w => objParamDTO.lstCC.Contains(w.cc_contable)).ToList();
                }

                if (objParamDTO.lstFK_Puestos != null)
                {
                    if (objParamDTO.lstFK_Puestos.Count() > 0)
                        lstEmpleadosActivos = lstEmpleadosActivos.Where(w => objParamDTO.lstFK_Puestos.Contains(Convert.ToInt32(w.puesto))).ToList();
                }
                #endregion

                List<TabuladorDetDTO> lstTabuladoresDTO = new List<TabuladorDetDTO>();
                TabuladorDetDTO objTabuladorDTO = new TabuladorDetDTO();
                int contadorIndex = 1;
                foreach (var item in lstEmpleadosActivos.OrderBy(o => o.puesto).ToList())
                {
                    // SE OBTIENE EL ULTIMO TABULADOR DEL EMPLEADO ACTIVO
                    tblRH_EK_Empleados objEmpleado = lstEmpleadosActivos.Where(w => w.clave_empleado == item.clave_empleado).FirstOrDefault();
                    tblRH_EK_Tabulador_Historial objEmpleadoHistorialTab = lstEmpleadoTabHistorial.Where(w => w.clave_empleado == item.clave_empleado).OrderByDescending(o => o.fechaAplicaCambio).OrderByDescending(o => o.id).FirstOrDefault();
                    tblRH_TAB_Tabuladores objTabulador = new tblRH_TAB_Tabuladores();
                    tblRH_TAB_TabuladoresDet objTabuladorDet = new tblRH_TAB_TabuladoresDet();
                    if (objEmpleado.puesto != null)
                    {
                        objTabulador = lstTabuladores.Where(w => w.FK_Puesto == objEmpleado.puesto).FirstOrDefault();
                        if (objTabulador != null)
                        {
                            // TO DO
                            List<tblRH_TAB_TabuladoresDet> lstTabuladoresDetRelPuesto = lstTabuladoresDet.Where(w => w.FK_Tabulador == objTabulador.id).ToList();
                            if (lstTabuladoresDetRelPuesto.Count() == 1)
                                objTabuladorDet = lstTabuladoresDetRelPuesto.FirstOrDefault();
                            else
                                objTabuladorDet = new tblRH_TAB_TabuladoresDet();
                        }
                    }

                    if (objEmpleado != null && objEmpleadoHistorialTab != null && objTabulador != null && objTabuladorDet != null)
                    {
                        objTabuladorDTO = new TabuladorDetDTO();

                        #region INFORMACIÓN GENERAL DEL PUESTO
                        tblRH_EK_Puestos objPuesto = lstPuestos.Where(w => w.puesto == objEmpleado.puesto).FirstOrDefault();
                        if (objPuesto == null)
                            throw new Exception("Ocurrió un error al obtener la descripción del puesto.");

                        int FK_Tabulador = objTabulador.id;
                        int FK_TabuladorDet = objTabuladorDet.id;
                        int FK_Puesto = objTabulador.FK_Puesto;
                        int FK_LineaNegocio = objTabuladorDet.FK_LineaNegocio;
                        int FK_AreaDepartamento = objPuesto.FK_AreaDepartamento;

                        // SE OBTIENE LAS CATEGORIAS QUE CONTENGA EL TABULADOR
                        List<int> lstCategoriasRelTabulador = _context.tblRH_TAB_TabuladoresDet.Where(w => w.FK_Tabulador == FK_Tabulador && w.registroActivo).Select(s => s.FK_Categoria).ToList();

                        tblRH_EK_Tipos_Nomina objTipoNomina = lstTipoNominas.Where(w => w.tipo_nomina == objPuesto.FK_TipoNomina).FirstOrDefault();
                        if (objTipoNomina == null)
                            throw new Exception("Ocurrió un error al obtener el tipo de nómina del puesto.");

                        tblRH_TAB_CatAreaDepartamento objAreaDepartamento = lstAreasDepartamentos.Where(w => w.id == objPuesto.FK_AreaDepartamento).FirstOrDefault();

                        tblRH_TAB_CatCategorias objCategoria = new tblRH_TAB_CatCategorias();
                        if (objTabuladorDet.FK_Categoria > 0)
                        {
                            objCategoria = lstCategorias.Where(w => w.id == objTabuladorDet.FK_Categoria).FirstOrDefault();
                            if (objCategoria == null)
                                throw new Exception("Ocurrió un error al obtener la categoría del puesto.");
                        }

                        tblRH_TAB_CatEsquemaPago objEsquemaPago = new tblRH_TAB_CatEsquemaPago();
                        if (objTabuladorDet.FK_EsquemaPago > 0)
                        {
                            objEsquemaPago = lstEsquemaPagos.Where(w => w.id == objTabuladorDet.FK_EsquemaPago).FirstOrDefault();
                            if (objEsquemaPago == null)
                                throw new Exception("Ocurrió un error al obtener el esquema de pago del puesto.");
                        }
                        #endregion

                        // SE ARMA PUESTO CON SUS TABULADORES
                        objTabuladorDTO.id = FK_Tabulador;
                        objTabuladorDTO.nombreEmpleado = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(string.Format("{0} {1} {2}", objEmpleado.nombre, objEmpleado.ape_paterno, objEmpleado.ape_materno));
                        objTabuladorDTO.idPuesto = objTabulador.FK_Puesto;
                        objTabuladorDTO.puestoDesc = SetPuestoEmpleado(objPuesto);
                        objTabuladorDTO.soloDescPuesto = objPuesto.descripcion;
                        objTabuladorDTO.FK_Categoria = objCategoria.id;
                        objTabuladorDTO.FK_LineaNegocio = FK_LineaNegocio;
                        objTabuladorDTO.FK_AreaDepartamento = FK_AreaDepartamento;
                        objTabuladorDTO.cc = objEmpleado.cc_contable;
                        objTabuladorDTO.categoriaDesc = !string.IsNullOrEmpty(objCategoria.concepto) ? string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button><br>", !string.IsNullOrEmpty(objCategoria.concepto) ? objCategoria.concepto.Trim() : string.Empty) : "-";
                        objTabuladorDTO.tipoNominaDesc = !string.IsNullOrEmpty(objTipoNomina.descripcion) ? objTipoNomina.descripcion.Trim() : string.Empty;

                        decimal sueldoMensual = 0;
                        if (objTipoNomina.tipo_nomina == (int)TipoNominaEnum.quincenal)
                            sueldoMensual = (decimal)objEmpleadoHistorialTab.suma * 2;
                        else if (objTipoNomina.tipo_nomina == (int)TipoNominaEnum.semanal)
                            sueldoMensual = ((decimal)objEmpleadoHistorialTab.suma / 7) * (decimal)30.4;

                        objTabuladorDTO.sueldoBaseStringActual += string.Format("<div class='row'><div class='col-lg-12'><input style='margin-bottom: 3px;' type='text' id='txtCE_Actual_SueldoBase_{0}' class='form-control sueldoBase' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, objEmpleadoHistorialTab.salario_base.ToString("C"), FK_TabuladorDet, FK_Puesto, FK_Tabulador);
                        objTabuladorDTO.complementoStringActual += string.Format("<div class='row'><div class='col-lg-12'><input style='margin-bottom: 3px;' type='text' id='txtCE_Actual_Complemento_{0}' class='form-control complemento' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, objEmpleadoHistorialTab.complemento.ToString("C"), FK_TabuladorDet, FK_Puesto, FK_Tabulador);
                        objTabuladorDTO.totalNominalStringActual += string.Format("<div class='row'><div class='col-lg-12'><input style='margin-bottom: 3px;' type='text' id='txtCE_Actual_TotalNominal_{0}' class='form-control totalNominal' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, objEmpleadoHistorialTab.suma.ToString("C"), FK_TabuladorDet, FK_Puesto, FK_Tabulador);
                        objTabuladorDTO.sueldoMensualStringActual += string.Format("<div class='row'><div class='col-lg-12'><input style='margin-bottom: 3px;' type='text' id='txtCE_Actual_SueldoMensual_{0}' class='form-control sueldoMensual' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, sueldoMensual.ToString("C"), FK_TabuladorDet, FK_Puesto, FK_Tabulador);

                        objTabuladorDTO.categoriaDescModificacion += SetFillComboCategorias(contadorIndex, objCategoria.id, FK_TabuladorDet, FK_Puesto, FK_Tabulador, lstCategorias.Where(w => lstCategoriasRelTabulador.Contains(w.id)).ToList());
                        objTabuladorDTO.sueldoBaseStringModificacion += string.Format("<div class='row'><div class='col-lg-12'><input style='margin-bottom: 3px;' type='text' id='txtCE_Modificacion_SueldoBase_{0}' class='form-control sueldoBase' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, objEmpleadoHistorialTab.salario_base.ToString("C"), FK_TabuladorDet, FK_Puesto, FK_Tabulador);
                        objTabuladorDTO.complementoStringModificacion += string.Format("<div class='row'><div class='col-lg-12'><input style='margin-bottom: 3px;' type='text' id='txtCE_Modificacion_Complemento_{0}' class='form-control complemento' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, objEmpleadoHistorialTab.complemento.ToString("C"), FK_TabuladorDet, FK_Puesto, FK_Tabulador);
                        objTabuladorDTO.totalNominalStringModificacion += string.Format("<div class='row'><div class='col-lg-12'><input style='margin-bottom: 3px;' type='text' id='txtCE_Modificacion_TotalNominal_{0}' class='form-control totalNominal' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, objEmpleadoHistorialTab.suma.ToString("C"), FK_TabuladorDet, FK_Puesto, FK_Tabulador);
                        objTabuladorDTO.sueldoMensualStringModificacion += string.Format("<div class='row'><div class='col-lg-12'><input style='margin-bottom: 3px;' type='text' id='txtCE_Modificacion_SueldoMensual_{0}' class='form-control sueldoMensual' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, sueldoMensual.ToString("C"), FK_TabuladorDet, FK_Puesto, FK_Tabulador);

                        objTabuladorDTO.aumentoPorc += string.Format("<div class='row'><div class='col-lg-12'><input style='margin-bottom: 3px;' type='text' id='txtCE_Modificacion_Porc_{0}' class='form-control aumentoPorc' FK_TabuladorDet='{1}' FK_Puesto='{2}' FK_Tabulador='{3}' clave_empleado='{4}' tipoNomina='{5}' esquemaPago='{6}' FK_LineaNegocio='{7}' FK_AreaDepartamento='{8}'></div></div>", contadorIndex, FK_TabuladorDet, FK_Puesto, FK_Tabulador, objEmpleado.clave_empleado, objPuesto.FK_TipoNomina, objEsquemaPago.concepto, FK_LineaNegocio, FK_AreaDepartamento);
                        objTabuladorDTO.contadorIndex = contadorIndex;
                        contadorIndex++;
                        lstTabuladoresDTO.Add(objTabuladorDTO);
                    }
                }

                lstTabuladoresDTO = lstTabuladoresDTO.OrderBy(e => e.soloDescPuesto).ToList();

                resultado.Clear();
                resultado.Add("lstEmpleadosActivosTabHistorial", lstTabuladoresDTO);
                resultado.Add("contadorIndex", contadorIndex);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetTabuladoresEmpleadosActivos", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> GetTabuladoresPuestos(TabuladorDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region CATALOGOS
                List<tblRH_EK_Tipos_Nomina> lstTipoNominas = _context.tblRH_EK_Tipos_Nomina.Where(w => w.esActivo == true).ToList();
                List<tblRH_TAB_CatAreaDepartamento> lstAreasDepartamentos = _context.tblRH_TAB_CatAreaDepartamento.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatCategorias> lstCategorias = _context.tblRH_TAB_CatCategorias.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatEsquemaPago> lstEsquemaPagos = _context.tblRH_TAB_CatEsquemaPago.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_Tabuladores> lstTabuladores = _context.tblRH_TAB_Tabuladores.Where(w => w.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo).ToList();
                List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.tabuladorDetAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo).ToList();
                List<tblRH_TAB_CatLineaNegocio> lstLineaNegocios = _context.tblRH_TAB_CatLineaNegocio.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatLineaNegocioDet> lstLineaNegociosDet = _context.tblRH_TAB_CatLineaNegocioDet.Where(w => w.registroActivo).ToList();
                List<tblRH_EK_Puestos> lstPuestos = _context.tblRH_EK_Puestos.Where(w => w.registroActivo).ToList();
                #endregion

                #region SE OBTIENE LISTADO DE TABULADORES DE PUESTOS
                #region FILTROS
                if (objParamDTO.lstFK_LineaNegocio != null)
                {
                    if (objParamDTO.lstFK_LineaNegocio.Count() > 0)
                    {
                        List<int> lstFK_Tabuladores = lstTabuladoresDet.Where(w => objParamDTO.lstFK_LineaNegocio.Contains(w.FK_LineaNegocio)).Select(s => s.FK_Tabulador).ToList();
                        List<int> lstFK_Puestos = lstTabuladores.Where(w => lstFK_Tabuladores.Contains(w.id)).Select(s => s.FK_Puesto).ToList();
                        lstPuestos = lstPuestos.Where(w => lstFK_Puestos.Contains(w.puesto)).ToList();
                        lstTabuladoresDet = lstTabuladoresDet.Where(w => objParamDTO.lstFK_LineaNegocio.Contains(w.FK_LineaNegocio)).ToList();
                    }
                }

                if (objParamDTO.lstFK_AreaDepartamento != null)
                {
                    if (objParamDTO.lstFK_AreaDepartamento.Count() > 0)
                        lstPuestos = lstPuestos.Where(w => objParamDTO.lstFK_AreaDepartamento.Contains(w.FK_AreaDepartamento)).ToList();
                }

                if (objParamDTO.lstCC != null && objParamDTO.lstCC.Count() > 0)
                {
                    List<int> lstPlantillasID = _context.tblRH_TAB_PlantillasPersonal.Where(w => w.registroActivo && w.plantillaAutorizada == EstatusGestionAutorizacionEnum.AUTORIZADO && objParamDTO.lstCC.Contains(w.cc)).Select(s => s.id).ToList();
                    List<int> lstFK_Puestos = _context.tblRH_TAB_PlantillasPersonalDet.Where(w => w.registroActivo && lstPlantillasID.Contains(w.FK_Plantilla)).Select(s => s.FK_Puesto).ToList();
                    lstPuestos = lstPuestos.Where(w => lstFK_Puestos.Contains(w.puesto)).ToList();
                }

                if (objParamDTO.lstFK_Puestos != null)
                {
                    if (objParamDTO.lstFK_Puestos.Count() > 0)
                        lstPuestos = lstPuestos.Where(w => objParamDTO.lstFK_Puestos.Contains(w.puesto)).ToList();
                }
                #endregion

                List<int> lstPuestosID = lstPuestos.Select(s => s.puesto).ToList();
                lstTabuladores = lstTabuladores.Where(w => lstPuestosID.Contains(w.FK_Puesto)).ToList();
                List<int> lstTabuladoresID = lstTabuladores.Select(s => s.id).ToList();
                lstTabuladoresDet = lstTabuladoresDet.Where(w => lstTabuladoresID.Contains(w.FK_Tabulador)).ToList();

                List<TabuladorDetDTO> lstTabuladoresDTO = new List<TabuladorDetDTO>();
                TabuladorDetDTO objTabuladorDTO = new TabuladorDetDTO();
                int contadorIndex = 1;
                foreach (var itemTabulador in lstTabuladores)
                {
                    objTabuladorDTO = new TabuladorDetDTO();
                    tblRH_TAB_Tabuladores objTabulador = lstTabuladores.Where(w => w.id == itemTabulador.id).FirstOrDefault();
                    if (objTabulador != null)
                    {
                        foreach (var itemTabuladorDet in lstTabuladoresDet.Where(w => w.FK_Tabulador == itemTabulador.id).ToList())
                        {
                            tblRH_EK_Puestos objPuesto = lstPuestos.Where(w => w.puesto == itemTabulador.FK_Puesto).FirstOrDefault();
                            if (objPuesto == null)
                                throw new Exception("Ocurrió un error al obtener la información del puesto.");

                            int FK_Tabulador = objTabulador.id;
                            int FK_TabuladorDet = itemTabuladorDet.id;
                            int FK_Puesto = objTabulador.FK_Puesto;
                            int FK_LineaNegocio = itemTabuladorDet.FK_LineaNegocio;
                            int FK_AreaDepartamento = objPuesto.FK_AreaDepartamento;
                            int FK_EsquemaPago = itemTabuladorDet.FK_EsquemaPago;

                            tblRH_TAB_CatLineaNegocio objLineaNegocio = lstLineaNegocios.Where(w => w.id == FK_LineaNegocio).FirstOrDefault();

                            tblRH_EK_Tipos_Nomina objTipoNomina = lstTipoNominas.Where(w => w.tipo_nomina == objPuesto.FK_TipoNomina).FirstOrDefault();
                            if (objTipoNomina == null)
                                throw new Exception("Ocurrió un error al obtener el tipo de nómina del puesto.");

                            tblRH_TAB_CatAreaDepartamento objAreaDepartamento = lstAreasDepartamentos.Where(w => w.id == objPuesto.FK_AreaDepartamento).FirstOrDefault();

                            tblRH_TAB_CatCategorias objCategoria = lstCategorias.Where(w => w.id == itemTabuladorDet.FK_Categoria).FirstOrDefault();
                            if (objCategoria == null)
                                throw new Exception("Ocurrió un error al obtener la categoría del puesto.");

                            tblRH_TAB_CatEsquemaPago objEsquemaPago = lstEsquemaPagos.Where(w => w.id == itemTabuladorDet.FK_EsquemaPago).FirstOrDefault();
                            if (objEsquemaPago == null)
                                throw new Exception("Ocurrió un error al obtener el esquema de pago del puesto.");

                            // SE ARMA PUESTO CON SUS TABULADORES
                            objTabuladorDTO.id = FK_Tabulador;
                            objTabuladorDTO.idPuesto = objTabulador.FK_Puesto;
                            objTabuladorDTO.puestoDesc = SetPuestoEmpleado(objPuesto);
                            objTabuladorDTO.soloDescPuesto = objPuesto.descripcion;
                            objTabuladorDTO.FK_Categoria = objCategoria != null ? objCategoria.id : 0;
                            objTabuladorDTO.FK_LineaNegocio = FK_LineaNegocio;
                            objTabuladorDTO.lineaNegocioDesc += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;' title='{0}'>{1}</button><br>", 
                                (objLineaNegocio != null ? !string.IsNullOrEmpty(objLineaNegocio.concepto) ? objLineaNegocio.concepto.Trim() : string.Empty : string.Empty), 
                                (objLineaNegocio != null ? !string.IsNullOrEmpty(objLineaNegocio.abreviacion) ? objLineaNegocio.abreviacion.Trim() : string.Empty : string.Empty));
                            objTabuladorDTO.FK_AreaDepartamento = FK_AreaDepartamento;
                            objTabuladorDTO.categoriaDesc += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button><br>", objCategoria != null ? !string.IsNullOrEmpty(objCategoria.concepto) ? objCategoria.concepto.Trim() : string.Empty : string.Empty);
                            objTabuladorDTO.esquemaPagoDesc += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button><br>", objEsquemaPago != null ? !string.IsNullOrEmpty(objEsquemaPago.concepto) ? objEsquemaPago.concepto.Trim() : string.Empty : string.Empty);
                            objTabuladorDTO.tipoNominaDesc = !string.IsNullOrEmpty(objTipoNomina.descripcion) ? objTipoNomina.descripcion.Trim() : string.Empty;

                            objTabuladorDTO.sueldoBaseStringActual += string.Format("<div class='row'><div class='col-lg-12'><input style='margin-bottom: 3px;' type='text' id='txtCE_Actual_SueldoBase_{0}' class='form-control sueldoBase' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, itemTabuladorDet.sueldoBase.ToString("C"), FK_TabuladorDet, FK_Puesto, FK_Tabulador);
                            objTabuladorDTO.complementoStringActual += string.Format("<div class='row'><div class='col-lg-12'><input style='margin-bottom: 3px;' type='text' id='txtCE_Actual_Complemento_{0}' class='form-control complemento' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, itemTabuladorDet.complemento.ToString("C"), FK_TabuladorDet, FK_Puesto, FK_Tabulador);
                            objTabuladorDTO.totalNominalStringActual += string.Format("<div class='row'><div class='col-lg-12'><input style='margin-bottom: 3px;' type='text' id='txtCE_Actual_TotalNominal_{0}' class='form-control totalNominal' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, itemTabuladorDet.totalNominal.ToString("C"), FK_TabuladorDet, FK_Puesto, FK_Tabulador);
                            objTabuladorDTO.sueldoMensualStringActual += string.Format("<div class='row'><div class='col-lg-12'><input style='margin-bottom: 3px;' type='text' id='txtCE_Actual_SueldoMensual_{0}' class='form-control sueldoMensual' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, itemTabuladorDet.sueldoMensual.ToString("C"), FK_TabuladorDet, FK_Puesto, FK_Tabulador);

                            objTabuladorDTO.categoriaDescModificacion += SetFillComboCategorias(contadorIndex, objCategoria.id, FK_TabuladorDet, FK_Puesto, FK_Tabulador, lstCategorias);
                            objTabuladorDTO.esquemaPagoDescModificacion += SetFillComboEsquemaPagos(contadorIndex, objEsquemaPago.id, FK_TabuladorDet, FK_Puesto, FK_Tabulador, lstEsquemaPagos);
                            objTabuladorDTO.sueldoBaseStringModificacion += string.Format("<div class='row'><div class='col-lg-12'><input style='margin-bottom: 3px;' type='text' id='txtCE_Modificacion_SueldoBase_{0}' class='form-control sueldoBase' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, itemTabuladorDet.sueldoBase.ToString("C"), FK_TabuladorDet, FK_Puesto, FK_Tabulador);
                            objTabuladorDTO.complementoStringModificacion += string.Format("<div class='row'><div class='col-lg-12'><input style='margin-bottom: 3px;' type='text' id='txtCE_Modificacion_Complemento_{0}' class='form-control complemento' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, itemTabuladorDet.complemento.ToString("C"), FK_TabuladorDet, FK_Puesto, FK_Tabulador);
                            objTabuladorDTO.totalNominalStringModificacion += string.Format("<div class='row'><div class='col-lg-12'><input style='margin-bottom: 3px;' type='text' id='txtCE_Modificacion_TotalNominal_{0}' class='form-control totalNominal' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, itemTabuladorDet.totalNominal.ToString("C"), FK_TabuladorDet, FK_Puesto, FK_Tabulador);
                            objTabuladorDTO.sueldoMensualStringModificacion += string.Format("<div class='row'><div class='col-lg-12'><input style='margin-bottom: 3px;' type='text' id='txtCE_Modificacion_SueldoMensual_{0}' class='form-control sueldoMensual' value='{1}' FK_TabuladorDet='{2}' FK_Puesto='{3}' FK_Tabulador='{4}'></div></div>", contadorIndex, itemTabuladorDet.sueldoMensual.ToString("C"), FK_TabuladorDet, FK_Puesto, FK_Tabulador);

                            objTabuladorDTO.aumentoPorc += string.Format("<div class='row'><div class='col-lg-12'><input style='margin-bottom: 3px;' type='text' id='txtCE_Modificacion_Porc_{0}' class='form-control aumentoPorc' FK_TabuladorDet='{1}' FK_Puesto='{2}' FK_Tabulador='{3}' tipoNomina='{4}' esquemaPago='{5}' FK_LineaNegocio='{6}' FK_AreaDepartamento='{7}' FK_EsquemaPago='{8}'></div></div>", contadorIndex, FK_TabuladorDet, FK_Puesto, FK_Tabulador, objPuesto.FK_TipoNomina, objEsquemaPago.concepto, FK_LineaNegocio, FK_AreaDepartamento, FK_EsquemaPago);
                            objTabuladorDTO.contadorIndex = contadorIndex;
                            contadorIndex++;
                        }
                        lstTabuladoresDTO.Add(objTabuladorDTO);
                    }
                }

                lstTabuladoresDTO = lstTabuladoresDTO.OrderBy(o => o.soloDescPuesto).ToList();

                resultado.Clear();
                resultado.Add("lstTabPuestos", lstTabuladoresDTO);
                resultado.Add("contadorIndex", contadorIndex);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetTabuladoresPuestos", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        #endregion

        #region GESTIÓN MODIFICACIÓN
        public Dictionary<string, object> GetGestionModificacion(GestionModificacionTabuladorDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region CATALOGOS
                List<tblRH_TAB_GestionAutorizantes> lstGestionAutorizantes = _context.tblRH_TAB_GestionAutorizantes.Where(w => w.registroActivo && w.vistaAutorizacion == VistaAutorizacionEnum.MODIFICACION_TABULADORES).ToList();
                List<tblRH_TAB_CatLineaNegocio> lstCatLineasNegocios = _context.tblRH_TAB_CatLineaNegocio.Where(w => w.registroActivo).ToList();
                #endregion

                #region SE OBTIENE LISTADO DE GESTIÓN DE MODIFICACION
                List<tblRH_TAB_GestionModificacionTabulador> lstGestionModificacion = _context.tblRH_TAB_GestionModificacionTabulador.Where(w => w.modificacionAutorizada == objParamDTO.modificacionAutorizada && w.registroActivo).ToList();
                List<tblRH_TAB_GestionModificacionTabuladorDet> lstGestionModificacionDet = _context.tblRH_TAB_GestionModificacionTabuladorDet.Where(w => w.registroActivo).ToList();

                List<GestionModificacionTabuladorDTO> lstGestionDTO = new List<GestionModificacionTabuladorDTO>();
                GestionModificacionTabuladorDTO objGestionDTO = new GestionModificacionTabuladorDTO();
                foreach (var item in lstGestionModificacion)
                {
                    objGestionDTO = new GestionModificacionTabuladorDTO();

                    #region AUTORIZANTES
                    List<tblRH_TAB_GestionAutorizantes> lstAutorizantes = lstGestionAutorizantes.Where(w => w.FK_Registro == item.id).OrderBy(o => o.nivelAutorizante).ToList();

                    int? sigAuth = null;
                    foreach (var itemAuth in lstAutorizantes)
                    {
                        if (itemAuth.autorizado == EstatusGestionAutorizacionEnum.AUTORIZADO || itemAuth.autorizado == EstatusGestionAutorizacionEnum.RECHAZADO)
                            objGestionDTO.esFirmar = false;
                        else
                        {
                            if (sigAuth == null)
                                sigAuth = itemAuth.FK_UsuarioAutorizacion;

                            if (sigAuth.Value == vSesiones.sesionUsuarioDTO.id)
                            {
                                objGestionDTO.esFirmar = true;
                                break;
                            }
                            else
                                objGestionDTO.esFirmar = false;
                        }
                    }
                    #endregion

                    // SE OBTIENE DETALLE DE LA MODIFICACIÓN
                    List<tblRH_TAB_GestionModificacionTabuladorDet> lstGestionModificacionDetRelEncabezado = lstGestionModificacionDet.Where(w => w.FK_IncrementoAnual == item.id).ToList();
                    if (lstGestionModificacionDetRelEncabezado.Count() <= 0)
                        throw new Exception("Ocurrió un error al obtener el detalle de la modificación.");

                    // SE OBTIENE LA LINEA DE NEGOCIO
                    tblRH_TAB_CatLineaNegocio objLineaNegocio = lstCatLineasNegocios.Where(w => w.id == lstGestionModificacionDetRelEncabezado[0].FK_LineaNegocio).FirstOrDefault();

                    objGestionDTO.id = item.id;
                    objGestionDTO.tipoModificacion = item.tipoModificacion;
                    objGestionDTO.tipoModificacionStr = EnumHelper.GetDescription((TipoModificacionEnum)item.tipoModificacion);
                    objGestionDTO.fechaCreacion = item.fechaCreacion;
                    objGestionDTO.fechaAplicaCambio = Convert.ToDateTime(lstGestionModificacionDetRelEncabezado[0].fechaAplicaCambio);
                    objGestionDTO.lineaNegocioStr = string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;' title='{0}'>{1}</button><br>", (objLineaNegocio != null ? !string.IsNullOrEmpty(objLineaNegocio.concepto) ? objLineaNegocio.concepto.Trim() : string.Empty : string.Empty), (objLineaNegocio != null ? !string.IsNullOrEmpty(objLineaNegocio.abreviacion) ? objLineaNegocio.abreviacion.Trim() : string.Empty : string.Empty));
                    objGestionDTO.modificacionAutorizada = item.modificacionAutorizada;
                    objGestionDTO.comentarioRechazo = item.comentarioRechazo;
                    objGestionDTO.FK_Tabulador = lstGestionModificacionDetRelEncabezado[0].FK_Tabulador;
                    lstGestionDTO.Add(objGestionDTO);
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("lstGestionDTO", lstGestionDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetGestionModificacion", e, AccionEnum.CONSULTA, objParamDTO.id, objParamDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> AutorizarRechazarGestionModificacion(GestionModificacionTabuladorDTO objParamDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado.Clear();
                try
                {
                    #region VALIDACIONES
                    if (objParamDTO.id <= 0) { throw new Exception("Ocurrió un error al realizar la acción."); }
                    if (objParamDTO.modificacionAutorizada <= 0) { throw new Exception("Ocurrió un error al realizar la acción."); }
                    #endregion

                    #region SE AUTORIZA/RECHAZA LA MODIFICACIÓN
                    tblRH_TAB_GestionModificacionTabulador objGestion = _context.tblRH_TAB_GestionModificacionTabulador.Where(w => w.id == objParamDTO.id && w.registroActivo).FirstOrDefault();
                    List<tblRH_TAB_GestionAutorizantes> lstFirmas = _context.tblRH_TAB_GestionAutorizantes.Where(w => w.registroActivo && w.vistaAutorizacion == VistaAutorizacionEnum.MODIFICACION_TABULADORES && w.FK_Registro == objParamDTO.id).ToList();

                    List<string> lstCorreosNotificarTodos = new List<string>();
                    List<string> lstCorreosNotificarRestantes = new List<string>();

                    var objTipoMod = _context.tblRH_TAB_CatTipoModificacion.FirstOrDefault(e => e.id == (int)objGestion.tipoModificacion);

                    int totalAuth = 0;
                    bool notifyNextAuth = false;
                    int totalAlertas = 0;

                    foreach (var item in lstFirmas)
                    {
                        tblP_Usuario objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == item.FK_UsuarioAutorizacion);

                        #region AGREGAR ALERTA Y CORREO(a la lista de correos) PARA EL SIGUIENTE AUTORIZANTE
                        if (notifyNextAuth && objParamDTO.modificacionAutorizada == EstatusGestionAutorizacionEnum.AUTORIZADO && totalAlertas == 0)
                        {
                            #region Alerta SIGOPLAN
                            tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                            objNuevaAlerta = new tblP_Alerta();
                            objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                            objNuevaAlerta.userRecibeID = item.FK_UsuarioAutorizacion;
#if DEBUG
                            objNuevaAlerta.userRecibeID = _USUARIO_OMAR_NUNEZ_ID; //USUARIO ID: Omar Nuñez.
#endif
                            objNuevaAlerta.tipoAlerta = 2;
                            objNuevaAlerta.sistemaID = 16;
                            objNuevaAlerta.visto = false;
                            objNuevaAlerta.url = "/Administrativo/Tabuladores/GestionModificacion";
                            objNuevaAlerta.objID = objGestion.id;
                            objNuevaAlerta.obj = "AutorizarModificacion";
                            objNuevaAlerta.msj = "Modificación Pendiente de Autorizar";
                            objNuevaAlerta.documentoID = 0;
                            objNuevaAlerta.moduloID = 0;
                            _context.tblP_Alerta.Add(objNuevaAlerta);
                            _context.SaveChanges();
                            #endregion //ALERTA SIGPLAN

                            //SIGUIENTE EN SER AUTORIZADO
                            lstCorreosNotificarRestantes.Add(objUsuario.correo);

                            //NOTIFICADA
                            notifyNextAuth = false;
                            totalAlertas++;
                        }

                        #endregion

                        if (item.autorizado == EstatusGestionAutorizacionEnum.AUTORIZADO)
                            totalAuth++;
                        else
                        {
                            if (item.FK_UsuarioAutorizacion == vSesiones.sesionUsuarioDTO.id)
                            {
                                if (objParamDTO.modificacionAutorizada == EstatusGestionAutorizacionEnum.AUTORIZADO)
                                {
                                    notifyNextAuth = true;
                                    totalAuth++;
                                }

                                item.autorizado = objParamDTO.modificacionAutorizada;
                                item.fechaFirma = DateTime.Now;
                                _context.SaveChanges();

                                #region SE ELIMINA LA ALERTA AL USUARIO QUE AUTORIZO
                                tblP_Alerta objAlerta = _context.tblP_Alerta.Where(w => w.obj == "AutorizarModificacion" && w.msj == "Modificación Pendiente de Autorizar" && w.objID == objParamDTO.id && w.userRecibeID == (int)vSesiones.sesionUsuarioDTO.id).FirstOrDefault();

                                if (objAlerta != null)
                                {
                                    objAlerta.visto = true;
                                    _context.SaveChanges();
                                }

                                #endregion
                            }
                        }
                    }

                    #region CORREO

                    #region REPORTE
                    string RutaServidor = "";
#if DEBUG
                    RutaServidor = @"C:\Proyectos\SIGOPLANv2\REPORTESCR\CAPITAL_HUMANO";
#else
                    RutaServidor = @"\\10.1.0.112\Proyecto\SIGOPLAN\REPORTESCR\CAPITAL_HUMANO";
#endif
                    string strModLineasNegocio = string.Empty;
                    ReportDocument rptCV = new ReportDocument();
                    if (objGestion.tipoModificacion == TipoModificacionEnum.MODIFICACION_A_PUESTOS)
                    {
                        rptCV = new rptRepTabuladoresModificacion();
                        //string path = Path.Combine(RutaServidor, "rptRepTabuladoresModificacion.rpt");
                        //rptCV.Load(path);

                        //var objParamsDTO = Session["objParamsPDF"] as TabuladorDTO;
                        TabuladorDTO objParamsDTO = new TabuladorDTO();
                        objParamsDTO.FK_IncrementoAnual = objGestion.id;

                        var dictRepTabuladores = GetTabuladoresModificacionReportePDF((objParamsDTO));
                        List<RepTabuladoresModificacionDTO> lstTabuladores = dictRepTabuladores["lstTabPuestos"] as List<RepTabuladoresModificacionDTO>;

                        List<string> lstDescLN = dictRepTabuladores["lstLineasDeNegocios"] as List<string>;
                        strModLineasNegocio = lstDescLN != null && lstDescLN.Count() > 0 ? (string.Join(", ", lstDescLN)) : "";

                        //LIMPIAR DICCIONARIO
                        resultado.Clear();

                        objParamsDTO.lstDescLineaNegocio = new List<string>();
                        objParamsDTO.lstDescLineaNegocio.Add("LN");

                        objParamsDTO.lstDescCC = new List<string>();
                        objParamsDTO.lstDescCC.Add("DESC");

                        objParamsDTO.lstReporteAutorizantesDTO = new List<RepAutorizantesTABDTO>();
                        RepAutorizantesTABDTO obj = new RepAutorizantesTABDTO();
                        objParamsDTO.lstReporteAutorizantesDTO.Add(obj);

                        string lstLineasNegocio = string.Join(", ", objParamsDTO.FK_LineaNegocio);
                        string lstCC = string.Join(", ", objParamsDTO.lstDescCC);

                        rptCV.Database.Tables[0].SetDataSource(getInfoEnca("Reporte de Modificacion de Tabuladores", "Dirección de Capital Humano"));
                        rptCV.Database.Tables[1].SetDataSource(lstTabuladores);
                        rptCV.Database.Tables[2].SetDataSource(objParamsDTO.lstReporteAutorizantesDTO);

                        rptCV.SetParameterValue("lstLineasNegocio", lstLineasNegocio);
                        rptCV.SetParameterValue("lstCC", lstCC);
                        rptCV.SetParameterValue("año", objParamsDTO.añoReporte);
                        rptCV.SetParameterValue("fecha", DateTime.Now.ToString("dd/MM/yyyy h:mm tt"));
                    }
                    else
                    {
                        rptCV = new rptRepTabuladoresModificacionEmpleados();
                        //string path = Path.Combine(RutaServidor, "rptRepTabuladoresModificacionEmpleados.rpt");
                        //rptCV.Load(path);

                        //var objParamsDTO = Session["objParamsPDF"] as TabuladorDTO;
                        TabuladorDTO objParamsDTO = new TabuladorDTO();
                        objParamsDTO.FK_IncrementoAnual = objGestion.id;

                        var dictRepTabuladores = GetTabuladoresModificacionReportePDF((objParamsDTO));
                        List<RepTabuladoresModificacionDTO> lstTabuladores = dictRepTabuladores["lstTabPuestos"] as List<RepTabuladoresModificacionDTO>;

                        List<string> lstDescLN = dictRepTabuladores["lstLineasDeNegocios"] as List<string>;
                        strModLineasNegocio = lstDescLN != null && lstDescLN.Count() > 0 ? (string.Join(", ", lstDescLN)) : "";

                        //LIMPIAR DICCIONARIO
                        resultado.Clear();

                        objParamsDTO.lstDescLineaNegocio = new List<string>();
                        objParamsDTO.lstDescLineaNegocio.Add("LN");

                        objParamsDTO.lstDescCC = new List<string>();
                        objParamsDTO.lstDescCC.Add("DESC");

                        objParamsDTO.lstReporteAutorizantesDTO = new List<RepAutorizantesTABDTO>();
                        RepAutorizantesTABDTO obj = new RepAutorizantesTABDTO();
                        objParamsDTO.lstReporteAutorizantesDTO.Add(obj);

                        string lstLineasNegocio = string.Join(", ", objParamsDTO.FK_LineaNegocio);
                        string lstCC = string.Join(", ", objParamsDTO.lstDescCC);

                        rptCV.Database.Tables[0].SetDataSource(getInfoEnca("Reporte de Modificacion de Tabuladores", "Dirección de Capital Humano"));
                        rptCV.Database.Tables[1].SetDataSource(lstTabuladores);
                        rptCV.Database.Tables[2].SetDataSource(objParamsDTO.lstReporteAutorizantesDTO);

                        rptCV.SetParameterValue("lstLineasNegocio", lstLineasNegocio);
                        rptCV.SetParameterValue("lstCC", lstCC);
                        rptCV.SetParameterValue("año", objParamsDTO.añoReporte);
                        rptCV.SetParameterValue("fecha", DateTime.Now.ToString("dd/MM/yyyy h:mm tt"));
                    }
                    Stream stream = rptCV.ExportToStream(ExportFormatType.PortableDocFormat);
                    #endregion

                    string cuerpo =
                                @"<html>
                                    <head>
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
                                                background-color: #ffcc5c;
                                            }
                                        </style>
                                    </head>
                                    <body lang=ES-MX link='#0563C1' vlink='#954F72'>

                                            <p class=MsoNormal>
                                                Buen día,<br><br>" +
                                                (totalAuth == lstFirmas.Count() ? "Se ha autorizado una Modificación de Tabuladores por todos los firmantes" : "Se ha autorizado una Modificación de Tabuladores")
                                                + @".<br><br>
                                            </p>
                                            <p class=MsoNormal style='font-weight:bold;'>" +
                                                "Modificacion: " + objTipoMod.concepto + @".<o:p></o:p>
                                            </p>
                                            <br><br><br>
                                            ";

                    #region TABLA AUTORIZANTES

                    cuerpo += @"
                                        <table>
                                            <thead>
                                                <tr>
                                                    <th>Nombre</th>
                                                    <th>Tipo</th>
                                                    <th>Autorizo</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            ";

                    bool esAuth = false;
                    int totalSiguientes = 0;

                    //EstatusGestionAutorizacionEnum P 0 A 1 R 2 
                    foreach (var itemDet in lstFirmas)
                    {
                        tblP_Usuario objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == itemDet.FK_UsuarioAutorizacion);
                        string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                        cuerpo += "<tr>" +
                                    "<td>" + nombreCompleto + "</td>" +
                                    "<td>" + EnumHelper.GetDescription(itemDet.nivelAutorizante) + "</td>" +
                                    GetEstatusTabulador((int)itemDet.autorizado, esAuth) +
                                "</tr>";

                        if (vSesiones.sesionUsuarioDTO.id == itemDet.FK_UsuarioAutorizacion && totalSiguientes == 0)
                        {
                            esAuth = true;
                            totalSiguientes++;
                        }
                        else
                        {
                            if (esAuth)
                            {
                                esAuth = false;

                                if (itemDet.autorizado == EstatusGestionAutorizacionEnum.AUTORIZADO)
                                {
                                    esAuth = true;
                                    totalSiguientes = 0;
                                }
                            }
                        }
                    }

                    cuerpo += "</tbody>" +
                                "</table>" +
                                "<br><br><br>";


                    #endregion

                    cuerpo += "<br><br><br>" +
                          "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                          "Capital Humano > Administración de personal > Configuración > Estructura Salarial > Gestión de Reportes.<br><br>" +
                          "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                          "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias." +
                        " </body>" +
                      "</html>";

                    #endregion

                    if (totalAuth == lstFirmas.Count())
                    {
                        objGestion.modificacionAutorizada = objParamDTO.modificacionAutorizada;
                        objGestion.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objGestion.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();

                        TipoModificacionEnum tipoModificacion = objGestion.tipoModificacion;
                        switch (tipoModificacion)
                        {
                            case TipoModificacionEnum.INCREMENTO_ANUAL_A_EMPLEADOS_ACTIVOS:
                                #region SE APLICA LA MODIFICACIÓN DE AUMENTO ANUAL EN BASE UN % AL EMPLEADO QUE CONTENGA EL PUESTO
                                {
                                    List<tblRH_TAB_GestionModificacionTabuladorDet> lstModificaciones = _context.tblRH_TAB_GestionModificacionTabuladorDet.Where(w => w.FK_IncrementoAnual == objParamDTO.id && w.registroActivo).ToList();
                                    foreach (var item in lstModificaciones)
                                    {
                                        #region SE REGISTRA EL NUEVO INCREMENTO AL EMPLEADO
                                        tblRH_EK_Tabulador_Historial objHistorial = new tblRH_EK_Tabulador_Historial();
                                        objHistorial.clave_empleado = item.clave_empleado;
                                        objHistorial.tabulador = item.tabulador;
                                        objHistorial.tabulador_anterior = item.tabulador_anterior;
                                        objHistorial.FK_Tabulador = item.FK_Tabulador;
                                        objHistorial.FK_TabuladorDet = item.FK_TabuladorDet;
                                        objHistorial.fecha_cambio = Convert.ToDateTime(item.fechaAplicaCambio);
                                        objHistorial.fechaAplicaCambio = item.fechaAplicaCambio;
                                        objHistorial.hora = item.hora.Value;
                                        objHistorial.suma = item.suma;
                                        objHistorial.salario_base = item.salario_base;
                                        objHistorial.complemento = item.complemento;
                                        objHistorial.bono_zona = item.bono_zona;
                                        objHistorial.motivoCambio = 2; // INCREMENTO ANUAL
                                        objHistorial.FK_UsuarioCreacion = item.FK_UsuarioCreacion;
                                        objHistorial.fechaCreacion = DateTime.Now;
                                        objHistorial.esActivo = true;
                                        _context.tblRH_EK_Tabulador_Historial.Add(objHistorial);
                                        _context.SaveChanges();
                                        #endregion

                                        #region SE INDICA EN EL DETALLE DE LA GESTIÓN, QUE EL INCREMENTO YA FUE APLICADO
                                        tblRH_TAB_GestionModificacionTabuladorDet objGestionModificacion = _context.tblRH_TAB_GestionModificacionTabuladorDet.Where(w => w.id == item.id).FirstOrDefault();
                                        if (objGestionModificacion == null)
                                            throw new Exception("Ocurrió un error al indicar que el incremento fue aplicado.");

                                        objGestionModificacion.registroAplicado = EstatusGestionAutorizacionEnum.AUTORIZADO;
                                        objGestionModificacion.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                        objGestionModificacion.fechaModificacion = DateTime.Now;
                                        _context.SaveChanges();
                                        #endregion
                                    }
                                }
                                #endregion
                                break;
                            case TipoModificacionEnum.MODIFICACION_A_PUESTOS:
                                #region SE APLICA LA MODIFICACIÓN AL PUESTO
                                {
                                    #region SE ENVIA A HISTORIAL COMO BACKUP LA INFORMACIÓN DEL PUESTO A MODIFICAR
                                    List<tblRH_TAB_GestionModificacionTabuladorDet> lstModificaciones = _context.tblRH_TAB_GestionModificacionTabuladorDet.Where(w => w.FK_IncrementoAnual == objParamDTO.id && w.registroActivo).ToList();
                                    if (lstModificaciones == null)
                                        throw new Exception("Ocurrió un error al obtener los puestos a modificar.");

                                    // SE OBTIENE EL TABULADOR ACTUAL PARA ENVIARLO A HISTOIAL
                                    foreach (var objModificacion in lstModificaciones)
                                    {
                                        int FK_Tabulador = objModificacion.FK_Tabulador;
                                        int FK_LineaNegocio = objModificacion.FK_LineaNegocio;
                                        tblRH_TAB_Tabuladores objTabulador = _context.tblRH_TAB_Tabuladores.Where(w => w.id == FK_Tabulador && w.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo).FirstOrDefault();
                                        List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.FK_Tabulador == FK_Tabulador && w.FK_LineaNegocio == FK_LineaNegocio && w.tabuladorDetAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo).ToList();

                                        #region SE ENVIA EL TABULADOR A HISTORIAL PARA PROCEDER CON LA MODIFICACIÓN
                                        // ENCABEZADO
                                        tblRH_TAB_TabuladoresHistorial objTabuladorHistorial = new tblRH_TAB_TabuladoresHistorial();
                                        objTabuladorHistorial.FK_Tabulador = objTabulador.id;
                                        objTabuladorHistorial.FK_Puesto = objTabulador.FK_Puesto;
                                        objTabuladorHistorial.tabuladorAutorizado = objTabulador.tabuladorAutorizado;
                                        objTabuladorHistorial.comentarioRechazo = objTabulador.comentarioRechazo;
                                        objTabuladorHistorial.FK_UsuarioCreacion = objTabulador.FK_UsuarioCreacion;
                                        objTabuladorHistorial.FK_UsuarioModificacion = objTabulador.FK_UsuarioModificacion;
                                        objTabuladorHistorial.fechaCreacion = objTabulador.fechaCreacion;
                                        objTabuladorHistorial.fechaModificacion = objTabulador.fechaModificacion;
                                        objTabuladorHistorial.registroActivo = objTabulador.registroActivo;
                                        _context.tblRH_TAB_TabuladoresHistorial.Add(objTabuladorHistorial);
                                        _context.SaveChanges();

                                        // DETALLE
                                        tblRH_TAB_TabuladoresDetHistorial objTabuladorHistorialDet = new tblRH_TAB_TabuladoresDetHistorial();
                                        List<tblRH_TAB_TabuladoresDetHistorial> lstTabuladoresHistorialDet = new List<tblRH_TAB_TabuladoresDetHistorial>();
                                        foreach (var item in lstTabuladoresDet)
                                        {
                                            objTabuladorHistorialDet = new tblRH_TAB_TabuladoresDetHistorial();
                                            objTabuladorHistorialDet.FK_Tabulador = item.FK_Tabulador;
                                            objTabuladorHistorialDet.FK_LineaNegocio = item.FK_LineaNegocio;
                                            objTabuladorHistorialDet.FK_Categoria = item.FK_Categoria;
                                            objTabuladorHistorialDet.tabuladorDetAutorizado = item.tabuladorDetAutorizado;
                                            objTabuladorHistorialDet.sueldoBase = (decimal)item.sueldoBase;
                                            objTabuladorHistorialDet.complemento = (decimal)item.complemento;
                                            objTabuladorHistorialDet.totalNominal = (decimal)item.totalNominal;
                                            objTabuladorHistorialDet.sueldoMensual = (decimal)item.sueldoMensual;
                                            objTabuladorHistorialDet.FK_EsquemaPago = item.FK_EsquemaPago;
                                            objTabuladorHistorialDet.FK_UsuarioCreacion = item.FK_UsuarioCreacion;
                                            objTabuladorHistorialDet.FK_UsuarioModificacion = item.FK_UsuarioModificacion;
                                            objTabuladorHistorialDet.fechaCreacion = item.fechaCreacion;
                                            objTabuladorHistorialDet.fechaModificacion = item.fechaModificacion;
                                            objTabuladorHistorialDet.registroActivo = item.registroActivo;
                                            lstTabuladoresHistorialDet.Add(objTabuladorHistorialDet);
                                        }
                                        _context.tblRH_TAB_TabuladoresDetHistorial.AddRange(lstTabuladoresHistorialDet);
                                        _context.SaveChanges();
                                        #endregion
                                    }

                                    #region SE REGISTRA LA MODIFICACIÓN DEL TABULADOR
                                    tblRH_TAB_TabuladoresDet objTabuladorDetModificacion = new tblRH_TAB_TabuladoresDet();
                                    List<tblRH_TAB_TabuladoresDet> lstTabuladoresDetModificacion = new List<tblRH_TAB_TabuladoresDet>();
                                    foreach (var item in lstModificaciones)
                                    {
                                        objTabuladorDetModificacion = _context.tblRH_TAB_TabuladoresDet.Where(w => w.id == item.FK_TabuladorDet && w.registroActivo).FirstOrDefault();
                                        objTabuladorDetModificacion.FK_Tabulador = item.FK_Tabulador;
                                        objTabuladorDetModificacion.FK_LineaNegocio = item.FK_LineaNegocio;
                                        objTabuladorDetModificacion.FK_Categoria = item.FK_Categoria;
                                        objTabuladorDetModificacion.tabuladorDetAutorizado = item.tabuladorDetAutorizado;
                                        objTabuladorDetModificacion.sueldoBase = item.salario_base;
                                        objTabuladorDetModificacion.complemento = item.complemento;
                                        objTabuladorDetModificacion.totalNominal = item.suma;
                                        objTabuladorDetModificacion.sueldoMensual = item.totalMensual;
                                        objTabuladorDetModificacion.FK_EsquemaPago = item.FK_EsquemaPago;
                                        objTabuladorDetModificacion.FK_UsuarioCreacion = item.FK_UsuarioCreacion;
                                        objTabuladorDetModificacion.FK_UsuarioModificacion = item.FK_UsuarioModificacion;
                                        objTabuladorDetModificacion.fechaCreacion = item.fechaCreacion;
                                        objTabuladorDetModificacion.fechaModificacion = item.fechaModificacion;
                                        objTabuladorDetModificacion.registroActivo = item.registroActivo;
                                        lstTabuladoresDetModificacion.Add(objTabuladorDetModificacion);
                                    }
                                    _context.SaveChanges();
                                    #endregion
                                    #endregion
                                }
                                #endregion
                                break;
                            default:
                                break;
                        }

                        lstCorreosNotificarTodos.Add(_CORREO_DIANA_ALVAREZ);
                        lstCorreosNotificarTodos.Add(_CORREO_MANUEL_CRUZ);
#if DEBUG
                        lstCorreosNotificarTodos = new List<string>();
                        lstCorreosNotificarTodos.Add(_CORREO_OMAR_NUNEZ);
#endif
                        List<byte[]> downloadPDFs = new List<byte[]>();
                        using (var streamReader = new MemoryStream())
                        {
                            stream.CopyTo(streamReader);
                            downloadPDFs.Add(streamReader.ToArray());

                            GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: MODIFICACIÓN TABULADORES AUTORIZADO POR TODOS LOS FIRMANTES FECHA: {1}",
                                PersonalUtilities.GetNombreEmpresa(), objGestion.fechaCreacion.ToString("dd/MM/yyyy")),
                                cuerpo, lstCorreosNotificarTodos, downloadPDFs, "Modificacion.pdf");

                        }
                    }
                    else
                    {
                        #region SE RECHAZA LA MODIFICACIÓN
                        if (objParamDTO.modificacionAutorizada == EstatusGestionAutorizacionEnum.RECHAZADO)
                        {
                            // SE RECHAZA LA MODIFICACIÓN (REGISTRO PRINCIPAL)
                            objGestion.modificacionAutorizada = objParamDTO.modificacionAutorizada;
                            objGestion.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objGestion.fechaModificacion = DateTime.Now;
                            _context.SaveChanges();

                            // SE RECHAZA EL DETALLE DE LA MODIFICACIÓN (DETALLE)
                            List<tblRH_TAB_GestionModificacionTabuladorDet> lstGestionTabuladoresDet = _context.tblRH_TAB_GestionModificacionTabuladorDet.Where(w => w.FK_IncrementoAnual == objGestion.id && w.registroActivo).ToList();
                            foreach (var item in lstGestionTabuladoresDet)
                            {
                                item.tabuladorDetAutorizado = EstatusGestionAutorizacionEnum.RECHAZADO;
                                item.registroAplicado = EstatusGestionAutorizacionEnum.RECHAZADO;
                                item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                item.fechaModificacion = DateTime.Now;
                            }
                            _context.SaveChanges();
                        }
                        #endregion

                        #region SE ELIMINA LA ALERTA AL USUARIO QUE RECHAZO
                        tblP_Alerta objAlerta = _context.tblP_Alerta.Where(w => w.obj == "AutorizarModificacion" && w.msj == "Modificación Pendiente de Autorizar" && w.objID == objParamDTO.id && w.userRecibeID == (int)vSesiones.sesionUsuarioDTO.id).FirstOrDefault();
                        if (objAlerta != null)
                        {
                            objAlerta.visto = true;
                            _context.SaveChanges();
                        }
                        #endregion

                        #region CORREO ESTATUS
                        if (objParamDTO.modificacionAutorizada == EstatusGestionAutorizacionEnum.AUTORIZADO)
                        {
                            lstCorreosNotificarRestantes.Add(_CORREO_DIANA_ALVAREZ);
                            lstCorreosNotificarRestantes.Add(_CORREO_MANUEL_CRUZ);
#if DEBUG
                            lstCorreosNotificarRestantes = new List<string>();
                            lstCorreosNotificarRestantes.Add(_CORREO_OMAR_NUNEZ);
                            //lstCorreosNotificarRestantes.Add("miguel.buzani@construplan.com.mx");
#endif

                            List<byte[]> downloadPDFs = new List<byte[]>();
                            using (var streamReader = new MemoryStream())
                            {
                                stream.CopyTo(streamReader);
                                downloadPDFs.Add(streamReader.ToArray());

                                GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: MODIFICACIÓN TABULADORES AUTORIZADA FECHA: {1}", PersonalUtilities.GetNombreEmpresa(), objGestion.fechaCreacion.ToString("dd/MM/yyyy")),
                                    cuerpo, lstCorreosNotificarRestantes, downloadPDFs, "Modificacion.pdf");

                            }
                        }
                        #endregion
                    }

                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, objParamDTO.modificacionAutorizada == EstatusGestionAutorizacionEnum.AUTORIZADO ? "Se ha autorizado con éxito." : "Se ha rechazado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objParamDTO.id, JsonUtils.convertNetObjectToJson(objParamDTO));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "AutorizarRechazarGestionModificacion", e, AccionEnum.ACTUALIZAR, objParamDTO.id, objParamDTO);
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetLstAutorizantesModificacion(int idModificacion)
        {
            resultado.Clear();

            using (var _ctx = new MainContext())
            {
                try
                {
                    var lstAutorizantes = _ctx.tblRH_TAB_GestionAutorizantes.Where(e => e.registroActivo && e.vistaAutorizacion == VistaAutorizacionEnum.MODIFICACION_TABULADORES && e.FK_Registro == idModificacion).ToList().Select(e => new GestionAutorizanteDTO()
                    {
                        id = e.id,
                        FK_Registro = e.FK_Registro,
                        vistaAutorizacion = e.vistaAutorizacion,
                        nivelAutorizante = e.nivelAutorizante,
                        FK_UsuarioAutorizacion = e.FK_UsuarioAutorizacion,
                        autorizado = e.autorizado,
                        comentario = e.comentario,
                        FK_UsuarioCreacion = e.FK_UsuarioCreacion,
                        FK_UsuarioModificacion = e.FK_UsuarioModificacion,
                        fechaCreacion = e.fechaCreacion,
                        fechaModificacion = e.fechaModificacion,
                    }).OrderBy(e => e.nivelAutorizante).ToList();

                    foreach (var item in lstAutorizantes)
                    {
                        var objUsuario = _ctx.tblP_Usuario.FirstOrDefault(e => e.id == item.FK_UsuarioAutorizacion);

                        item.nombreAutorizante = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                    }

                    resultado.Clear();
                    resultado.Add(ITEMS, lstAutorizantes);
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    resultado.Clear();
                    resultado.Add(MESSAGE, e);
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarComentarioRechazoModificacion(int idModificacion, string comentario)
        {
            resultado.Clear();

            using (var _ctx = new MainContext())
            {
                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var objModificacion = _ctx.tblRH_TAB_GestionModificacionTabulador.FirstOrDefault(e => e.id == idModificacion);
                        objModificacion.comentarioRechazo = comentario;

                        var lstCorreosNotificarTodos = new List<string>();
                        var objTipoMod = _context.tblRH_TAB_CatTipoModificacion.FirstOrDefault(e => e.id == (int)objModificacion.tipoModificacion);
                        List<tblRH_TAB_GestionAutorizantes> lstFirmas = _context.tblRH_TAB_GestionAutorizantes.Where(w => w.registroActivo && w.vistaAutorizacion == VistaAutorizacionEnum.MODIFICACION_TABULADORES && w.FK_Registro == idModificacion).ToList();

                        #region CUERPO CORREO

                        #region REPORTE
                        string RutaServidor = "";

#if DEBUG
                        RutaServidor = @"C:\Proyectos\SIGOPLANv2\REPORTESCR\CAPITAL_HUMANO";
#else
                        RutaServidor = @"\\10.1.0.112\Proyecto\SIGOPLAN\REPORTESCR\CAPITAL_HUMANO";
#endif
                        string strModLineasNegocio = string.Empty;
                        string lstLineasNegocio = string.Empty;
                        ReportDocument rptCV = new ReportDocument();
                        if (objModificacion.tipoModificacion == TipoModificacionEnum.MODIFICACION_A_PUESTOS)
                        {
                            rptCV = new rptRepTabuladoresModificacion();

                            //string path = Path.Combine(RutaServidor, "rptRepTabuladoresModificacion.rpt");
                            //rptCV.Load(path);

                            //var objParamsDTO = Session["objParamsPDF"] as TabuladorDTO;
                            TabuladorDTO objParamsDTO = new TabuladorDTO();
                            objParamsDTO.FK_IncrementoAnual = idModificacion;

                            var dictRepTabuladores = GetTabuladoresModificacionReportePDF((objParamsDTO));
                            List<RepTabuladoresModificacionDTO> lstTabuladores = dictRepTabuladores["lstTabPuestos"] as List<RepTabuladoresModificacionDTO>;

                            List<string> lstDescLN = dictRepTabuladores["lstLineasDeNegocios"] as List<string>;

                            strModLineasNegocio = lstDescLN != null && lstDescLN.Count() > 0 ? (string.Join(", ", lstDescLN)) : "";

                            //LIMPIAR DICCIONARIO
                            resultado.Clear();

                            objParamsDTO.lstDescLineaNegocio = new List<string>();
                            objParamsDTO.lstDescLineaNegocio.Add("LN");

                            objParamsDTO.lstDescCC = new List<string>();
                            objParamsDTO.lstDescCC.Add("DESC");

                            objParamsDTO.lstReporteAutorizantesDTO = new List<RepAutorizantesTABDTO>();
                            RepAutorizantesTABDTO obj = new RepAutorizantesTABDTO();
                            objParamsDTO.lstReporteAutorizantesDTO.Add(obj);

                            lstLineasNegocio = string.Join(", ", objParamsDTO.FK_LineaNegocio);
                            string lstCC = string.Join(", ", objParamsDTO.lstDescCC);

                            rptCV.Database.Tables[0].SetDataSource(getInfoEnca("Reporte de Modificacion de Tabuladores", "Dirección de Capital Humano"));
                            rptCV.Database.Tables[1].SetDataSource(lstTabuladores);
                            rptCV.Database.Tables[2].SetDataSource(objParamsDTO.lstReporteAutorizantesDTO);

                            rptCV.SetParameterValue("lstLineasNegocio", lstLineasNegocio);
                            rptCV.SetParameterValue("lstCC", lstCC);
                            rptCV.SetParameterValue("año", objParamsDTO.añoReporte);
                            rptCV.SetParameterValue("fecha", DateTime.Now.ToString("dd/MM/yyyy h:mm tt"));
                        }
                        else
                        {
                            rptCV = new rptRepTabuladoresModificacionEmpleados();

                            //string path = Path.Combine(RutaServidor, "rptRepTabuladoresModificacionEmpleados.rpt");
                            //rptCV.Load(path);

                            //var objParamsDTO = Session["objParamsPDF"] as TabuladorDTO;
                            TabuladorDTO objParamsDTO = new TabuladorDTO();
                            objParamsDTO.FK_IncrementoAnual = idModificacion;

                            var dictRepTabuladores = GetTabuladoresModificacionReportePDF((objParamsDTO));
                            List<RepTabuladoresModificacionDTO> lstTabuladores = dictRepTabuladores["lstTabPuestos"] as List<RepTabuladoresModificacionDTO>;

                            List<string> lstDescLN = dictRepTabuladores["lstLineasDeNegocios"] as List<string>;

                            strModLineasNegocio = lstDescLN != null && lstDescLN.Count() > 0 ? (string.Join(", ", lstDescLN)) : "";

                            //LIMPIAR DICCIONARIO
                            resultado.Clear();

                            objParamsDTO.lstDescLineaNegocio = new List<string>();
                            objParamsDTO.lstDescLineaNegocio.Add("LN");

                            objParamsDTO.lstDescCC = new List<string>();
                            objParamsDTO.lstDescCC.Add("DESC");

                            objParamsDTO.lstReporteAutorizantesDTO = new List<RepAutorizantesTABDTO>();
                            RepAutorizantesTABDTO obj = new RepAutorizantesTABDTO();
                            objParamsDTO.lstReporteAutorizantesDTO.Add(obj);

                            lstLineasNegocio = string.Join(", ", objParamsDTO.FK_LineaNegocio);
                            string lstCC = string.Join(", ", objParamsDTO.lstDescCC);

                            rptCV.Database.Tables[0].SetDataSource(getInfoEnca("Reporte de Modificacion de Tabuladores", "Dirección de Capital Humano"));
                            rptCV.Database.Tables[1].SetDataSource(lstTabuladores);
                            rptCV.Database.Tables[2].SetDataSource(objParamsDTO.lstReporteAutorizantesDTO);

                            rptCV.SetParameterValue("lstLineasNegocio", lstLineasNegocio);
                            rptCV.SetParameterValue("lstCC", lstCC);
                            rptCV.SetParameterValue("año", objParamsDTO.añoReporte);
                            rptCV.SetParameterValue("fecha", DateTime.Now.ToString("dd/MM/yyyy h:mm tt"));
                        }
                        Stream stream = rptCV.ExportToStream(ExportFormatType.PortableDocFormat);
                        #endregion

                        string cuerpo =
                                    @"<html>
                                            <head>
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
                                                        background-color: #ffcc5c;
                                                    }
                                                </style>
                                            </head>
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>

                                                    <p class=MsoNormal>
                                                        Buen día,<br><br>Se ha rechazado un tabulador. Motivo : " + comentario + "." + @".<br><br>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                        "Modificacion: " + objTipoMod.concepto + @".<o:p></o:p>
                                                    </p>
                                                    <br><br><br>
                                            ";

                        #region TABLA AUTORIZANTES

                        cuerpo += @"
                                        <table>
                                            <thead>
                                                <tr>
                                                    <th>Nombre</th>
                                                    <th>Tipo</th>
                                                    <th>Autorizo</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            ";

                        bool esAuth = false;
                        //EstatusGestionAutorizacionEnum P 0 A 1 R 2 
                        foreach (var itemDet in lstFirmas)
                        {
                            tblP_Usuario objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == itemDet.FK_UsuarioAutorizacion);

                            if (vSesiones.sesionUsuarioDTO.id == itemDet.FK_UsuarioAutorizacion)
                            {
                                itemDet.autorizado = EstatusGestionAutorizacionEnum.RECHAZADO;
                                itemDet.fechaFirma = DateTime.Now;
                            }

                            string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                            cuerpo += "<tr>" +
                                        "<td>" + nombreCompleto + "</td>" +
                                        "<td>" + EnumHelper.GetDescription(itemDet.nivelAutorizante) + "</td>" +
                                        GetEstatusTabulador((int)itemDet.autorizado, false) +
                                    "</tr>";

                        }

                        cuerpo += "</tbody>" +
                                    "</table>" +
                                    "<br><br><br>";


                        #endregion

                        cuerpo += "<br><br><br>" +
                              "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                              "Capital Humano > Administración de personal > Configuración > Estructura Salarial > Gestión de tabuladores.<br><br>" +
                              "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                              "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias." +
                            " </body>" +
                          "</html>";

                        #endregion

                        lstCorreosNotificarTodos.Add(_CORREO_DIANA_ALVAREZ);
                        lstCorreosNotificarTodos.Add(_CORREO_MANUEL_CRUZ);
#if DEBUG
                        lstCorreosNotificarTodos = new List<string>();
                        lstCorreosNotificarTodos.Add(_CORREO_OMAR_NUNEZ);
#endif
                        List<byte[]> downloadPDFs = new List<byte[]>();
                        using (var streamReader = new MemoryStream())
                        {
                            stream.CopyTo(streamReader);
                            downloadPDFs.Add(streamReader.ToArray());

                            GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: MODIFICACIÓN TABULADORES AUTORIZADO POR TODOS LOS FIRMANTES FECHA: {1}", PersonalUtilities.GetNombreEmpresa(), objModificacion.fechaCreacion.ToString("dd/MM/yyyy")),
                                cuerpo, lstCorreosNotificarTodos, downloadPDFs, "Modificacion.pdf");
                        }

                        _ctx.SaveChanges();
                        resultado.Clear();
                        resultado.Add(SUCCESS, true);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Clear();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, e.Message);
                    }
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetModificacionDetalle(GestionModificacionTabuladorDetDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region VALIDACIONES
                if (objParamDTO.id <= 0) { throw new Exception("Ocurrió un error al obtener el detalle del tabulador."); }
                #endregion

                #region CATALOGOS
                List<tblRH_EK_Puestos> lstPuestos = _context.tblRH_EK_Puestos.Where(w => w.FK_AreaDepartamento > 0 && w.FK_NivelMando > 0 && w.FK_Sindicato > 0 && w.FK_TipoNomina > 0 && w.registroActivo).ToList();
                List<tblRH_EK_Tipos_Nomina> lstTipoNominas = _context.tblRH_EK_Tipos_Nomina.Where(w => w.esActivo == true).ToList();
                List<tblRH_TAB_CatAreaDepartamento> lstAreasDepartamentos = _context.tblRH_TAB_CatAreaDepartamento.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatCategorias> lstCategorias = _context.tblRH_TAB_CatCategorias.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatEsquemaPago> lstEsquemasPagos = _context.tblRH_TAB_CatEsquemaPago.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_PlantillasPersonal> lstPlantillas = _context.tblRH_TAB_PlantillasPersonal.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_PlantillasPersonalDet> lstPlantillasDet = _context.tblRH_TAB_PlantillasPersonalDet.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatLineaNegocio> lstCatLineasNegocios = _context.tblRH_TAB_CatLineaNegocio.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_TabuladoresHistorial> lstTabHistorial = _context.tblRH_TAB_TabuladoresHistorial.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_TabuladoresDetHistorial> lstTabDetHistorial = _context.tblRH_TAB_TabuladoresDetHistorial.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_Tabuladores> lstTabuladores = _context.tblRH_TAB_Tabuladores.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.registroActivo).ToList();
                List<tblRH_EK_Empleados> lstEmpleados = _context.tblRH_EK_Empleados.Where(w => w.esActivo).ToList();
                #endregion

                if (objParamDTO.tipoModificacion == TipoModificacionEnum.MODIFICACION_A_PUESTOS)
                {
                    #region SE OBTIENE DETALLE DE LA MODIFICACIÓN SELECCIONADA

                    #region SE OBTIENE ENCABEZADO DE LA MODIFICACIÓN Y SU DETALLE
                    tblRH_TAB_GestionModificacionTabulador objGestionModificacion = _context.tblRH_TAB_GestionModificacionTabulador.Where(w => w.id == objParamDTO.id && w.registroActivo).FirstOrDefault();
                    if (objGestionModificacion == null)
                        throw new Exception("Ocurrió un error al obtener el detalle de la modificación.");

                    List<tblRH_TAB_GestionModificacionTabuladorDet> lstGestionModificacionDet = _context.tblRH_TAB_GestionModificacionTabuladorDet.Where(w => w.FK_IncrementoAnual == objParamDTO.id && w.registroActivo).OrderBy(o => o.FK_Puesto).ToList();
                    if (lstGestionModificacionDet.Count() <= 0)
                        throw new Exception("Ocurrió un error al obtener el detalle de la modificación.");

                    var lstPuestosGroup = lstGestionModificacionDet.GroupBy(o => o.FK_Puesto).ToList();
                    List<int> lstPuestosFK = lstPuestosGroup.Select(s => s.Key).ToList();
                    #endregion

                    List<GestionModificacionTabuladorDetDTO> lstGestionDTO = new List<GestionModificacionTabuladorDetDTO>();
                    GestionModificacionTabuladorDetDTO objGestionDTO = new GestionModificacionTabuladorDetDTO();
                    foreach (var FK_Puesto in lstPuestosFK)
                    {
                        #region SE OBTIENE INFORMACIÓN DEL PUESTO
                        tblRH_EK_Puestos objPuesto = lstPuestos.Where(w => w.puesto == FK_Puesto).FirstOrDefault();
                        if (objPuesto == null)
                            throw new Exception("Ocurrió un error al obtener la información del puesto.");

                        string puestoDesc = !string.IsNullOrEmpty(objPuesto.descripcion) ? objPuesto.descripcion.Trim() : string.Empty;
                        if (string.IsNullOrEmpty(puestoDesc))
                            throw new Exception("Ocurrió un error al obtener el nombre del puesto.");

                        tblRH_EK_Tipos_Nomina objTipoNomina = lstTipoNominas.Where(w => w.tipo_nomina == objPuesto.FK_TipoNomina).FirstOrDefault();
                        if (objTipoNomina == null)
                            throw new Exception("Ocurrió un error al obtener el tipo de nómina.");

                        string tipoNominaDesc = !string.IsNullOrEmpty(objTipoNomina.descripcion) ? objTipoNomina.descripcion.Trim() : string.Empty;
                        if (string.IsNullOrEmpty(tipoNominaDesc))
                            throw new Exception("Ocurrió un error al obtener el tipo de nómina.");

                        objGestionDTO = new GestionModificacionTabuladorDetDTO();
                        objGestionDTO.FK_Puesto = FK_Puesto;
                        objGestionDTO.puestoDesc = string.Format("[{0}] {1}", FK_Puesto, puestoDesc);
                        objGestionDTO.puestoSoloTexto = puestoDesc;
                        objGestionDTO.tipoNominaDesc = tipoNominaDesc;
                        #endregion

                        List<tblRH_TAB_GestionModificacionTabuladorDet> lstGestionPuestoDet = lstGestionModificacionDet.Where(w => w.FK_Puesto == FK_Puesto).ToList();
                        foreach (var objGestionDet in lstGestionPuestoDet)
                        {
                            #region DETALLE
                            #region SE OBTIENE LA LINEA DE NEGOCIO
                            tblRH_TAB_CatLineaNegocio objLineaNegocio = lstCatLineasNegocios.Where(w => w.id == objGestionDet.FK_LineaNegocio).FirstOrDefault();
                            if (objLineaNegocio == null)
                                throw new Exception("Ocurrió un error al obtener la linea de negocio.");

                            string lineaNegocioTitle = objLineaNegocio.concepto.Trim();
                            string lineaNegocioDesc = objLineaNegocio.abreviacion.Trim();
                            #endregion

                            #region SE OBTIENE LA CATEGORIA DEL PUESTO
                            tblRH_TAB_CatCategorias objCategoria = lstCategorias.Where(w => w.id == objGestionDet.FK_Categoria).FirstOrDefault();
                            if (objCategoria == null)
                                throw new Exception("Ocurrió un error al obtener la categoría del puesto.");

                            string categoriaDesc = objCategoria.concepto.Trim();
                            #endregion

                            #region SE OBTIENE EL ESQUEMA DE PAGO
                            tblRH_TAB_CatEsquemaPago objEsquemaPago = lstEsquemasPagos.Where(w => w.id == objGestionDet.FK_EsquemaPago).FirstOrDefault();
                            if (objEsquemaPago == null)
                                throw new Exception("Ocurrió un error al obtener el esquema de pago del puesto.");
                            #endregion

                            #region SE INTEGRA EL DETALLE DE LA MODIFICACIÓN EN EL PUESTO
                            objGestionDTO.FK_LineaNegocio = objGestionDet.FK_LineaNegocio;
                            objGestionDTO.lineaNegocioDesc += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px; font-size: 11px;' title='{0}'>{1}</button><br>", lineaNegocioTitle, lineaNegocioDesc);
                            objGestionDTO.esquemaPagoDesc += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px; font-size: 11px;' title='{0}'>{0}</button><br>", objEsquemaPago.concepto);
                            objGestionDTO.FK_Categoria = objGestionDet.FK_Categoria;
                            objGestionDTO.categoriaDesc += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px; font-size: 11px;'>{0}</button><br>", categoriaDesc);

                            #region SUELDO MODIFICACIÓN
                            objGestionDTO.sueldoBase_Modificacion += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px; font-size: 11px;'>{0}</button><br>", objGestionDet.salario_base.ToString("C"));
                            objGestionDTO.complemento_Modificacion += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px; font-size: 11px;'>{0}</button><br>", objGestionDet.complemento.ToString("C"));
                            objGestionDTO.totalNominal_Modificacion += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px; font-size: 11px;'>{0}</button><br>", objGestionDet.suma.ToString("C"));
                            objGestionDTO.totalMensual_Modificacion += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px; font-size: 11px;'>{0}</button><br>", objGestionDet.totalMensual.ToString("C"));
                            #endregion

                            #region SUELDO ANTERIOR
                            // SE OBTIENE EL SUELDO ANTERIOR EN BASE A SU HISTORIAL DETALLE
                            tblRH_TAB_TabuladoresDetHistorial objTabuladorAnterior = lstTabDetHistorial.Where(w => w.FK_Tabulador == objGestionDet.FK_Tabulador && w.FK_LineaNegocio == objGestionDet.FK_LineaNegocio && w.FK_Categoria == objGestionDet.FK_Categoria && w.registroActivo).FirstOrDefault();
                            if (objTabuladorAnterior == null)
                            {
                                tblRH_TAB_Tabuladores objTabulador = lstTabuladores.Where(w => w.FK_Puesto == FK_Puesto && w.registroActivo).FirstOrDefault();
                                if (objTabulador == null)
                                    throw new Exception("Ocurrió un error al obtener la información del tabulador.");

                                tblRH_TAB_TabuladoresDet objTabuladorDet = lstTabuladoresDet.Where(w => w.FK_Tabulador == objTabulador.id && w.FK_LineaNegocio == objGestionDet.FK_LineaNegocio && w.FK_Categoria == objGestionDet.FK_Categoria && w.registroActivo).FirstOrDefault();
                                if (objTabuladorDet == null)
                                    throw new Exception("Ocurrió un error al obtener la información del tabulador.");

                                objTabuladorAnterior = new tblRH_TAB_TabuladoresDetHistorial();
                                objTabuladorAnterior.sueldoBase = objTabuladorDet.sueldoBase;
                                objTabuladorAnterior.complemento = objTabuladorDet.complemento;
                                objTabuladorAnterior.totalNominal = objTabuladorDet.totalNominal;
                                objTabuladorAnterior.sueldoMensual = objTabuladorDet.sueldoMensual;
                            }
                            objGestionDTO.sueldoBase_Anterior += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px; font-size: 11px;'>{0}</button><br>", objTabuladorAnterior.sueldoBase.ToString("C"));
                            objGestionDTO.complemento_Anterior += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px; font-size: 11px;'>{0}</button><br>", objTabuladorAnterior.complemento.ToString("C"));
                            objGestionDTO.totalNominal_Anterior += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px; font-size: 11px;'>{0}</button><br>", objTabuladorAnterior.totalNominal.ToString("C"));
                            objGestionDTO.totalMensual_Anterior += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px; font-size: 11px;'>{0}</button><br>", objTabuladorAnterior.sueldoMensual.ToString("C"));
                            #endregion

                            objGestionDTO.tipoIncremento += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px; font-size: 11px;'>{0}</button><br>", objGestionDet.porcentajeIncremento > 0 ? string.Format("{0}%", objGestionDet.porcentajeIncremento.ToString()) : "$");
                            #endregion
                            #endregion
                        }
                        lstGestionDTO.Add(objGestionDTO);
                    }

                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    if (objParamDTO.tipoModificacion == TipoModificacionEnum.INCREMENTO_ANUAL_A_EMPLEADOS_ACTIVOS)
                        resultado.Add("lstTabEmpleadosActivos", lstGestionDTO);
                    else
                        resultado.Add("lstTabPuestos", lstGestionDTO);
                    #endregion
                }
                else
                {
                    List<tblRH_TAB_GestionModificacionTabuladorDet> lstModificacionEmpleadoDet = _context.tblRH_TAB_GestionModificacionTabuladorDet.Where(w => w.FK_IncrementoAnual == objParamDTO.id && w.registroActivo).ToList();

                    List<GestionModificacionTabuladorDetDTO> lstGestionDTO = new List<GestionModificacionTabuladorDetDTO>();
                    GestionModificacionTabuladorDetDTO objGestionDTO = new GestionModificacionTabuladorDetDTO();
                    foreach (var item in lstModificacionEmpleadoDet)
                    {
                        #region SE OBTIENE INFORMACION GENERAL DEL PUESTO
                        tblRH_EK_Puestos objPuesto = lstPuestos.Where(w => w.puesto == item.FK_Puesto).FirstOrDefault();
                        if (objPuesto == null)
                            throw new Exception("Ocurrió un error al obtener la información del puesto.");

                        tblRH_EK_Tipos_Nomina objTipoNomina = lstTipoNominas.Where(w => w.tipo_nomina == objPuesto.FK_TipoNomina).FirstOrDefault();
                        if (objTipoNomina == null)
                            throw new Exception("Ocurrió un error al obtener el tipo de nómina del puesto.");

                        tblRH_TAB_CatLineaNegocio objCatLineaNegocio = lstCatLineasNegocios.Where(w => w.id == item.FK_LineaNegocio).FirstOrDefault();
                        if (objCatLineaNegocio == null)
                            throw new Exception("Ocurrió un error al obtener la línea de negocio.");

                        tblRH_TAB_CatCategorias objCategoria = lstCategorias.Where(w => w.id == item.FK_Categoria).FirstOrDefault();
                        if (objCategoria == null)
                            throw new Exception("Ocurrió un error al obtener la categoría.");
                        #endregion

                        tblRH_EK_Empleados objEmpleado = lstEmpleados.Where(w => w.clave_empleado == item.clave_empleado).FirstOrDefault();
                        if (objEmpleado == null)
                            throw new Exception("Ocurrió un error al obtener la información del empleado.");

                        objGestionDTO = new GestionModificacionTabuladorDetDTO();
                        objGestionDTO.nombreEmpleado = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(string.Format("{0} {1} {2}", objEmpleado.nombre, objEmpleado.ape_paterno, objEmpleado.ape_materno));
                        objGestionDTO.puestoDesc = SetPuestoEmpleado(lstPuestos.Where(s => s.puesto == item.FK_Puesto).FirstOrDefault());
                        objGestionDTO.tipoNominaDesc = objTipoNomina != null ? !string.IsNullOrEmpty(objTipoNomina.descripcion) ? string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px; font-size: 11px;' title='{0}'>{0}</button><br>", objTipoNomina.descripcion) : null : null;
                        objGestionDTO.lineaNegocioDesc = objCatLineaNegocio != null ? !string.IsNullOrEmpty(objCatLineaNegocio.abreviacion) ? string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px; font-size: 11px;' title='{0}'>{1}</button><br>", objCatLineaNegocio.concepto, objCatLineaNegocio.abreviacion) : null : null;
                        objGestionDTO.categoriaDesc = objCategoria != null ? !string.IsNullOrEmpty(objCategoria.concepto) ? string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px; font-size: 11px;' title='{0}'>{0}</button><br>", objCategoria.concepto) : null : null;

                        if (objParamDTO.modificacionAutorizada == EstatusGestionAutorizacionEnum.PENDIENTE)
                        {
                            tblRH_EK_Tabulador_Historial objPagoActualEmpleado = _context.tblRH_EK_Tabulador_Historial.Where(w => w.clave_empleado == item.clave_empleado && w.esActivo).OrderByDescending(o => o.id).FirstOrDefault();
                            if (objPagoActualEmpleado == null)
                                throw new Exception("Ocurrió un error al obtener la información del empleado.");

                            // ACTUAL ANTES DE MODIFICACIÓN
                            objGestionDTO.sueldoBase_Anterior = objPagoActualEmpleado.salario_base.ToString("C");
                            objGestionDTO.complemento_Anterior = objPagoActualEmpleado.complemento.ToString("C");
                            objGestionDTO.totalNominal_Anterior = objPagoActualEmpleado.suma.ToString("C");

                            if (objPuesto.FK_TipoNomina == (int)TipoNominaEnum.quincenal)
                                objGestionDTO.totalMensual_Anterior = (objPagoActualEmpleado.suma * 2).ToString("C");
                            else if (objPuesto.FK_TipoNomina == (int)TipoNominaEnum.semanal)
                                objGestionDTO.totalMensual_Anterior = ((objPagoActualEmpleado.suma / 7) * (decimal)30.4).ToString("C");
                            // END: ACTUAL ANTES DE MODIFICACIÓN

                            // MODIFICACIÓN ANTES DE AUTORIZACIÓN
                            objGestionDTO.sueldoBase_Modificacion = item.salario_base.ToString("C");
                            objGestionDTO.complemento_Modificacion = item.complemento.ToString("C");
                            objGestionDTO.totalNominal_Modificacion = item.suma.ToString("C");

                            if (objPuesto.FK_TipoNomina == (int)TipoNominaEnum.quincenal)
                                objGestionDTO.totalMensual_Modificacion = (item.suma * 2).ToString("C");
                            else if (objPuesto.FK_TipoNomina == (int)TipoNominaEnum.semanal)
                                objGestionDTO.totalMensual_Modificacion = ((item.suma / 7) * (decimal)30.4).ToString("C");
                            // END: MODIFICACIÓN ANTES DE AUTORIZACIÓN
                        }
                        else if (objParamDTO.modificacionAutorizada == EstatusGestionAutorizacionEnum.AUTORIZADO)
                        {
                            int CERO = 0;

                            // HISTORIAL DE PAGOS DEL EMPLEADO
                            List<tblRH_EK_Tabulador_Historial> lstHistorialPagosEmpleado = _context.tblRH_EK_Tabulador_Historial.Where(w => w.clave_empleado == item.clave_empleado && w.esActivo).OrderByDescending(o => o.id).ToList();

                            // SE OBTIENE EL PAGO ACTUAL DEL EMPLEADO
                            tblRH_EK_Tabulador_Historial objPagoActualEmpleado = lstHistorialPagosEmpleado.FirstOrDefault();
                            if (objPagoActualEmpleado == null)
                                throw new Exception("Ocurrió un error al obtener la información del empleado.");

                            // SE OBTIENE EL PAGO ANTERIOR DEL EMPLEADO
                            tblRH_EK_Tabulador_Historial objPagoAnteriorEmpleado = new tblRH_EK_Tabulador_Historial();
                            if (lstHistorialPagosEmpleado.Count() > 1)
                                objPagoAnteriorEmpleado = lstHistorialPagosEmpleado[1];

                            // ACTUAL
                            objGestionDTO.sueldoBase_Anterior = objPagoActualEmpleado.salario_base.ToString("C");
                            objGestionDTO.complemento_Anterior = objPagoActualEmpleado.complemento.ToString("C");
                            objGestionDTO.totalNominal_Anterior = objPagoActualEmpleado.suma.ToString("C");

                            if (objPuesto.FK_TipoNomina == (int)TipoNominaEnum.quincenal)
                                objGestionDTO.totalMensual_Anterior = (objPagoActualEmpleado.suma * 2).ToString("C");
                            else if (objPuesto.FK_TipoNomina == (int)TipoNominaEnum.semanal)
                                objGestionDTO.totalMensual_Anterior = ((objPagoActualEmpleado.suma / 7) * (decimal)30.4).ToString("C");
                            // END: ACTUAL

                            // ANTERIOR
                            objGestionDTO.sueldoBase_Modificacion = objPagoAnteriorEmpleado.clave_empleado > 0 ? objPagoAnteriorEmpleado.salario_base.ToString("C") : CERO.ToString("C");
                            objGestionDTO.complemento_Modificacion = objPagoAnteriorEmpleado.clave_empleado > 0 ? objPagoAnteriorEmpleado.complemento.ToString("C") : CERO.ToString("C");
                            objGestionDTO.totalNominal_Modificacion = objPagoAnteriorEmpleado.clave_empleado > 0 ? objPagoAnteriorEmpleado.suma.ToString("C") : CERO.ToString("C");

                            if (objPuesto.FK_TipoNomina == (int)TipoNominaEnum.quincenal)
                                objGestionDTO.totalMensual_Modificacion = (objPagoAnteriorEmpleado.suma * 2).ToString("C");
                            else if (objPuesto.FK_TipoNomina == (int)TipoNominaEnum.semanal)
                                objGestionDTO.totalMensual_Modificacion = ((objPagoAnteriorEmpleado.suma / 7) * (decimal)30.4).ToString("C");
                            // END: ANTERIOR
                        }
                        objGestionDTO.tipoIncremento = string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px; font-size: 11px;'>{0}</button><br>", item.porcentajeIncremento > 0 ? string.Format("{0}%", item.porcentajeIncremento.ToString()) : "$");
                        lstGestionDTO.Add(objGestionDTO);
                    }
                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add("lstTabEmpleadosActivos", lstGestionDTO);
                }
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetModificacionDetalle", e, AccionEnum.CONSULTA, objParamDTO.id, objParamDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> GetTabuladoresModificacionReportePDF(TabuladorDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region CATALOGOS
                List<tblRH_EK_Tipos_Nomina> lstTipoNominas = _context.tblRH_EK_Tipos_Nomina.Where(w => w.esActivo == true).ToList();
                List<tblRH_TAB_CatAreaDepartamento> lstAreasDepartamentos = _context.tblRH_TAB_CatAreaDepartamento.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatCategorias> lstCategorias = _context.tblRH_TAB_CatCategorias.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatEsquemaPago> lstEsquemaPagos = _context.tblRH_TAB_CatEsquemaPago.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_Tabuladores> lstTabuladores = _context.tblRH_TAB_Tabuladores.Where(w => w.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo).ToList();
                List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.registroActivo && w.tabuladorDetAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO).ToList();
                List<tblRH_TAB_CatLineaNegocio> lstLineaNegocios = _context.tblRH_TAB_CatLineaNegocio.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatLineaNegocioDet> lstLineaNegociosDet = _context.tblRH_TAB_CatLineaNegocioDet.Where(w => w.registroActivo).ToList();
                List<tblRH_EK_Puestos> lstPuestos = _context.tblRH_EK_Puestos.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatSindicato> lstSindicatos = _context.tblRH_TAB_CatSindicato.Where(e => e.registroActivo).ToList();
                List<tblRH_TAB_TabuladoresHistorial> lstTabuladoresHistorial = _context.tblRH_TAB_TabuladoresHistorial.Where(w => w.FK_Tabulador > 0 && w.registroActivo).ToList();
                List<tblRH_TAB_TabuladoresDetHistorial> lstTabuladoresDetHistorial = _context.tblRH_TAB_TabuladoresDetHistorial.Where(w => w.FK_Tabulador > 0 && w.registroActivo).ToList();
                #endregion

                //HEADERS LN UNICOS
                var lstDescLineaNegociosU = new List<tblRH_TAB_CatLineaNegocio>();

                // SE VERIFICA EL TIPO DE MODIFICACIÓN
                TipoModificacionEnum objTipoModificacion = _context.tblRH_TAB_GestionModificacionTabulador.Where(w => w.id == objParamDTO.FK_IncrementoAnual && w.registroActivo).Select(s => s.tipoModificacion).FirstOrDefault();
                if (objTipoModificacion == TipoModificacionEnum.MODIFICACION_A_PUESTOS)
                {
                    #region SE OBTIENE LISTADO DE TABULADORES DE PUESTOS
                    List<tblRH_TAB_GestionModificacionTabuladorDet> lstGestionModificacionDet = _context.tblRH_TAB_GestionModificacionTabuladorDet.Where(w => w.FK_IncrementoAnual == objParamDTO.FK_IncrementoAnual && w.registroActivo).ToList();
                    if (lstGestionModificacionDet.Count() > 0)
                        objParamDTO.lstFK_LineaNegocio = lstGestionModificacionDet.Select(s => s.FK_LineaNegocio).ToList();

                    List<int> lstFK_PuestosGestionModificacion = lstGestionModificacionDet.Where(w => w.FK_IncrementoAnual == objParamDTO.FK_IncrementoAnual && w.registroActivo).Select(s => s.FK_Puesto).ToList();
                    lstPuestos = lstPuestos.Where(w => lstFK_PuestosGestionModificacion.Contains(w.puesto)).ToList();

                    List<int> lstPuestosID = lstPuestos.Select(s => s.puesto).ToList();
                    lstTabuladores = lstTabuladores.Where(w => lstPuestosID.Contains(w.FK_Puesto)).ToList();
                    List<int> lstTabuladoresID = lstTabuladores.Select(s => s.id).ToList();
                    lstTabuladoresDet = lstTabuladoresDet.Where(w => lstTabuladoresID.Contains(w.FK_Tabulador)).ToList();
                    if (objParamDTO.lstFK_LineaNegocio != null)
                    {
                        if (objParamDTO.lstFK_LineaNegocio.Count() > 0)
                            lstTabuladoresDet = lstTabuladoresDet.Where(w => objParamDTO.lstFK_LineaNegocio.Contains(w.FK_LineaNegocio)).ToList();
                    }

                    List<RepTabuladoresModificacionDTO> lstTabuladoresDTO = new List<RepTabuladoresModificacionDTO>();
                    RepTabuladoresModificacionDTO objNewTabuladorDTO = new RepTabuladoresModificacionDTO();
                    foreach (var itemTabulador in lstTabuladores)
                    {
                        tblRH_TAB_Tabuladores objTabulador = lstTabuladores.Where(w => w.id == itemTabulador.id).FirstOrDefault();
                        if (objTabulador != null)
                        {
                            tblRH_EK_Puestos objPuesto = lstPuestos.Where(w => w.puesto == itemTabulador.FK_Puesto).FirstOrDefault();
                            if (objPuesto == null)
                                throw new Exception("Ocurrió un error al obtener la información del puesto.");

                            foreach (var itemTabuladorDet in lstTabuladoresDet.Where(w => w.FK_Tabulador == itemTabulador.id).ToList())
                            {
                                objNewTabuladorDTO = new RepTabuladoresModificacionDTO();

                                tblRH_TAB_CatLineaNegocio objLineaNegocio = lstLineaNegocios.Where(w => w.id == itemTabuladorDet.FK_LineaNegocio).FirstOrDefault();

                                if (!lstDescLineaNegociosU.Contains(objLineaNegocio))
                                {
                                    lstDescLineaNegociosU.Add(objLineaNegocio);
                                }

                                tblRH_EK_Tipos_Nomina objTipoNomina = lstTipoNominas.Where(w => w.tipo_nomina == objPuesto.FK_TipoNomina).FirstOrDefault();
                                if (objTipoNomina == null)
                                    throw new Exception("Ocurrió un error al obtener el tipo de nómina del puesto.");

                                tblRH_TAB_CatAreaDepartamento objAreaDepartamento = lstAreasDepartamentos.Where(w => w.id == objPuesto.FK_AreaDepartamento).FirstOrDefault();

                                tblRH_TAB_CatCategorias objCategoria = lstCategorias.Where(w => w.id == itemTabuladorDet.FK_Categoria).FirstOrDefault();
                                if (objCategoria == null)
                                    throw new Exception("Ocurrió un error al obtener la categoría del puesto.");

                                tblRH_TAB_CatEsquemaPago objEsquemaPago = lstEsquemaPagos.Where(w => w.id == itemTabuladorDet.FK_EsquemaPago).FirstOrDefault();
                                if (objEsquemaPago == null)
                                    throw new Exception("Ocurrió un error al obtener el esquema de pago del puesto.");

                                tblRH_TAB_CatSindicato objSindicato = lstSindicatos.FirstOrDefault(e => e.id == objPuesto.FK_Sindicato);

                                objNewTabuladorDTO.id = objTabulador.FK_Puesto.ToString();
                                objNewTabuladorDTO.puesto = objPuesto.descripcion;
                                objNewTabuladorDTO.lineaNegocios = objLineaNegocio.abreviacion;
                                objNewTabuladorDTO.categoria = objCategoria.concepto;
                                objNewTabuladorDTO.esquemaPagoDesc = objEsquemaPago.concepto;

                                // SALARIO ACTUAL
                                tblRH_TAB_GestionModificacionTabuladorDet objGestionModificacionTabDet = _context.tblRH_TAB_GestionModificacionTabuladorDet
                                    .Where(w => w.FK_Puesto == itemTabulador.FK_Puesto && w.FK_Tabulador == itemTabuladorDet.FK_Tabulador && w.registroActivo).OrderByDescending(o => o.id).FirstOrDefault();

                                objNewTabuladorDTO.sueldoBase_Actual = objGestionModificacionTabDet.salario_base.ToString("C");
                                objNewTabuladorDTO.complemento_Actual = objGestionModificacionTabDet.complemento.ToString("C");
                                objNewTabuladorDTO.totalNominal_Actual = objGestionModificacionTabDet.suma.ToString("C");
                                objNewTabuladorDTO.totalMensual_Actual = objGestionModificacionTabDet.totalMensual.ToString("C");
                                // END: SALARIO ACTUAL

                                // SUELDO ANTERIOR
                                tblRH_TAB_TabuladoresDetHistorial objTabuladorDetHistorial = lstTabuladoresDetHistorial.Where(w => w.FK_Tabulador == objTabulador.id && w.FK_Categoria == objCategoria.id && w.FK_LineaNegocio == objLineaNegocio.id).FirstOrDefault();
                                objNewTabuladorDTO.sueldoBase_Anterior = objTabuladorDetHistorial != null ? objTabuladorDetHistorial.sueldoBase.ToString("C") : itemTabuladorDet.sueldoBase.ToString("C");
                                objNewTabuladorDTO.complemento_Anterior = objTabuladorDetHistorial != null ? objTabuladorDetHistorial.complemento.ToString("C") : itemTabuladorDet.complemento.ToString("C");
                                objNewTabuladorDTO.totalNominal_Anterior = objTabuladorDetHistorial != null ? objTabuladorDetHistorial.totalNominal.ToString("C") : itemTabuladorDet.totalNominal.ToString("C");
                                objNewTabuladorDTO.totalMensual_Anterior = objTabuladorDetHistorial != null ? objTabuladorDetHistorial.sueldoMensual.ToString("C") : itemTabuladorDet.sueldoMensual.ToString("C");
                                // END: SUELDO ANTERIOR

                                objNewTabuladorDTO.nomina = objTipoNomina.descripcion;
                                objNewTabuladorDTO.personal = " ";
                                objNewTabuladorDTO.descAreaDepartamento = objAreaDepartamento.concepto;
                                objNewTabuladorDTO.descSindicato = objSindicato != null && objSindicato.id == 1 ? "S" : "N";

                                lstTabuladoresDTO.Add(objNewTabuladorDTO);
                            }
                        }
                    }

                    lstTabuladoresDTO = lstTabuladoresDTO.OrderBy(e => e.puesto).ToList();
                    List<string> lstConceptosLN = lstDescLineaNegociosU.Select(e => e.concepto).ToList();

                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add("lstTabPuestos", lstTabuladoresDTO);
                    resultado.Add("lstLineasDeNegocios", lstConceptosLN);

                    #endregion
                }
                else
                {
                    #region REPORTE SALARIO DE LA MODIFICACIÓN PARA EMPLEADOS ACTIVOS
                    List<tblRH_TAB_GestionModificacionTabuladorDet> lstGestiones = _context.tblRH_TAB_GestionModificacionTabuladorDet.Where(w => w.FK_IncrementoAnual == objParamDTO.FK_IncrementoAnual && w.registroActivo).ToList();

                    if (lstGestiones.Count() <= 0)
                        throw new Exception("Ocurrió un error al obtener la información del reporte.");

                    if (objParamDTO.lstFK_LineaNegocio != null && objParamDTO.lstFK_LineaNegocio.Count() > 0)
                    {
                        var objLineaNegocio = _context.tblRH_TAB_CatLineaNegocio.Where(w => objParamDTO.lstFK_LineaNegocio.Contains(w.id) && w.registroActivo).ToList();

                        foreach (var item in objLineaNegocio)
                        {
                            lstDescLineaNegociosU.Add(item);
                        }
                    }

                    List<RepTabuladoresModificacionDTO> lstReporteDTO = new List<RepTabuladoresModificacionDTO>();
                    foreach (var objGestion in lstGestiones)
                    {
                        #region SE OBTIENE LA INFORMACIÓN DEL PUESTO
                        tblRH_EK_Puestos objPuesto = _context.tblRH_EK_Puestos.Where(w => w.puesto == objGestion.FK_Puesto && w.registroActivo).FirstOrDefault();
                        if (objPuesto == null)
                            throw new Exception("Ocurrió un error al obtener la información del puesto.");

                        tblRH_TAB_CatLineaNegocio objLineaNegocio = lstLineaNegocios.Where(w => w.id == objGestion.FK_LineaNegocio).FirstOrDefault();

                        tblRH_TAB_CatCategorias objCategoria = _context.tblRH_TAB_CatCategorias.Where(w => w.id == objGestion.FK_Categoria && w.registroActivo).FirstOrDefault();
                        #endregion

                        RepTabuladoresModificacionDTO objReporteDTO = new RepTabuladoresModificacionDTO();
                        objReporteDTO.id = objGestion.FK_Puesto.ToString();
                        tblRH_EK_Empleados objEmpleado = GetNombreEmpleado(objGestion.clave_empleado);
                        objReporteDTO.nombreEmpleado = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(string.Format("{0} {1} {2}", objEmpleado.nombre, objEmpleado.ape_paterno, objEmpleado.ape_materno));
                        objReporteDTO.puesto = objPuesto.descripcion;
                        objReporteDTO.lineaNegocios = objLineaNegocio != null ? objLineaNegocio.abreviacion : " ";
                        objReporteDTO.categoria = objCategoria != null ? objCategoria.concepto : " ";

                        // SE OBTIENE LISTADO DE HISTORIAL DE PAGO DEL EMPLEADO
                        List<tblRH_EK_Tabulador_Historial> lstHistorialEmpleado = _context.tblRH_EK_Tabulador_Historial.Where(w => w.clave_empleado == objGestion.clave_empleado && w.esActivo).OrderByDescending(o => o.id).ToList();

                        if (lstHistorialEmpleado.Count() > 0)
                        {
                            // SUELDO ACTUAL DEL EMPLEADO
                            tblRH_EK_Tabulador_Historial objPagoActual = lstHistorialEmpleado.FirstOrDefault();
                            objReporteDTO.sueldoBase_Anterior = objPagoActual.salario_base.ToString("C");
                            objReporteDTO.complemento_Anterior = objPagoActual.complemento.ToString("C");
                            objReporteDTO.totalNominal_Anterior = objPagoActual.suma.ToString("C");

                            decimal sueldoMensual = 0;
                            if (objPuesto.FK_TipoNomina == (int)TipoNominaEnum.quincenal)
                                sueldoMensual = (decimal)objPagoActual.suma * 2;
                            else if (objPuesto.FK_TipoNomina == (int)TipoNominaEnum.semanal)
                                sueldoMensual = ((decimal)objPagoActual.suma / 7) * (decimal)30.4;

                            objReporteDTO.totalMensual_Anterior = sueldoMensual.ToString("C");
                            // END: SUELDO ACTUAL DEL EMPLEADO

                            // SUELDO ANTERIOR DEL EMPLEADO
                            tblRH_TAB_GestionModificacionTabuladorDet objPagoAnterior = _context.tblRH_TAB_GestionModificacionTabuladorDet.Where(w => w.FK_IncrementoAnual == objGestion.FK_IncrementoAnual && w.clave_empleado == objGestion.clave_empleado && w.registroActivo).FirstOrDefault();
                            objReporteDTO.sueldoBase_Actual = objPagoAnterior.salario_base.ToString("C");
                            objReporteDTO.complemento_Actual = objPagoAnterior.complemento.ToString("C");
                            objReporteDTO.totalNominal_Actual = objPagoAnterior.suma.ToString("C");

                            sueldoMensual = 0;
                            if (objPuesto.FK_TipoNomina == (int)TipoNominaEnum.quincenal)
                                sueldoMensual = (decimal)objPagoAnterior.suma * 2;
                            else if (objPuesto.FK_TipoNomina == (int)TipoNominaEnum.semanal)
                                sueldoMensual = ((decimal)objPagoAnterior.suma / 7) * (decimal)30.4;

                            objReporteDTO.totalMensual_Actual = sueldoMensual.ToString("C");
                            // END: SUELDO ANTERIOR DEL EMPLEADO

                            lstReporteDTO.Add(objReporteDTO);
                        }
                    }

                    List<string> lstConceptosLN = lstDescLineaNegociosU.Select(e => e.concepto).ToList();

                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add("lstTabPuestos", lstReporteDTO);
                    resultado.Add("lstLineasDeNegocios", lstConceptosLN);
                    #endregion
                }
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.REPORTE, objParamDTO.id, objParamDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        #endregion

        #region REPORTES
        public Dictionary<string, object> GetTabuladoresReporte(TabuladorDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region CATALOGOS
                List<tblRH_EK_Tipos_Nomina> lstTipoNominas = _context.tblRH_EK_Tipos_Nomina.Where(w => w.esActivo == true).ToList();
                List<tblRH_TAB_CatAreaDepartamento> lstAreasDepartamentos = _context.tblRH_TAB_CatAreaDepartamento.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatCategorias> lstCategorias = _context.tblRH_TAB_CatCategorias.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatEsquemaPago> lstEsquemaPagos = _context.tblRH_TAB_CatEsquemaPago.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_Tabuladores> lstTabuladores = _context.tblRH_TAB_Tabuladores.Where(w => w.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo).ToList();
                List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.tabuladorDetAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo).ToList();
                List<tblRH_TAB_CatLineaNegocio> lstLineaNegocios = _context.tblRH_TAB_CatLineaNegocio.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatLineaNegocioDet> lstLineaNegociosDet = _context.tblRH_TAB_CatLineaNegocioDet.Where(w => w.registroActivo).ToList();
                List<tblRH_EK_Puestos> lstPuestos = _context.tblRH_EK_Puestos.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatSindicato> lstSindicatos = _context.tblRH_TAB_CatSindicato.Where(e => e.registroActivo).ToList();

                #endregion

                #region SE OBTIENE LISTADO DE TABULADORES DE PUESTOS
                #region FILTROS
                if (objParamDTO.lstFK_LineaNegocio != null)
                {
                    if (objParamDTO.lstFK_LineaNegocio.Count() > 0)
                    {
                        List<int> lstFK_Tabuladores = lstTabuladoresDet.Where(w => objParamDTO.lstFK_LineaNegocio.Contains(w.FK_LineaNegocio)).Select(s => s.FK_Tabulador).ToList();
                        List<int> lstFK_Puestos = lstTabuladores.Where(w => lstFK_Tabuladores.Contains(w.id)).Select(s => s.FK_Puesto).ToList();
                        lstPuestos = lstPuestos.Where(w => lstFK_Puestos.Contains(w.puesto)).ToList();
                    }
                }

                if (objParamDTO.lstFK_AreaDepartamento != null)
                {
                    if (objParamDTO.lstFK_AreaDepartamento.Count() > 0)
                        lstPuestos = lstPuestos.Where(w => objParamDTO.lstFK_AreaDepartamento.Contains(w.FK_AreaDepartamento)).ToList();
                }

                if (objParamDTO.lstCC != null && objParamDTO.lstCC.Count() > 0)
                {
                    var lstPlantillaTabs = _context.tblRH_TAB_PlantillasPersonal.Where(e => e.registroActivo && e.plantillaAutorizada == EstatusGestionAutorizacionEnum.AUTORIZADO && objParamDTO.lstCC.Contains(e.cc)).Select(e => e.id).ToList();
                    List<int> lstFK_Puestos = _context.tblRH_TAB_PlantillasPersonalDet.Where(e => e.registroActivo && lstPlantillaTabs.Contains(e.FK_Plantilla)).Select(e => e.FK_Puesto).ToList();
                    lstPuestos = lstPuestos.Where(w => lstFK_Puestos.Contains(w.puesto)).ToList();
                }

                if (objParamDTO.lstFK_Puestos != null)
                {
                    if (objParamDTO.lstFK_Puestos.Count() > 0)
                        lstPuestos = lstPuestos.Where(w => objParamDTO.lstFK_Puestos.Contains(w.puesto)).ToList();
                }
                #endregion

                List<int> lstPuestosID = lstPuestos.Select(s => s.puesto).ToList();
                lstTabuladores = lstTabuladores.Where(w => lstPuestosID.Contains(w.FK_Puesto)).ToList();
                List<int> lstTabuladoresID = lstTabuladores.Select(s => s.id).ToList();
                lstTabuladoresDet = lstTabuladoresDet.Where(w => lstTabuladoresID.Contains(w.FK_Tabulador)).ToList();
                if (objParamDTO.lstFK_LineaNegocio != null)
                {
                    if (objParamDTO.lstFK_LineaNegocio.Count() > 0)
                        lstTabuladoresDet = lstTabuladoresDet.Where(w => objParamDTO.lstFK_LineaNegocio.Contains(w.FK_LineaNegocio)).ToList();
                }

                List<TabuladorDetDTO> lstTabuladoresDTO = new List<TabuladorDetDTO>();
                TabuladorDetDTO objTabuladorDTO = new TabuladorDetDTO();
                foreach (var itemTabulador in lstTabuladores)
                {
                    objTabuladorDTO = new TabuladorDetDTO();
                    tblRH_TAB_Tabuladores objTabulador = lstTabuladores.Where(w => w.id == itemTabulador.id).FirstOrDefault();
                    if (objTabulador != null)
                    {
                        objTabuladorDTO.lstLineasNegocios = new List<string>();
                        objTabuladorDTO.lstCategorias = new List<string>();
                        objTabuladorDTO.lstSueldosBases = new List<string>();
                        objTabuladorDTO.lstComplementos = new List<string>();
                        objTabuladorDTO.lstTotalNominal = new List<string>();
                        objTabuladorDTO.lstSueldoMensual = new List<string>();
                        foreach (var itemTabuladorDet in lstTabuladoresDet.Where(w => w.FK_Tabulador == itemTabulador.id).ToList())
                        {
                            tblRH_EK_Puestos objPuesto = lstPuestos.Where(w => w.puesto == itemTabulador.FK_Puesto).FirstOrDefault();
                            if (objPuesto == null)
                                throw new Exception("Ocurrió un error al obtener la información del puesto.");

                            int FK_Tabulador = objTabulador.id;
                            int FK_TabuladorDet = itemTabuladorDet.id;
                            int FK_Puesto = objTabulador.FK_Puesto;
                            int FK_LineaNegocio = itemTabuladorDet.FK_LineaNegocio;
                            int FK_AreaDepartamento = objPuesto.FK_AreaDepartamento;
                            int FK_EsquemaPago = itemTabuladorDet.FK_EsquemaPago;

                            tblRH_TAB_CatLineaNegocio objLineaNegocio = lstLineaNegocios.Where(w => w.id == FK_LineaNegocio).FirstOrDefault();

                            tblRH_EK_Tipos_Nomina objTipoNomina = lstTipoNominas.Where(w => w.tipo_nomina == objPuesto.FK_TipoNomina).FirstOrDefault();
                            if (objTipoNomina == null)
                                throw new Exception("Ocurrió un error al obtener el tipo de nómina del puesto.");

                            tblRH_TAB_CatAreaDepartamento objAreaDepartamento = lstAreasDepartamentos.Where(w => w.id == objPuesto.FK_AreaDepartamento).FirstOrDefault();

                            tblRH_TAB_CatCategorias objCategoria = lstCategorias.Where(w => w.id == itemTabuladorDet.FK_Categoria).FirstOrDefault();
                            if (objCategoria == null)
                                throw new Exception("Ocurrió un error al obtener la categoría del puesto.");

                            tblRH_TAB_CatEsquemaPago objEsquemaPago = lstEsquemaPagos.Where(w => w.id == itemTabuladorDet.FK_EsquemaPago).FirstOrDefault();
                            if (objEsquemaPago == null)
                                throw new Exception("Ocurrió un error al obtener el esquema de pago del puesto.");

                            tblRH_TAB_CatSindicato objSindicato = lstSindicatos.FirstOrDefault(e => e.id == objPuesto.FK_Sindicato);

                            // SE CONSTRUYE PUESTO CON SUS TABULADORES
                            objTabuladorDTO.id = FK_Tabulador;
                            objTabuladorDTO.idPuesto = objTabulador.FK_Puesto;
                            //objTabuladorDTO.puestoDesc = SetPuestoEmpleado(objPuesto); //DIANA PIDIO SEPARAR EL ID EN UNA NUEVA COLUMNA
                            objTabuladorDTO.puestoDesc = objPuesto.descripcion;
                            objTabuladorDTO.FK_Categoria = objCategoria != null ? objCategoria.id : 0;
                            objTabuladorDTO.FK_LineaNegocio = FK_LineaNegocio;
                            objTabuladorDTO.lineaNegocioDesc += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;' title='{0}'>{1}</button><br>", (objLineaNegocio != null ? !string.IsNullOrEmpty(objLineaNegocio.concepto) ? objLineaNegocio.concepto.Trim() : string.Empty : string.Empty), (objLineaNegocio != null ? !string.IsNullOrEmpty(objLineaNegocio.abreviacion) ? objLineaNegocio.abreviacion.Trim() : string.Empty : string.Empty));
                            objTabuladorDTO.FK_AreaDepartamento = FK_AreaDepartamento;
                            objTabuladorDTO.descAreaDepartamento = objAreaDepartamento.concepto;
                            objTabuladorDTO.categoriaDesc += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button><br>", (objCategoria != null ? !string.IsNullOrEmpty(objCategoria.concepto) ? objCategoria.concepto.Trim() : string.Empty : string.Empty));
                            objTabuladorDTO.tipoNominaDesc = !string.IsNullOrEmpty(objTipoNomina.descripcion) ? objTipoNomina.descripcion.Trim() : string.Empty;
                            objTabuladorDTO.descSindicato = objSindicato.concepto;

                            objTabuladorDTO.sueldoBaseStringActual += string.Format("<div class='row'><div class='col-lg-12'><button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button></div>", itemTabuladorDet.sueldoBase.ToString("C"));
                            objTabuladorDTO.complementoStringActual += string.Format("<div class='row'><div class='col-lg-12'><button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button></div>", itemTabuladorDet.complemento.ToString("C"));
                            objTabuladorDTO.totalNominalStringActual += string.Format("<div class='row'><div class='col-lg-12'><button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button></div>", itemTabuladorDet.totalNominal.ToString("C"));
                            objTabuladorDTO.sueldoMensualStringActual += string.Format("<div class='row'><div class='col-lg-12'><button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button></div>", itemTabuladorDet.sueldoMensual.ToString("C"));

                            // INFO PARA GENERAR REPORTE EXCEL
                            string lineaNegocioDesc = (objLineaNegocio != null ? !string.IsNullOrEmpty(objLineaNegocio.concepto) ? objLineaNegocio.concepto.Trim() : string.Empty : string.Empty);
                            objTabuladorDTO.lstLineasNegocios.Add(lineaNegocioDesc);

                            string categoria = (objCategoria != null ? !string.IsNullOrEmpty(objCategoria.concepto) ? objCategoria.concepto.Trim() : string.Empty : string.Empty);
                            objTabuladorDTO.lstCategorias.Add(categoria);

                            objTabuladorDTO.lstSueldosBases.Add(itemTabuladorDet.sueldoBase.ToString("C"));
                            objTabuladorDTO.lstComplementos.Add(itemTabuladorDet.complemento.ToString("C"));
                            objTabuladorDTO.lstTotalNominal.Add(itemTabuladorDet.totalNominal.ToString("C"));
                            objTabuladorDTO.lstSueldoMensual.Add(itemTabuladorDet.sueldoMensual.ToString("C"));
                        }
                        lstTabuladoresDTO.Add(objTabuladorDTO);
                    }
                }

                lstTabuladoresDTO = lstTabuladoresDTO.Where(w => w.id > 0).OrderBy(e => e.puestoDesc).ToList();

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("lstTabPuestos", lstTabuladoresDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetTabuladoresReporte", e, AccionEnum.CONSULTA, objParamDTO.id, objParamDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public List<string> GetCCLineaNegocios(List<int> lineasNegocio)
        {
            var lineas = _context.tblRH_TAB_CatLineaNegocioDet.Where(x => lineasNegocio.Contains(x.FK_LineaNegocio) && x.registroActivo).ToList();
            var _ccs = lineas.Select(x => x.cc).ToList();
            _ccs = _context.tblC_Nom_CatalogoCC.Where(x => _ccs.Contains(x.cc) && (x.semanal || x.quincenal)).Select(x => x.cc).ToList();
            return _ccs;
        }

        public Dictionary<string, object> GetTabuladoresReportePdf(TabuladorDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region CATALOGOS
                List<tblRH_EK_Tipos_Nomina> lstTipoNominas = _context.tblRH_EK_Tipos_Nomina.Where(w => w.esActivo == true).ToList();
                List<tblRH_TAB_CatAreaDepartamento> lstAreasDepartamentos = _context.tblRH_TAB_CatAreaDepartamento.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatCategorias> lstCategorias = _context.tblRH_TAB_CatCategorias.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatEsquemaPago> lstEsquemaPagos = _context.tblRH_TAB_CatEsquemaPago.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_Tabuladores> lstTabuladores = _context.tblRH_TAB_Tabuladores.Where(w => w.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo).ToList();
                List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.registroActivo && w.tabuladorDetAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO).ToList();
                List<tblRH_TAB_CatLineaNegocio> lstLineaNegocios = _context.tblRH_TAB_CatLineaNegocio.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatLineaNegocioDet> lstLineaNegociosDet = _context.tblRH_TAB_CatLineaNegocioDet.Where(w => w.registroActivo).ToList();
                List<tblRH_EK_Puestos> lstPuestos = _context.tblRH_EK_Puestos.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatSindicato> lstSindicatos = _context.tblRH_TAB_CatSindicato.Where(e => e.registroActivo).ToList();
                #endregion

                #region SE OBTIENE LISTADO DE TABULADORES DE PUESTOS
                #region FILTROS
                if (objParamDTO.lstFK_LineaNegocio != null)
                {
                    if (objParamDTO.lstFK_LineaNegocio.Count() > 0)
                    {
                        List<int> lstFK_Tabuladores = lstTabuladoresDet.Where(w => objParamDTO.lstFK_LineaNegocio.Contains(w.FK_LineaNegocio)).Select(s => s.FK_Tabulador).ToList();
                        List<int> lstFK_Puestos = lstTabuladores.Where(w => lstFK_Tabuladores.Contains(w.id)).Select(s => s.FK_Puesto).ToList();
                        lstPuestos = lstPuestos.Where(w => lstFK_Puestos.Contains(w.puesto)).ToList();
                    }
                }

                if (objParamDTO.lstFK_AreaDepartamento != null)
                {
                    if (objParamDTO.lstFK_AreaDepartamento.Count() > 0)
                        lstPuestos = lstPuestos.Where(w => objParamDTO.lstFK_AreaDepartamento.Contains(w.FK_AreaDepartamento)).ToList();
                }

                if (objParamDTO.lstCC != null && objParamDTO.lstCC.Count() > 0)
                {
                    var lstPlantillaTabs = _context.tblRH_TAB_PlantillasPersonal.Where(e => e.registroActivo && e.plantillaAutorizada == EstatusGestionAutorizacionEnum.AUTORIZADO && objParamDTO.lstCC.Contains(e.cc)).Select(e => e.id).ToList();
                    List<int> lstFK_Puestos = _context.tblRH_TAB_PlantillasPersonalDet.Where(e => e.registroActivo && lstPlantillaTabs.Contains(e.FK_Plantilla)).Select(e => e.FK_Puesto).ToList();
                    lstPuestos = lstPuestos.Where(w => lstFK_Puestos.Contains(w.puesto)).ToList();
                }

                if (objParamDTO.lstFK_Puestos != null)
                {
                    if (objParamDTO.lstFK_Puestos.Count() > 0)
                        lstPuestos = lstPuestos.Where(w => objParamDTO.lstFK_Puestos.Contains(w.puesto)).ToList();
                }
                #endregion

                List<int> lstPuestosID = lstPuestos.Select(s => s.puesto).ToList();
                lstTabuladores = lstTabuladores.Where(w => lstPuestosID.Contains(w.FK_Puesto)).ToList();
                List<int> lstTabuladoresID = lstTabuladores.Select(s => s.id).ToList();
                lstTabuladoresDet = lstTabuladoresDet.Where(w => lstTabuladoresID.Contains(w.FK_Tabulador)).ToList();
                if (objParamDTO.lstFK_LineaNegocio != null)
                {
                    if (objParamDTO.lstFK_LineaNegocio.Count() > 0)
                        lstTabuladoresDet = lstTabuladoresDet.Where(w => objParamDTO.lstFK_LineaNegocio.Contains(w.FK_LineaNegocio)).ToList();
                }

                List<RepTabuladoresDTO> lstTabuladoresDTO = new List<RepTabuladoresDTO>();
                RepTabuladoresDTO objNewTabuladorDTO = new RepTabuladoresDTO();
                foreach (var itemTabulador in lstTabuladores)
                {
                    tblRH_TAB_Tabuladores objTabulador = lstTabuladores.Where(w => w.id == itemTabulador.id).FirstOrDefault();
                    if (objTabulador != null)
                    {
                        tblRH_EK_Puestos objPuesto = lstPuestos.Where(w => w.puesto == itemTabulador.FK_Puesto).FirstOrDefault();
                        if (objPuesto == null)
                            throw new Exception("Ocurrió un error al obtener la información del puesto.");

                        foreach (var itemTabuladorDet in lstTabuladoresDet.Where(w => w.FK_Tabulador == itemTabulador.id).ToList())
                        {
                            objNewTabuladorDTO = new RepTabuladoresDTO();

                            tblRH_TAB_CatLineaNegocio objLineaNegocio = lstLineaNegocios.Where(w => w.id == itemTabuladorDet.FK_LineaNegocio).FirstOrDefault();

                            tblRH_EK_Tipos_Nomina objTipoNomina = lstTipoNominas.Where(w => w.tipo_nomina == objPuesto.FK_TipoNomina).FirstOrDefault();
                            if (objTipoNomina == null)
                                throw new Exception("Ocurrió un error al obtener el tipo de nómina del puesto.");

                            tblRH_TAB_CatAreaDepartamento objAreaDepartamento = lstAreasDepartamentos.Where(w => w.id == objPuesto.FK_AreaDepartamento).FirstOrDefault();

                            tblRH_TAB_CatCategorias objCategoria = lstCategorias.Where(w => w.id == itemTabuladorDet.FK_Categoria).FirstOrDefault();
                            if (objCategoria == null)
                                throw new Exception("Ocurrió un error al obtener la categoría del puesto.");

                            tblRH_TAB_CatEsquemaPago objEsquemaPago = lstEsquemaPagos.Where(w => w.id == itemTabuladorDet.FK_EsquemaPago).FirstOrDefault();
                            if (objEsquemaPago == null)
                                throw new Exception("Ocurrió un error al obtener el esquema de pago del puesto.");

                            tblRH_TAB_CatSindicato objSindicato = lstSindicatos.FirstOrDefault(e => e.id == objPuesto.FK_Sindicato);

                            objNewTabuladorDTO.id = objTabulador.FK_Puesto.ToString();
                            objNewTabuladorDTO.puesto = objPuesto.descripcion;
                            objNewTabuladorDTO.lineaNegocios = objLineaNegocio.concepto;
                            objNewTabuladorDTO.categoria = objCategoria.concepto;
                            objNewTabuladorDTO.sueldoBase = itemTabuladorDet.sueldoBase.ToString("C");
                            objNewTabuladorDTO.sueldoComplemento = itemTabuladorDet.complemento.ToString("C");
                            objNewTabuladorDTO.sueldoTotal = itemTabuladorDet.totalNominal.ToString("C");
                            objNewTabuladorDTO.sueldoMensual = itemTabuladorDet.sueldoMensual.ToString("C");
                            objNewTabuladorDTO.nomina = objTipoNomina.descripcion;
                            objNewTabuladorDTO.personal = " ";
                            objNewTabuladorDTO.descAreaDepartamento = objAreaDepartamento.concepto;
                            objNewTabuladorDTO.descSindicato = objSindicato != null && objSindicato.id == 1 ? "S" : "N";

                            lstTabuladoresDTO.Add(objNewTabuladorDTO);
                        }
                    }
                }

                lstTabuladoresDTO = lstTabuladoresDTO.OrderBy(e => e.puesto).ToList();

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("lstTabPuestos", lstTabuladoresDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetTabuladoresReporte", e, AccionEnum.CONSULTA, objParamDTO.id, objParamDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> SendReporteCorreo(TabuladorDTO objParamDTO)
        {
            resultado.Clear();

            using (var _ctx = new MainContext())
            {
                using (var dbContextTransaction = _ctx.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var itemLN in objParamDTO.lstFK_LineaNegocio)
                        {
                            #region REMPLAZAR REGISTRO EXISTENTE
                            var objReporteExistente = _ctx.tblRH_TAB_Reportes.FirstOrDefault(e => e.registroActivo && e.FK_LineaNegocio == itemLN && e.año == objParamDTO.añoReporte);
                            var objLN = _ctx.tblRH_TAB_CatLineaNegocio.FirstOrDefault(e => e.id == itemLN);

                            if (objReporteExistente != null)
                            {
                                objReporteExistente.registroActivo = false;
                                _ctx.SaveChanges();

                                var lstAlerta = _ctx.tblP_Alerta.Where(e => e.visto == false && e.objID == objReporteExistente.id && e.obj == "AutorizacionReporteTabulador").ToList();

                                foreach (var item in lstAlerta)
                                {
                                    item.visto = true;
                                    _ctx.SaveChanges();
                                }
                            }
                            #endregion

                            #region CREAR NUEVO REPORTE
                            var objCEReporte = new tblRH_TAB_Reportes();

                            objCEReporte.FK_LineaNegocio = itemLN;
                            objCEReporte.año = objParamDTO.añoReporte;
                            objCEReporte.estatus = EstatusGestionAutorizacionEnum.PENDIENTE;
                            objCEReporte.FK_UsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                            objCEReporte.FK_UsuarioModificacion = null;
                            objCEReporte.fechaCreacion = DateTime.Now;
                            objCEReporte.fechaModificacion = null;
                            objCEReporte.registroActivo = true;

                            _ctx.tblRH_TAB_Reportes.Add(objCEReporte);
                            _ctx.SaveChanges();

                            #endregion

                            #region CREAR RELACION LINEA Y PUESTO
                            if (objParamDTO.lstFK_LineaNegocio != null)
                            {
                                foreach (var item in objParamDTO.lstFK_LineaNegocio)
                                {
                                    _ctx.tblRH_TAB_Reporte_LineaNegocio.Add(new tblRH_TAB_Reporte_LineaNegocio()
                                    {
                                        FK_Reporte = objCEReporte.id,
                                        FK_LineaNegocio = item,
                                        registroActivo = true,
                                    });
                                }
                            }

                            if (objParamDTO.lstFK_Puestos != null)
                            {
                                foreach (var item in objParamDTO.lstFK_Puestos)
                                {
                                    _ctx.tblRH_TAB_Reporte_Puestos.Add(new tblRH_TAB_Reporte_Puestos()
                                    {
                                        FK_Reporte = objCEReporte.id,
                                        FK_Puesto = item,
                                        registroActivo = true,
                                    });
                                }
                            }

                            if (objParamDTO.lstCC != null)
                            {
                                foreach (var item in objParamDTO.lstCC)
                                {
                                    _ctx.tblRH_TAB_Reporte_CC.Add(new tblRH_TAB_Reporte_CC()
                                    {
                                        FK_Reporte = objCEReporte.id,
                                        cc = item,
                                        registroActivo = true,
                                    });
                                }
                            }

                            #endregion

                            #region AGREGAR FIRMANTES
                            List<tblRH_TAB_GestionAutorizantes> lstGestion = new List<tblRH_TAB_GestionAutorizantes>();
                            tblRH_TAB_GestionAutorizantes objGestion = new tblRH_TAB_GestionAutorizantes();
                            foreach (var item in objParamDTO.lstGestionAutorizantesDTO)
                            {
                                objGestion = new tblRH_TAB_GestionAutorizantes();
                                objGestion.FK_Registro = objCEReporte.id;
                                objGestion.vistaAutorizacion = VistaAutorizacionEnum.REPORTES_TABULADORES;
                                objGestion.nivelAutorizante = item.nivelAutorizante;
                                objGestion.FK_UsuarioAutorizacion = item.FK_UsuarioAutorizacion;
                                objGestion.autorizado = (int)EstatusGestionAutorizacionEnum.PENDIENTE;
                                objGestion.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                                objGestion.fechaCreacion = DateTime.Now;
                                objGestion.registroActivo = true;
                                lstGestion.Add(objGestion);
                            }
                            _ctx.tblRH_TAB_GestionAutorizantes.AddRange(lstGestion);
                            _ctx.SaveChanges();

                            #region Alerta SIGOPLAN
                            //PRIMER AUTORIZANTE
                            var objReponsableCC = objParamDTO.lstGestionAutorizantesDTO.FirstOrDefault();

                            tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                            objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                            objNuevaAlerta.userRecibeID = objReponsableCC.FK_UsuarioAutorizacion;
#if DEBUG
                            objNuevaAlerta.userRecibeID = _USUARIO_OMAR_NUNEZ_ID; // OMAR NUÑEZ
#endif
                            objNuevaAlerta.tipoAlerta = 2;
                            objNuevaAlerta.sistemaID = 16;
                            objNuevaAlerta.visto = false;
                            objNuevaAlerta.url = "/Administrativo/Tabuladores/GestionReportes";
                            objNuevaAlerta.objID = objCEReporte.id;
                            objNuevaAlerta.obj = "AutorizacionReporteTabulador";
                            objNuevaAlerta.msj = string.Format("Rpt de tabuladores Pendiente LN: {0}", objLN.concepto);
                            objNuevaAlerta.documentoID = 0;
                            objNuevaAlerta.moduloID = 0;
                            _ctx.tblP_Alerta.Add(objNuevaAlerta);
                            _ctx.SaveChanges();
                            #endregion //ALERTA SIGPLAN
                            #endregion
                        }

                        #region CORREO

                        string RutaServidor = "";

#if DEBUG
                        RutaServidor = @"C:\Proyectos\SIGOPLANv2\REPORTESCR\CAPITAL_HUMANO";
#else
                        RutaServidor = @"\\10.1.0.112\Proyecto\SIGOPLAN\REPORTESCR\CAPITAL_HUMANO";
#endif

                        List<RepAutorizantesTABDTO> lstRepAuth = new List<RepAutorizantesTABDTO>();
                        List<string> lstCorreosNotificar = new List<string>();

                        if (objParamDTO.lstGestionAutorizantesDTO != null)
                        {
                            foreach (var item in objParamDTO.lstGestionAutorizantesDTO)
                            {
                                var objUsuario = _ctx.tblP_Usuario.FirstOrDefault(e => e.id == item.FK_UsuarioAutorizacion);
                                string nombreCompletoAuth = "";

                                if (objUsuario != null)
                                {
                                    nombreCompletoAuth = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                                    lstCorreosNotificar.Add(objUsuario.correo);
                                }

                                lstRepAuth.Add(new RepAutorizantesTABDTO
                                {
                                    autorizado = EnumHelper.GetDescription(item.autorizado),
                                    puestoAutorizante = EnumHelper.GetDescription(item.nivelAutorizante),
                                    nombreAutorizante = nombreCompletoAuth,
                                });
                            }
                        }

                        ReportDocument rptCV = new rptRepTabuladores();

                        //string path = Path.Combine(RutaServidor, "rptRepTabuladores.rpt");
                        //rptCV.Load(path);

                        var lstDescCC = GetDescCCs(objParamDTO.lstCC);
                        var lstDescLineasNegocio = GetDescLineaNegocio(objParamDTO.lstFK_LineaNegocio);

                        objParamDTO.lstDescCC = lstDescCC;
                        objParamDTO.lstDescLineaNegocio = lstDescLineasNegocio;

                        var dictRepTabuladores = GetTabuladoresReportePdf(objParamDTO);
                        var lstTabuladores = dictRepTabuladores["lstTabPuestos"] as List<RepTabuladoresDTO>;

                        //LIMPIAR RESULTADO DESPUES DE GetTabuladoresReportePdf
                        resultado.Clear();

                        string lstLineasNegocio = String.Join(", ", objParamDTO.lstDescLineaNegocio);
                        string lstCC = String.Join(", ", objParamDTO.lstDescCC);

                        rptCV.Database.Tables[0].SetDataSource(getInfoEnca("Reporte de Tabuladores.", "Dirección de Capital Humano"));
                        rptCV.Database.Tables[1].SetDataSource(lstTabuladores);
                        rptCV.Database.Tables[2].SetDataSource(lstRepAuth);

                        rptCV.SetParameterValue("lstLineasNegocio", lstLineasNegocio);
                        rptCV.SetParameterValue("lstCC", lstCC);
                        rptCV.SetParameterValue("año", objParamDTO.añoReporte);
                        rptCV.SetParameterValue("fecha", DateTime.Now.ToString("dd/MM/yyyy h:mm tt"));

                        Stream stream = rptCV.ExportToStream(ExportFormatType.PortableDocFormat);

                        string nombreCompletoUsrActual = "";

                        var objUsrActual = _ctx.tblP_Usuario.FirstOrDefault(e => e.id == vSesiones.sesionUsuarioDTO.id);

                        if (objUsrActual != null)
                        {
                            nombreCompletoUsrActual = objUsrActual.nombre + " " + objUsrActual.apellidoPaterno + " " + objUsrActual.apellidoMaterno;
                        }

                        #region TABLA AUTORIZANTES

                        string cuerpo =
                                                @"<html>
                                            <head>
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
                                                        background-color: #ffcc5c;
                                                    }
                                                </style>
                                            </head>
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>

                                                    <p class=MsoNormal>
                                                        Buen dia.<br>Se informa que fue realizado un reporte de tabulador para autorización en la(s) LINEA DE NEGOCIOS: " + lstLineasNegocio + " por el usuario (" + nombreCompletoUsrActual + @").<br><br>
                                                    </p>
                                                    
                                                    <br><br><br>
                                    <table>
                                        <thead>
                                            <tr>
                                                <th>Nombre</th>
                                                <th>Tipo</th>
                                                <th>Autorizo</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        ";

                        bool esPrimero = true;
                        //EstatusGestionAutorizacionEnum P 0 A 1 R 2 
                        if (objParamDTO.lstGestionAutorizantesDTO != null)
                        {
                            foreach (var itemDet in objParamDTO.lstGestionAutorizantesDTO)
                            {
                                tblP_Usuario objUsuario = _ctx.tblP_Usuario.FirstOrDefault(e => e.id == itemDet.FK_UsuarioAutorizacion);
                                string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                                cuerpo += "<tr>" +
                                            "<td>" + nombreCompleto + "</td>" +
                                            "<td>" + EnumHelper.GetDescription(itemDet.nivelAutorizante) + "</td>" +
                                            GetEstatusTabulador(0, esPrimero) +
                                        "</tr>";

                                if (esPrimero)
                                {
                                    esPrimero = false;
                                }
                            }
                        }


                        cuerpo += "</tbody>" +
                                    "</table>" +
                                    "<br><br><br>";


                        #endregion

                        lstCorreosNotificar.Add(_CORREO_DIANA_ALVAREZ);
                        lstCorreosNotificar.Add(_CORREO_MANUEL_CRUZ);
#if DEBUG
                        lstCorreosNotificar = new List<string>() { _CORREO_OMAR_NUNEZ };
#endif
                        List<byte[]> downloadPDFs = new List<byte[]>();
                        using (var streamReader = new MemoryStream())
                        {
                            stream.CopyTo(streamReader);
                            downloadPDFs.Add(streamReader.ToArray());

                            GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: REPORTE DE TABULADOR [LINEA DE NEGOCIO: {1}]", PersonalUtilities.GetNombreEmpresa(), lstLineasNegocio), cuerpo +
                            "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                            "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias.", lstCorreosNotificar.Distinct().ToList(), downloadPDFs, "Plantilla.pdf");

                        }
                        #endregion

                        dbContextTransaction.Commit();
                        resultado.Clear();
                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();

                        resultado.Clear();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, e.Message);
                    }
                }
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboFiltroPuestos_Reportes(TabuladorDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region FILL COMBO PUESTOS CON TABULADORES AUTORIZADOS Y SE FILTRA EN BASE AL AREA/DEPARTAMENTO SELECCIONADO

                var lstPuestoIds = new List<int>();

                if (objParamDTO.lstCC != null && objParamDTO.lstCC.Count() > 0)
                {
                    var lstPlantillaTabs = _context.tblRH_TAB_PlantillasPersonal.Where(e => e.registroActivo && e.plantillaAutorizada == EstatusGestionAutorizacionEnum.AUTORIZADO && objParamDTO.lstCC.Contains(e.cc)).Select(e => e.id).ToList();
                    lstPuestoIds = _context.tblRH_TAB_PlantillasPersonalDet.Where(e => e.registroActivo && lstPlantillaTabs.Contains(e.FK_Plantilla)).Select(e => e.FK_Puesto).ToList();

                    if (objParamDTO.lstFK_LineaNegocio != null && objParamDTO.lstFK_LineaNegocio.Count() > 0)
                    {
                        var lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(e => e.registroActivo && e.tabuladorDetAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO &&
                            objParamDTO.lstFK_LineaNegocio.Contains(e.FK_LineaNegocio)).Select(e => e.FK_Tabulador).ToList();

                        var lstPuestosTabuladores = _context.tblRH_TAB_Tabuladores.Where(e => e.registroActivo && e.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO &&
                            lstTabuladoresDet.Contains(e.id) && lstPuestoIds.Contains(e.FK_Puesto)).Select(e => e.FK_Puesto).ToList();

                        lstPuestoIds = lstPuestosTabuladores;
                    }
                }
                else if (objParamDTO.lstFK_LineaNegocio != null && objParamDTO.lstFK_LineaNegocio.Count() > 0)
                {
                    //var lstPlantillaTabs = _context.tblRH_TAB_PlantillasPersonal.Where(e => e.registroActivo && e.plantillaAutorizada == EstatusGestionAutorizacionEnum.AUTORIZADO && lstCCsLN.Contains(e.cc)).Select(e => e.id).ToList();
                    var lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(e => e.registroActivo && e.tabuladorDetAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO &&
                        objParamDTO.lstFK_LineaNegocio.Contains(e.FK_LineaNegocio)).Select(e => e.FK_Tabulador).ToList();

                    var lstPuestosTabuladores = _context.tblRH_TAB_Tabuladores.Where(e => e.registroActivo && e.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO &&
                        lstTabuladoresDet.Contains(e.id)).Select(e => e.FK_Puesto).ToList();

                    lstPuestoIds.AddRange(lstPuestosTabuladores);
                }

                lstPuestoIds = lstPuestoIds.Distinct().ToList();

                //List<int> lstFK_Puestos = _context.tblRH_TAB_Tabuladores.Where(w => w.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO &&
                //    lstPuestoIds.Contains(w.FK_Puesto) && w.registroActivo).Select(s => s.FK_Puesto).ToList();
                List<tblRH_EK_Puestos> lstPuestos = _context.tblRH_EK_Puestos.Where(w => lstPuestoIds.Contains(w.puesto) && w.registroActivo).OrderBy(o => o.descripcion).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstPuestos)
                {
                    tblRH_EK_Puestos objPuesto = lstPuestos.Where(w => w.puesto == item.puesto).FirstOrDefault();
                    if (objPuesto != null)
                    {
                        if (objPuesto.puesto > 0 && !string.IsNullOrEmpty(objPuesto.descripcion))
                        {
                            objComboDTO = new ComboDTO();
                            objComboDTO.Value = objPuesto.puesto.ToString();
                            objComboDTO.Text = string.Format("[{0}] {1}", objPuesto.puesto, objPuesto.descripcion.Trim());
                            lstComboDTO.Add(objComboDTO);
                        }
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboFiltroPuestos_Modificacion", e, AccionEnum.FILLCOMBO, objParamDTO.id, objParamDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        #endregion

        #region GESTION REPORTES
        public Dictionary<string, object> GetGestionReportes(ReportesTabuladoresDTO objParamDTO)
        {
            resultado.Clear();

            using (var _ctx = new MainContext())
            {
                try
                {
                    var lstCC = _ctx.tblC_Nom_CatalogoCC.Where(e => e.estatus).ToList();
                    var lstReportes = _ctx.tblRH_TAB_Reportes.Where(e => e.registroActivo && e.estatus == objParamDTO.estatus && (objParamDTO.año != null ? (e.año == objParamDTO.año) : true)).ToList();
                    var lstReporteDTO = new List<ReportesTabuladoresDTO>();
                    var lstLineaNegocios = _ctx.tblRH_TAB_CatLineaNegocio.Where(e => e.registroActivo);

                    foreach (var item in lstReportes)
                    {
                        var objLN = _ctx.tblRH_TAB_CatLineaNegocio.FirstOrDefault(e => e.id == item.FK_LineaNegocio);

                        var objReportesDTO = new ReportesTabuladoresDTO();
                        objReportesDTO.id = item.id;
                        objReportesDTO.FK_LineaNegocio = item.FK_LineaNegocio;
                        objReportesDTO.estatus = item.estatus;
                        objReportesDTO.comentarioRechazo = item.comentarioRechazo;
                        objReportesDTO.descLN = objLN.concepto;

                        objReportesDTO.año = item.año;

                        //var objLineaNegociosCCDet =_ctx.tblRH_TAB_CatLineaNegocioDet.FirstOrDefault(e => e.cc == item.cc);

                        //var objLineaNegocioCC = lstLineaNegocios.FirstOrDefault(e => e.id == objLineaNegociosCCDet.FK_LineaNegocio);

                        //string lineaNegocioDesc = "";
                        //if (objLineaNegocioCC != null)
                        //{
                        //    lineaNegocioDesc = objLineaNegocioCC.concepto;
                        //}

                        //objReportesDTO.lineaNegociosCC = lineaNegocioDesc;

                        List<tblRH_TAB_GestionAutorizantes> lstAutorizantes = _ctx.tblRH_TAB_GestionAutorizantes.Where(w => w.registroActivo && w.vistaAutorizacion == VistaAutorizacionEnum.REPORTES_TABULADORES && w.FK_Registro == item.id).OrderBy(e => e.nivelAutorizante).ToList();

                        var lstAutorizantesOrdered = new List<tblRH_TAB_GestionAutorizantes>();

                        //INSERTAR EN EL ORDEN CORRECTO LOS AUTORIZANTES
                        //lstAutorizantesOrdered.AddRange(lstAutorizantes.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.RESPONSABLE_CC));
                        //lstAutorizantesOrdered.AddRange(lstAutorizantes.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.DIRECTOR_LINEA_NEGOCIOS));
                        //lstAutorizantesOrdered.AddRange(lstAutorizantes.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.ALTA_DIRECCION));
                        //lstAutorizantesOrdered.AddRange(lstAutorizantes.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.CAPITAL_HUMANO));

                        int? sigAuth = null;

                        foreach (var itemAuth in lstAutorizantes)
                        {
                            if (itemAuth.autorizado == EstatusGestionAutorizacionEnum.AUTORIZADO || itemAuth.autorizado == EstatusGestionAutorizacionEnum.RECHAZADO)
                                objReportesDTO.esFirmar = false;
                            else
                            {
                                if (sigAuth == null)
                                    sigAuth = itemAuth.FK_UsuarioAutorizacion;

                                if (sigAuth.Value == vSesiones.sesionUsuarioDTO.id)
                                {
                                    objReportesDTO.esFirmar = true;
                                    break;
                                }
                                else
                                    objReportesDTO.esFirmar = false;
                            }
                        }

                        lstReporteDTO.Add(objReportesDTO);
                    }

                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, lstReporteDTO);
                }
                catch (Exception e)
                {
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> AutorizarRechazarReporte(ReportesTabuladoresDTO objParamDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado.Clear();
                try
                {
                    #region VALIDACIONES
                    if (objParamDTO.id <= 0) { throw new Exception("Ocurrió un error al realizar la acción."); }
                    if (objParamDTO.estatus <= 0) { throw new Exception("Ocurrió un error al realizar la acción."); }
                    #endregion

                    #region SE AUTORIZA/RECHAZA LA PLANTILLA

                    var lstFirmas = _context.tblRH_TAB_GestionAutorizantes.Where(e => e.registroActivo && e.vistaAutorizacion == VistaAutorizacionEnum.REPORTES_TABULADORES && e.FK_Registro == objParamDTO.id).ToList();
                    tblRH_TAB_Reportes objReporte = _context.tblRH_TAB_Reportes.FirstOrDefault(e => e.registroActivo && e.id == objParamDTO.id);
                    var lstReporte_Puesto = _context.tblRH_TAB_Reporte_Puestos.Where(e => e.registroActivo && e.FK_Reporte == objReporte.id).Select(e => e.FK_Puesto).ToList();
                    var lstReporte_CC = _context.tblRH_TAB_Reporte_CC.Where(e => e.registroActivo && e.FK_Reporte == objReporte.id).Select(e => e.cc).ToList();
                    var lstReporte_LineaNegocio = new List<int>() { objReporte.FK_LineaNegocio };
                    var objLN = _context.tblRH_TAB_CatLineaNegocio.FirstOrDefault(e => e.id == objReporte.FK_LineaNegocio);

                    var lstAutorizantesOrdered = new List<tblRH_TAB_GestionAutorizantes>();
                    List<string> lstCorreosNotificarTodos = new List<string>();
                    List<string> lstCorreosNotificarRestantes = new List<string>();

                    //INSERTAR EN EL ORDEN CORRECTO LOS AUTORIZANTES
                    //lstAutorizantesOrdered.AddRange(lstFirmas.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.RESPONSABLE_CC));
                    //lstAutorizantesOrdered.AddRange(lstFirmas.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.DIRECTOR_LINEA_NEGOCIOS));
                    //lstAutorizantesOrdered.AddRange(lstFirmas.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.ALTA_DIRECCION));
                    //lstAutorizantesOrdered.AddRange(lstFirmas.Where(e => e.nivelAutorizante == NivelAutorizanteEnum.CAPITAL_HUMANO));

                    int totalAuth = 0;
                    bool notifyNextAuth = false;
                    int totalAlertas = 0;

                    foreach (var item in lstFirmas)
                    {
                        tblP_Usuario objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == item.FK_UsuarioAutorizacion);

                        #region AGREGAR ALERTA Y CORREO(a la lista de correos) PARA EL SIGUIENTE AUTORIZANTE
                        if (notifyNextAuth && objParamDTO.estatus == EstatusGestionAutorizacionEnum.AUTORIZADO && totalAlertas == 0)
                        {
                            #region Alerta SIGOPLAN
                            tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                            objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                            objNuevaAlerta.userRecibeID = item.FK_UsuarioAutorizacion;
#if DEBUG
                            //objNuevaAlerta.userRecibeID = _USUARIO_OMAR_NUNEZ_ID; //USUARIO ID: Omar Nuñez.
                            //objNuevaAlerta.userRecibeID = 3807; //USUARIO ID: OSKAR НАХУЙ БЛЯТЬ.
#endif
                            objNuevaAlerta.tipoAlerta = 2;
                            objNuevaAlerta.sistemaID = 16;
                            objNuevaAlerta.visto = false;
                            objNuevaAlerta.url = "/Administrativo/Tabuladores/GestionReportes";
                            objNuevaAlerta.objID = objReporte.id;
                            objNuevaAlerta.obj = "AutorizacionReporteTabulador";
                            objNuevaAlerta.msj = "Rpt de tabuladores pendiente LN: " + objLN.concepto;
                            objNuevaAlerta.documentoID = 0;
                            objNuevaAlerta.moduloID = 0;
                            _context.tblP_Alerta.Add(objNuevaAlerta);
                            _context.SaveChanges();
                            #endregion //ALERTA SIGPLAN

                            //SIGUIENTE EN SER AUTORIZADO
                            lstCorreosNotificarRestantes.Add(objUsuario.correo);

                            //NOTIFICADA
                            notifyNextAuth = false;
                            totalAlertas++;
                        }

                        #endregion

                        if (item.autorizado == EstatusGestionAutorizacionEnum.AUTORIZADO)
                            totalAuth++;
                        else
                        {
                            if (item.FK_UsuarioAutorizacion == vSesiones.sesionUsuarioDTO.id)
                            {
                                if (objParamDTO.estatus == EstatusGestionAutorizacionEnum.AUTORIZADO)
                                {
                                    totalAuth++;
                                    notifyNextAuth = true;

                                    var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == item.FK_UsuarioAutorizacion && e.visto == false && e.objID == objReporte.id && e.obj == "AutorizacionReporteTabulador");
                                    if (objAlerta != null)
                                    {
                                        objAlerta.visto = true;
                                        _context.SaveChanges();
                                    }

                                }
                                else
                                {
                                    var objAlerta = _context.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == item.FK_UsuarioAutorizacion && e.visto == false && e.objID == objReporte.id && e.obj == "AutorizacionReporteTabulador");
                                    if (objAlerta != null)
                                    {
                                        objAlerta.visto = true;
                                        _context.SaveChanges();
                                    }
                                }

                                item.fechaFirma = DateTime.Now;
                                item.autorizado = objParamDTO.estatus;
                                item.fechaModificacion = DateTime.Now;
                                item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                _context.SaveChanges();
                            }
                        }
                    }

                    #region CUERPO CORREO

                    #region PDF
                    string RutaServidor = "";

#if DEBUG
                    RutaServidor = @"C:\Proyectos\SIGOPLANv2\REPORTESCR\CAPITAL_HUMANO";
#else
                    RutaServidor = @"\\10.1.0.112\Proyecto\SIGOPLAN\REPORTESCR\CAPITAL_HUMANO";
#endif

                    List<RepAutorizantesTABDTO> lstRepAuth = new List<RepAutorizantesTABDTO>();
                    List<string> lstCorreosNotificar = new List<string>();

                    ReportDocument rptCV = new rptRepTabuladores();

                    //string path = Path.Combine(RutaServidor, "rptRepTabuladores.rpt");
                    //rptCV.Load(path);

                    var lstDescCC = GetDescCCs(lstReporte_CC);
                    var lstDescLineasNegocio = GetDescLineaNegocio(lstReporte_LineaNegocio);

                    objParamDTO.lstDescCC = lstDescCC;
                    objParamDTO.lstDescLineaNegocio = lstDescLineasNegocio;

                    var dictRepTabuladores = GetTabuladoresReportePdf(new TabuladorDTO
                    {
                        lstFK_LineaNegocio = lstReporte_LineaNegocio,
                        lstFK_Puestos = lstReporte_Puesto,
                        lstCC = lstReporte_CC,
                    });
                    var lstTabuladores = dictRepTabuladores["lstTabPuestos"] as List<RepTabuladoresDTO>;

                    //LIMPIAR RESULTADO DESPUES DE GetTabuladoresReportePdf
                    resultado.Clear();

                    string lstLineasNegocio = String.Join(", ", objParamDTO.lstDescLineaNegocio);
                    string lstCC = String.Join(", ", objParamDTO.lstDescCC);
                    #endregion

                    string cuerpo =
                                @"<html>
                                            <head>
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
                                                        background-color: #ffcc5c;
                                                    }
                                                </style>
                                            </head>
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>

                                                    <p class=MsoNormal>
                                                        Buen día,<br><br>" +
                                                        (totalAuth == lstFirmas.Count() ? "Se ha autorizado un Reporte de Tabuladores por todos los firmantes" : "Se ha autorizado un Reporte de Tabuladores")
                                                        + @".<br><br>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                "Linea de Negocios: " + objLN.concepto + @".<o:p></o:p>
                                                    </p>
                                                    <br><br><br>
                                            ";

                    #region PROBABLEMENTE LO PIDEN

                    //                    var lstGrpTabuladoresDet = lstTabuladoresDet.GroupBy(e => e.FK_LineaNegocio).ToList();

                    //                    foreach (var item in lstGrpTabuladoresDet)
                    //                    {
                    //                        cuerpo += @"
                    //                                            <table>
                    //                                                <thead>
                    //                                                    <tr>
                    //                                                        <th>Linea Negocio</th>
                    //                                                        <th>Categoria</th>
                    //                                                        <th>Sueldo Base</th>
                    //                                                        <th>Complemento</th>
                    //                                                        <th>Total Nominal</th>
                    //                                                        <th>Sueldo Mensual</th>
                    //                                                        <th>Esquema de Pago</th>
                    //                                                    </tr>
                    //                                                </thead>
                    //                                                <tbody>
                    //                                                ";

                    //                        foreach (var itemDet in item)
                    //                        {
                    //                            cuerpo += "<tr>" +
                    //                                "<td>" + lstLineaNegocio.FirstOrDefault(e => e.id == itemDet.FK_LineaNegocio).concepto + "</td>" +
                    //                                "<td>" + lstCategorias.FirstOrDefault(e => e.id == itemDet.FK_Categoria).concepto + "</td>" +
                    //                                "<td>" + itemDet.sueldoBase.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                    //                                "<td>" + itemDet.complemento.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                    //                                "<td>" + itemDet.totalNominal.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                    //                                "<td>" + itemDet.sueldoMensual.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                    //                                "<td>" + lstEsquemaPago.FirstOrDefault(e => e.id == itemDet.FK_EsquemaPago).concepto + "</td>" +
                    //                            "</tr>";
                    //                        }

                    //                        cuerpo += "</tbody>" +
                    //                                  "</table>" +
                    //                                  "<br><br><br>";
                    //                    }
                    #endregion

                    #region TABLA AUTORIZANTES

                    cuerpo += @"
                                        <table>
                                            <thead>
                                                <tr>
                                                    <th>Nombre</th>
                                                    <th>Tipo</th>
                                                    <th>Autorizo</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            ";

                    bool esAuth = false;
                    int totalSiguientes = 0;

                    //EstatusGestionAutorizacionEnum P 0 A 1 R 2 
                    foreach (var itemDet in lstFirmas)
                    {
                        tblP_Usuario objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == itemDet.FK_UsuarioAutorizacion);
                        string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                        cuerpo += "<tr>" +
                                    "<td>" + nombreCompleto + "</td>" +
                                    "<td>" + EnumHelper.GetDescription(itemDet.nivelAutorizante) + "</td>" +
                                    GetEstatusTabulador((int)itemDet.autorizado, esAuth) +
                                "</tr>";

                        if (vSesiones.sesionUsuarioDTO.id == itemDet.FK_UsuarioAutorizacion && totalSiguientes == 0)
                        {
                            esAuth = true;
                            totalSiguientes++;
                        }
                        else
                        {
                            if (esAuth)
                            {
                                esAuth = false;

                                if (itemDet.autorizado == EstatusGestionAutorizacionEnum.AUTORIZADO)
                                {
                                    esAuth = true;
                                    totalSiguientes = 0;
                                }
                            }
                        }

                        #region FIRMAS PDF
                        if (objUsuario != null)
                        {
                            lstCorreosNotificar.Add(objUsuario.correo);
                        }

                        lstRepAuth.Add(new RepAutorizantesTABDTO
                        {
                            autorizado = EnumHelper.GetDescription(itemDet.autorizado),
                            puestoAutorizante = EnumHelper.GetDescription(itemDet.nivelAutorizante),
                            nombreAutorizante = nombreCompleto,
                            firma = itemDet.fechaFirma.HasValue ? GlobalUtils.CrearFirmaDigitalConFecha(itemDet.FK_Registro, DocumentosEnum.TabuladoresPuestos, itemDet.FK_UsuarioAutorizacion, itemDet.fechaFirma.Value) : ""
                        });
                        #endregion
                    }

                    cuerpo += "</tbody>" +
                                "</table>" +
                                "<br><br><br>";


                    #endregion

                    cuerpo += "<br><br><br>" +
                          "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                          "Capital Humano > Administración de personal > Configuración > Estructura Salarial > Gestión de Reportes.<br><br>" +
                          "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                          "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias." +
                        " </body>" +
                      "</html>";

                    rptCV.Database.Tables[0].SetDataSource(getInfoEnca("Reporte de Tabuladores.", "Dirección de Capital Humano"));
                    rptCV.Database.Tables[1].SetDataSource(lstTabuladores);
                    rptCV.Database.Tables[2].SetDataSource(lstRepAuth);

                    rptCV.SetParameterValue("lstLineasNegocio", lstLineasNegocio);
                    rptCV.SetParameterValue("lstCC", lstCC);
                    rptCV.SetParameterValue("año", objReporte.año);
                    rptCV.SetParameterValue("fecha", DateTime.Now.ToString("dd/MM/yyyy h:mm tt"));

                    Stream stream = rptCV.ExportToStream(ExportFormatType.PortableDocFormat);
                    #endregion

                    bool esAuthCompleta = false;
                    if (totalAuth == lstFirmas.Count())
                    {
                        esAuthCompleta = true;
                        objReporte.estatus = objParamDTO.estatus;
                        objReporte.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objReporte.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();

                        lstCorreosNotificarTodos.Add(_CORREO_DIANA_ALVAREZ);
                        lstCorreosNotificarTodos.Add(_CORREO_MANUEL_CRUZ);
#if DEBUG
                        lstCorreosNotificarTodos = new List<string>();
                        lstCorreosNotificarTodos.Add(_CORREO_OMAR_NUNEZ);
#endif
                        List<byte[]> downloadPDFs = new List<byte[]>();
                        using (var streamReader = new MemoryStream())
                        {
                            stream.CopyTo(streamReader);
                            downloadPDFs.Add(streamReader.ToArray());

                            GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: REPORTE TABULADORES AUTORIZADO POR TODOS LOS FIRMANTES EN LA LINEA DE NEGOCIOS: [{1}]", PersonalUtilities.GetNombreEmpresa(), objLN.concepto),
                                cuerpo, lstCorreosNotificarTodos, downloadPDFs, "Plantilla.pdf");
                        }
                    }
                    else
                    {

                        if (objParamDTO.estatus == EstatusGestionAutorizacionEnum.RECHAZADO)
                        {
                            objReporte.estatus = objParamDTO.estatus;
                            objReporte.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objReporte.fechaModificacion = DateTime.Now;
                            _context.SaveChanges();

                        }

                        if (objParamDTO.estatus == EstatusGestionAutorizacionEnum.AUTORIZADO)
                        {
                            #region CORREO ESTATUS
                            lstCorreosNotificarRestantes.Add(_CORREO_DIANA_ALVAREZ);
                            lstCorreosNotificarTodos.Add(_CORREO_MANUEL_CRUZ);
#if DEBUG
                            lstCorreosNotificarRestantes = new List<string>();
                            lstCorreosNotificarRestantes.Add(_CORREO_OMAR_NUNEZ);
#endif
                            List<byte[]> downloadPDFs = new List<byte[]>();
                            using (var streamReader = new MemoryStream())
                            {
                                stream.CopyTo(streamReader);
                                downloadPDFs.Add(streamReader.ToArray());

                                GlobalUtils.sendEmailAdjuntoInMemory2(string.Format(("{0}: REPORTE TABULADORES AUTORIZADO EN LA LINEA DE NEGOCIOS: [{1}]"), PersonalUtilities.GetNombreEmpresa(), objLN.concepto),
                                    cuerpo, lstCorreosNotificarRestantes, downloadPDFs, "Plantilla.pdf");

                            }

                            #endregion
                        }

                    }

                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add("esAuthCompleta", esAuthCompleta);
                    resultado.Add(MESSAGE, objParamDTO.estatus == EstatusGestionAutorizacionEnum.AUTORIZADO ? "Se ha autorizado con éxito." : "Se ha rechazado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, objParamDTO.id, JsonUtils.convertNetObjectToJson(objParamDTO));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "AutorizarRechazarReporte", e, AccionEnum.ACTUALIZAR, objParamDTO.id, objParamDTO);
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetLstAutorizantesReporte(int idReporte)
        {
            resultado.Clear();

            using (var _ctx = new MainContext())
            {
                try
                {
                    List<tblRH_TAB_GestionAutorizantes> lstAutorizantes = _ctx.tblRH_TAB_GestionAutorizantes.Where(w => w.vistaAutorizacion == VistaAutorizacionEnum.REPORTES_TABULADORES && w.FK_Registro == idReporte && w.registroActivo).OrderBy(e => e.nivelAutorizante).ToList();
                    GestionAutorizanteDTO objAutorizanteDTO = new GestionAutorizanteDTO();
                    List<GestionAutorizanteDTO> lstAutorizantesDTO = new List<GestionAutorizanteDTO>();
                    foreach (var item in lstAutorizantes)
                    {
                        int usuarioYaEnLista = lstAutorizantesDTO.Where(w => w.FK_UsuarioAutorizacion == item.FK_UsuarioAutorizacion).Count();

                        if (usuarioYaEnLista <= 0)
                        {
                            tblP_Usuario objUsuario = _ctx.tblP_Usuario.Where(w => w.id == item.FK_UsuarioAutorizacion && w.estatus).FirstOrDefault();
                            if (objUsuario == null)
                                throw new Exception("Ocurrió un error al obtener el nombre del autorizante.");

                            objAutorizanteDTO = new GestionAutorizanteDTO();
                            objAutorizanteDTO.id = item.id;
                            objAutorizanteDTO.nombreAutorizante = PersonalUtilities.NombreCompletoPrimerLetraMayuscula(string.Format("{0} {1} {2}", objUsuario.nombre, objUsuario.apellidoPaterno, objUsuario.apellidoMaterno));
                            objAutorizanteDTO.FK_Registro = item.FK_Registro;
                            objAutorizanteDTO.vistaAutorizacion = item.vistaAutorizacion;
                            objAutorizanteDTO.nivelAutorizante = item.nivelAutorizante;
                            objAutorizanteDTO.FK_UsuarioAutorizacion = item.FK_UsuarioAutorizacion;
                            objAutorizanteDTO.autorizado = item.autorizado;
                            objAutorizanteDTO.fechaFirma = item.fechaFirma;
                            objAutorizanteDTO.comentario = item.comentario;
                            objAutorizanteDTO.FK_UsuarioCreacion = item.FK_UsuarioCreacion;
                            objAutorizanteDTO.FK_UsuarioModificacion = item.FK_UsuarioModificacion;
                            objAutorizanteDTO.fechaCreacion = item.fechaCreacion;
                            objAutorizanteDTO.fechaModificacion = item.fechaModificacion;
                            lstAutorizantesDTO.Add(objAutorizanteDTO);
                        }
                    }

                    //INSERTAR EN EL ORDEN CORRECTO LOS AUTORIZANTES
                    //var lstAutorizantesOrdenAutorizantes = new List<GestionAutorizanteDTO>();
                    //lstAutorizantesOrdenAutorizantes = new List<GestionAutorizanteDTO>();
                    //lstAutorizantesOrdenAutorizantes.AddRange(lstAutorizantesDTO.Where(w => w.nivelAutorizante == NivelAutorizanteEnum.RESPONSABLE_CC));
                    //lstAutorizantesOrdenAutorizantes.AddRange(lstAutorizantesDTO.Where(w => w.nivelAutorizante == NivelAutorizanteEnum.DIRECTOR_LINEA_NEGOCIOS));
                    //lstAutorizantesOrdenAutorizantes.AddRange(lstAutorizantesDTO.Where(w => w.nivelAutorizante == NivelAutorizanteEnum.ALTA_DIRECCION));
                    //lstAutorizantesOrdenAutorizantes.AddRange(lstAutorizantesDTO.Where(w => w.nivelAutorizante == NivelAutorizanteEnum.CAPITAL_HUMANO));

                    resultado.Clear();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, lstAutorizantesDTO);
                }
                catch (Exception e)
                {
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarComentarioRechazoReporte(int idReporte, string comentario)
        {
            resultado.Clear();

            using (var _ctx = new MainContext())
            {
                using (var dbContextTransaction = _ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var objReporte = _ctx.tblRH_TAB_Reportes.FirstOrDefault(e => e.id == idReporte);
                        objReporte.comentarioRechazo = comentario;
                        _ctx.SaveChanges();

                        //var lstReporte_LineaNegocio = _context.tblRH_TAB_Reporte_LineaNegocio.Where(e => e.registroActivo && e.FK_Reporte == objReporte.id).Select(e => e.FK_LineaNegocio).ToList();
                        var lstReporte_LineaNegocio = new List<int>() { objReporte.FK_LineaNegocio };
                        var lstReporte_Puesto = _context.tblRH_TAB_Reporte_Puestos.Where(e => e.registroActivo && e.FK_Reporte == objReporte.id).Select(e => e.FK_Puesto).ToList();
                        var lstReporte_CC = _context.tblRH_TAB_Reporte_CC.Where(e => e.registroActivo && e.FK_Reporte == objReporte.id).Select(e => e.cc).ToList();
                        //var lstReporte_CC = new List<string>() { objReporte.cc};

                        //var objCC = _context.tblP_CC.FirstOrDefault(e => e.cc == objReporte.cc);
                        var objLN = _context.tblRH_TAB_CatLineaNegocio.FirstOrDefault(e => e.id == objReporte.FK_LineaNegocio);
                        var lstFirmas = _ctx.tblRH_TAB_GestionAutorizantes.Where(e => e.registroActivo && e.vistaAutorizacion == VistaAutorizacionEnum.REPORTES_TABULADORES && e.FK_Registro == idReporte).ToList();
                        var lstCorreosNotificarRestantes = new List<string>();
                        List<string> lstCorreosNotificar = new List<string>();
                        List<RepAutorizantesTABDTO> lstRepAuth = new List<RepAutorizantesTABDTO>();

                        #region CUERPO CORREO

                        #region PDF
                        string RutaServidor = "";

#if DEBUG
                        RutaServidor = @"C:\Proyectos\SIGOPLANv2\REPORTESCR\CAPITAL_HUMANO";
#else
                        RutaServidor = @"\\10.1.0.112\Proyecto\SIGOPLAN\REPORTESCR\CAPITAL_HUMANO";
#endif

                        //List<RepAutorizantesTABDTO> lstRepAuth = new List<RepAutorizantesTABDTO>();
                        //List<string> lstCorreosNotificar = new List<string>();

                        ReportDocument rptCV = new rptRepTabuladores();

                        //string path = Path.Combine(RutaServidor, "rptRepTabuladores.rpt");
                        //rptCV.Load(path);

                        var lstDescCC = GetDescCCs(lstReporte_CC);
                        var lstDescLineasNegocio = GetDescLineaNegocio(lstReporte_LineaNegocio);

                        TabuladorDTO objParamDTO = new TabuladorDTO();

                        objParamDTO.lstDescCC = lstDescCC;
                        objParamDTO.lstDescLineaNegocio = lstDescLineasNegocio;
                        objParamDTO.lstFK_LineaNegocio = lstReporte_LineaNegocio;
                        objParamDTO.lstFK_Puestos = lstReporte_Puesto;
                        objParamDTO.lstCC = lstReporte_CC;

                        var dictRepTabuladores = GetTabuladoresReportePdf(objParamDTO);
                        var lstTabuladores = dictRepTabuladores["lstTabPuestos"] as List<RepTabuladoresDTO>;

                        //LIMPIAR RESULTADO DESPUES DE GetTabuladoresReportePdf
                        resultado.Clear();

                        string lstLineasNegocio = String.Join(", ", objParamDTO.lstDescLineaNegocio);
                        string lstCC = String.Join(", ", objParamDTO.lstDescCC);
                        #endregion


                        string cuerpo =
                                    @"<html>
                                            <head>
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
                                                        background-color: #ffcc5c;
                                                    }
                                                </style>
                                            </head>
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>

                                                    <p class=MsoNormal>
                                                        Buen día,<br><br>Se ha rechazado un reporte de tabuladores. Motivo : " + comentario + "." + @".<br><br>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                    "LINEA NEGOCIOS: " + objLN.concepto + @".<o:p></o:p>
                                                    </p>
                                                    <br><br><br>
                                            ";

                        #region TABLA AUTORIZANTES

                        cuerpo += @"
                                        <table>
                                            <thead>
                                                <tr>
                                                    <th>Nombre</th>
                                                    <th>Tipo</th>
                                                    <th>Autorizo</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            ";

                        bool esAuth = false;
                        //EstatusGestionAutorizacionEnum P 0 A 1 R 2 
                        foreach (var itemDet in lstFirmas)
                        {
                            tblP_Usuario objUsuario = _ctx.tblP_Usuario.FirstOrDefault(e => e.id == itemDet.FK_UsuarioAutorizacion);

                            if (vSesiones.sesionUsuarioDTO.id == itemDet.FK_UsuarioAutorizacion)
                            {
                                itemDet.autorizado = EstatusGestionAutorizacionEnum.RECHAZADO;
                            }

                            string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                            cuerpo += "<tr>" +
                                        "<td>" + nombreCompleto + "</td>" +
                                        "<td>" + EnumHelper.GetDescription(itemDet.nivelAutorizante) + "</td>" +
                                        GetEstatusTabulador((int)itemDet.autorizado, false) +
                                    "</tr>";

                            #region FIRMAS PDF
                            if (objUsuario != null)
                            {
                                lstCorreosNotificar.Add(objUsuario.correo);
                            }

                            lstRepAuth.Add(new RepAutorizantesTABDTO
                            {
                                autorizado = EnumHelper.GetDescription(itemDet.autorizado),
                                puestoAutorizante = EnumHelper.GetDescription(itemDet.nivelAutorizante),
                                nombreAutorizante = nombreCompleto,
                            });
                            #endregion

                        }

                        cuerpo += "</tbody>" +
                                    "</table>" +
                                    "<br><br><br>";


                        #endregion

                        cuerpo += "<br><br><br>" +
                              "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                              "Capital Humano > Administración de personal > Configuración > Estructura Salarial > Gestión de Reportes.<br><br>" +
                              "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                              "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias." +
                            " </body>" +
                          "</html>";

                        rptCV.Database.Tables[0].SetDataSource(getInfoEnca("Reporte de Tabuladores.", "Dirección de Capital Humano"));
                        rptCV.Database.Tables[1].SetDataSource(lstTabuladores);
                        rptCV.Database.Tables[2].SetDataSource(lstRepAuth);

                        rptCV.SetParameterValue("lstLineasNegocio", lstLineasNegocio);
                        rptCV.SetParameterValue("lstCC", lstCC);
                        rptCV.SetParameterValue("año", objReporte.año);
                        rptCV.SetParameterValue("fecha", DateTime.Now.ToString("dd/MM/yyyy h:mm tt"));

                        Stream stream = rptCV.ExportToStream(ExportFormatType.PortableDocFormat);

                        #endregion

                        lstCorreosNotificarRestantes.Add(_CORREO_DIANA_ALVAREZ);
                        lstCorreosNotificarRestantes.Add(_CORREO_MANUEL_CRUZ);
#if DEBUG
                        lstCorreosNotificarRestantes = new List<string>();
                        lstCorreosNotificarRestantes.Add(_CORREO_OMAR_NUNEZ);
#endif
                        List<byte[]> downloadPDFs = new List<byte[]>();
                        using (var streamReader = new MemoryStream())
                        {
                            stream.CopyTo(streamReader);
                            downloadPDFs.Add(streamReader.ToArray());

                            GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: REPORTE TABULADORES RECHAZADO EN LA LINEA DE NEGOCIOS: [{1}]", PersonalUtilities.GetNombreEmpresa(), objLN.concepto),
                                cuerpo, lstCorreosNotificarRestantes, downloadPDFs, "Plantilla.pdf");

                        }

                        dbContextTransaction.Commit();
                        resultado.Clear();
                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Clear();
                        resultado.Add(MESSAGE, e.Message);
                        resultado.Add(SUCCESS, false);
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetTabuladoresReporteByCC(TabuladorDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region CATALOGOS
                List<tblRH_EK_Tipos_Nomina> lstTipoNominas = _context.tblRH_EK_Tipos_Nomina.Where(w => w.esActivo == true).ToList();
                List<tblRH_TAB_CatAreaDepartamento> lstAreasDepartamentos = _context.tblRH_TAB_CatAreaDepartamento.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatCategorias> lstCategorias = _context.tblRH_TAB_CatCategorias.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatEsquemaPago> lstEsquemaPagos = _context.tblRH_TAB_CatEsquemaPago.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_Tabuladores> lstTabuladores = _context.tblRH_TAB_Tabuladores.Where(w => w.tabuladorAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo).ToList();
                List<tblRH_TAB_TabuladoresDet> lstTabuladoresDet = _context.tblRH_TAB_TabuladoresDet.Where(w => w.tabuladorDetAutorizado == EstatusGestionAutorizacionEnum.AUTORIZADO && w.registroActivo).ToList();
                List<tblRH_TAB_CatLineaNegocio> lstLineaNegocios = _context.tblRH_TAB_CatLineaNegocio.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatLineaNegocioDet> lstLineaNegociosDet = _context.tblRH_TAB_CatLineaNegocioDet.Where(w => w.registroActivo).ToList();
                List<tblRH_EK_Puestos> lstPuestos = _context.tblRH_EK_Puestos.Where(w => w.registroActivo).ToList();
                List<tblRH_TAB_CatSindicato> lstSindicatos = _context.tblRH_TAB_CatSindicato.Where(e => e.registroActivo).ToList();

                #endregion

                var objReporte = _context.tblRH_TAB_Reportes.FirstOrDefault(e => e.registroActivo && e.año == objParamDTO.añoReporte && e.FK_LineaNegocio == objParamDTO.FK_LineaNegocio);

                if (objReporte == null)
                    throw new Exception("Ocurrio algo mal, favor de contactarse con el departamento de TI");

                //var lstReporte_LineaNegocio = _context.tblRH_TAB_Reporte_LineaNegocio.Where(e => e.registroActivo && e.FK_Reporte == objReporte.id).Select(e => e.FK_LineaNegocio).ToList();

                var lstReporte_LineaNegocio = new List<int>() { objParamDTO.FK_LineaNegocio };

                var lstReporte_Puesto = _context.tblRH_TAB_Reporte_Puestos.Where(e => e.registroActivo && e.FK_Reporte == objReporte.id).Select(e => e.FK_Puesto).ToList();
                var lstReporte_CC = _context.tblRH_TAB_Reporte_CC.Where(e => e.registroActivo && e.FK_Reporte == objReporte.id).Select(e => e.cc).ToList();

                objParamDTO.lstFK_LineaNegocio = lstReporte_LineaNegocio;
                objParamDTO.lstFK_Puestos = lstReporte_Puesto;
                objParamDTO.lstCC = lstReporte_CC;

                #region SE OBTIENE LISTADO DE TABULADORES DE PUESTOS
                #region FILTROS
                if (objParamDTO.lstFK_LineaNegocio != null)
                {
                    if (objParamDTO.lstFK_LineaNegocio.Count() > 0)
                    {
                        List<int> lstFK_Tabuladores = lstTabuladoresDet.Where(w => objParamDTO.lstFK_LineaNegocio.Contains(w.FK_LineaNegocio)).Select(s => s.FK_Tabulador).ToList();
                        List<int> lstFK_Puestos = lstTabuladores.Where(w => lstFK_Tabuladores.Contains(w.id)).Select(s => s.FK_Puesto).ToList();
                        lstPuestos = lstPuestos.Where(w => lstFK_Puestos.Contains(w.puesto)).ToList();
                    }
                }

                if (objParamDTO.lstFK_AreaDepartamento != null)
                {
                    if (objParamDTO.lstFK_AreaDepartamento.Count() > 0)
                        lstPuestos = lstPuestos.Where(w => objParamDTO.lstFK_AreaDepartamento.Contains(w.FK_AreaDepartamento)).ToList();
                }

                if (objParamDTO.lstCC != null)
                {
                    if (objParamDTO.lstCC.Count() > 0)
                    {
                        List<tblRH_EK_Empleados> lstEmpleados = _context.tblRH_EK_Empleados.Where(w => objParamDTO.lstCC.Contains(w.cc_contable) && w.estatus_empleado == "A").ToList();
                        List<int> lstFK_Puestos = new List<int>();
                        foreach (var item in lstEmpleados)
                        {
                            if (item.puesto > 0)
                            {
                                int puesto = Convert.ToInt32(item.puesto);
                                lstFK_Puestos.Add(puesto);
                            }
                        }
                        lstPuestos = lstPuestos.Where(w => lstFK_Puestos.Contains(w.puesto)).ToList();
                    }
                }

                if (objParamDTO.lstFK_Puestos != null)
                {
                    if (objParamDTO.lstFK_Puestos.Count() > 0)
                        lstPuestos = lstPuestos.Where(w => objParamDTO.lstFK_Puestos.Contains(w.puesto)).ToList();
                }
                #endregion

                List<int> lstPuestosID = lstPuestos.Select(s => s.puesto).ToList();
                lstTabuladores = lstTabuladores.Where(w => lstPuestosID.Contains(w.FK_Puesto)).ToList();
                List<int> lstTabuladoresID = lstTabuladores.Select(s => s.id).ToList();
                lstTabuladoresDet = lstTabuladoresDet.Where(w => lstTabuladoresID.Contains(w.FK_Tabulador)).ToList();
                if (objParamDTO.lstFK_LineaNegocio != null)
                {
                    if (objParamDTO.lstFK_LineaNegocio.Count() > 0)
                        lstTabuladoresDet = lstTabuladoresDet.Where(w => objParamDTO.lstFK_LineaNegocio.Contains(w.FK_LineaNegocio)).ToList();
                }

                List<TabuladorDetDTO> lstTabuladoresDTO = new List<TabuladorDetDTO>();
                TabuladorDetDTO objTabuladorDTO = new TabuladorDetDTO();
                foreach (var itemTabulador in lstTabuladores)
                {
                    objTabuladorDTO = new TabuladorDetDTO();
                    tblRH_TAB_Tabuladores objTabulador = lstTabuladores.Where(w => w.id == itemTabulador.id).FirstOrDefault();
                    if (objTabulador != null)
                    {
                        objTabuladorDTO.lstLineasNegocios = new List<string>();
                        objTabuladorDTO.lstCategorias = new List<string>();
                        objTabuladorDTO.lstSueldosBases = new List<string>();
                        objTabuladorDTO.lstComplementos = new List<string>();
                        objTabuladorDTO.lstTotalNominal = new List<string>();
                        objTabuladorDTO.lstSueldoMensual = new List<string>();
                        foreach (var itemTabuladorDet in lstTabuladoresDet.Where(w => w.FK_Tabulador == itemTabulador.id).ToList())
                        {
                            tblRH_EK_Puestos objPuesto = lstPuestos.Where(w => w.puesto == itemTabulador.FK_Puesto).FirstOrDefault();
                            if (objPuesto == null)
                                throw new Exception("Ocurrió un error al obtener la información del puesto.");

                            int FK_Tabulador = objTabulador.id;
                            int FK_TabuladorDet = itemTabuladorDet.id;
                            int FK_Puesto = objTabulador.FK_Puesto;
                            int FK_LineaNegocio = itemTabuladorDet.FK_LineaNegocio;
                            int FK_AreaDepartamento = objPuesto.FK_AreaDepartamento;
                            int FK_EsquemaPago = itemTabuladorDet.FK_EsquemaPago;

                            tblRH_TAB_CatLineaNegocio objLineaNegocio = lstLineaNegocios.Where(w => w.id == FK_LineaNegocio).FirstOrDefault();

                            tblRH_EK_Tipos_Nomina objTipoNomina = lstTipoNominas.Where(w => w.tipo_nomina == objPuesto.FK_TipoNomina).FirstOrDefault();
                            if (objTipoNomina == null)
                                throw new Exception("Ocurrió un error al obtener el tipo de nómina del puesto.");

                            tblRH_TAB_CatAreaDepartamento objAreaDepartamento = lstAreasDepartamentos.Where(w => w.id == objPuesto.FK_AreaDepartamento).FirstOrDefault();

                            tblRH_TAB_CatCategorias objCategoria = lstCategorias.Where(w => w.id == itemTabuladorDet.FK_Categoria).FirstOrDefault();
                            if (objCategoria == null)
                                throw new Exception("Ocurrió un error al obtener la categoría del puesto.");

                            tblRH_TAB_CatEsquemaPago objEsquemaPago = lstEsquemaPagos.Where(w => w.id == itemTabuladorDet.FK_EsquemaPago).FirstOrDefault();
                            if (objEsquemaPago == null)
                                throw new Exception("Ocurrió un error al obtener el esquema de pago del puesto.");

                            tblRH_TAB_CatSindicato objSindicato = lstSindicatos.FirstOrDefault(e => e.id == objPuesto.FK_Sindicato);

                            // SE CONSTRUYE PUESTO CON SUS TABULADORES
                            objTabuladorDTO.id = FK_Tabulador;
                            objTabuladorDTO.idPuesto = objTabulador.FK_Puesto;
                            //objTabuladorDTO.puestoDesc = SetPuestoEmpleado(objPuesto); //DIANA PIDIO SEPARAR EL ID EN UNA NUEVA COLUMNA
                            objTabuladorDTO.puestoDesc = objPuesto.descripcion;
                            objTabuladorDTO.FK_Categoria = objCategoria != null ? objCategoria.id : 0;
                            objTabuladorDTO.FK_LineaNegocio = FK_LineaNegocio;
                            objTabuladorDTO.lineaNegocioDesc += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;' title='{0}'>{1}</button><br>", (objLineaNegocio != null ? !string.IsNullOrEmpty(objLineaNegocio.concepto) ? objLineaNegocio.concepto.Trim() : string.Empty : string.Empty), (objLineaNegocio != null ? !string.IsNullOrEmpty(objLineaNegocio.abreviacion) ? objLineaNegocio.abreviacion.Trim() : string.Empty : string.Empty));
                            objTabuladorDTO.FK_AreaDepartamento = FK_AreaDepartamento;
                            objTabuladorDTO.descAreaDepartamento = objAreaDepartamento.concepto;
                            objTabuladorDTO.categoriaDesc += string.Format("<button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button><br>", (objCategoria != null ? !string.IsNullOrEmpty(objCategoria.concepto) ? objCategoria.concepto.Trim() : string.Empty : string.Empty));
                            objTabuladorDTO.tipoNominaDesc = !string.IsNullOrEmpty(objTipoNomina.descripcion) ? objTipoNomina.descripcion.Trim() : string.Empty;
                            objTabuladorDTO.descSindicato = objSindicato.concepto;

                            objTabuladorDTO.sueldoBaseStringActual += string.Format("<div class='row'><div class='col-lg-12'><button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button></div>", itemTabuladorDet.sueldoBase.ToString("C"));
                            objTabuladorDTO.complementoStringActual += string.Format("<div class='row'><div class='col-lg-12'><button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button></div>", itemTabuladorDet.complemento.ToString("C"));
                            objTabuladorDTO.totalNominalStringActual += string.Format("<div class='row'><div class='col-lg-12'><button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button></div>", itemTabuladorDet.totalNominal.ToString("C"));
                            objTabuladorDTO.sueldoMensualStringActual += string.Format("<div class='row'><div class='col-lg-12'><button type='text' class='btn btn-info' style='margin-bottom: 3px;'>{0}</button></div>", itemTabuladorDet.sueldoMensual.ToString("C"));

                            // INFO PARA GENERAR REPORTE EXCEL
                            string lineaNegocioDesc = (objLineaNegocio != null ? !string.IsNullOrEmpty(objLineaNegocio.concepto) ? objLineaNegocio.concepto.Trim() : string.Empty : string.Empty);
                            objTabuladorDTO.lstLineasNegocios.Add(lineaNegocioDesc);

                            string categoria = (objCategoria != null ? !string.IsNullOrEmpty(objCategoria.concepto) ? objCategoria.concepto.Trim() : string.Empty : string.Empty);
                            objTabuladorDTO.lstCategorias.Add(categoria);

                            objTabuladorDTO.lstSueldosBases.Add(itemTabuladorDet.sueldoBase.ToString("C"));
                            objTabuladorDTO.lstComplementos.Add(itemTabuladorDet.complemento.ToString("C"));
                            objTabuladorDTO.lstTotalNominal.Add(itemTabuladorDet.totalNominal.ToString("C"));
                            objTabuladorDTO.lstSueldoMensual.Add(itemTabuladorDet.sueldoMensual.ToString("C"));
                        }
                        lstTabuladoresDTO.Add(objTabuladorDTO);
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("lstTabPuestos", lstTabuladoresDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetTabuladoresReporte", e, AccionEnum.CONSULTA, objParamDTO.id, objParamDTO);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public TabuladorDTO GetParametrosReporte(int idReporte)
        {
            var objParametro = new TabuladorDTO();

            try
            {
                var objReporte = _context.tblRH_TAB_Reportes.FirstOrDefault(e => e.registroActivo && e.id == idReporte);

                if (objReporte == null)
                    throw new Exception("Ocurrio algo mal, favor de contactarse con el departamento de TI");

                //var lstReporte_LineaNegocio = _context.tblRH_TAB_Reporte_LineaNegocio.Where(e => e.registroActivo && e.FK_Reporte == objReporte.id).Select(e => e.FK_LineaNegocio).ToList();
                var lstReporte_LineaNegocio = new List<int>() { objReporte.FK_LineaNegocio };

                var lstReporte_Puesto = _context.tblRH_TAB_Reporte_Puestos.Where(e => e.registroActivo && e.FK_Reporte == objReporte.id).Select(e => e.FK_Puesto).ToList();
                var lstReporte_CC = _context.tblRH_TAB_Reporte_CC.Where(e => e.registroActivo && e.FK_Reporte == objReporte.id).Select(e => e.cc).ToList();

                objParametro.lstFK_LineaNegocio = lstReporte_LineaNegocio;
                objParametro.lstFK_Puestos = lstReporte_Puesto;
                objParametro.lstCC = lstReporte_CC;
                objParametro.añoReporte = objReporte.año;
            }
            catch (Exception e)
            {
                throw e;
            }

            return objParametro;
        }

        public Dictionary<string, object> AutorizarReportesMasivo(List<int> lstIdReportes)
        {
            resultado.Clear();

            using (var _ctx = new MainContext())
            {
                using (var dbContextTransaction = _ctx.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var itemRep in lstIdReportes)
                        {
                            var lstFirmas = _ctx.tblRH_TAB_GestionAutorizantes.Where(e =>
                                e.registroActivo
                                && e.vistaAutorizacion == VistaAutorizacionEnum.REPORTES_TABULADORES
                                && e.FK_Registro == itemRep).ToList();

                            var objReporte = _ctx.tblRH_TAB_Reportes.FirstOrDefault(e => e.registroActivo && e.id == itemRep);
                            var lstReporte_CC = _ctx.tblRH_TAB_Reporte_CC.Where(e => e.registroActivo && e.FK_Reporte == objReporte.id).Select(e => e.cc).ToList();

                            var objLN = _ctx.tblRH_TAB_CatLineaNegocio.FirstOrDefault(e => e.id == objReporte.FK_LineaNegocio);
                            //var objCC = _ctx.tblP_CC.FirstOrDefault(e => e.cc == objReporte.cc);

                            var lstCorreosNotificarRestantes = new List<string>();
                            var lstCorreosNotificarTodos = new List<string>();

                            #region AUTORIZAR FIRMAS Y ADMN ALERTA
                            int totalAuth = 0;
                            bool notifyNextAuth = false;
                            int totalAlertas = 0;
                            bool esAutorizar = false;

                            int? sigAuth = null;
                            bool esSigAuth = false;

                            foreach (var item in lstFirmas)
                            {
                                tblP_Usuario objUsuario = _ctx.tblP_Usuario.FirstOrDefault(e => e.id == item.FK_UsuarioAutorizacion);
                                lstCorreosNotificarTodos.Add(objUsuario.correo);

                                //if (item.autorizado == EstatusGestionAutorizacionEnum.PENDIENTE)
                                //{
                                //    lstCorreosNotificarRestantes.Add(objUsuario.correo);
                                //}

                                #region AGREGAR ALERTA PARA EL SIGUIENTE AUTORIZANTE
                                if (notifyNextAuth && totalAlertas == 0)
                                {
                                    #region Alerta SIGOPLAN
                                    tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                                    objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                                    objNuevaAlerta.userRecibeID = item.FK_UsuarioAutorizacion;
#if DEBUG
                                    //objNuevaAlerta.userRecibeID = _USUARIO_OMAR_NUNEZ_ID; //USUARIO ID: Omar Nuñez.
                                    //objNuevaAlerta.userRecibeID = 3807; //USUARIO ID: OSKAR НАХУЙ БЛЯТЬ.
#endif
                                    objNuevaAlerta.tipoAlerta = 2;
                                    objNuevaAlerta.sistemaID = 16;
                                    objNuevaAlerta.visto = false;
                                    objNuevaAlerta.url = "/Administrativo/Tabuladores/GestionReportes";
                                    objNuevaAlerta.objID = objReporte.id;
                                    objNuevaAlerta.obj = "AutorizacionReporteTabulador";
                                    objNuevaAlerta.msj = "Rpt de tabuladores pendiente LN: " + objLN.concepto;
                                    objNuevaAlerta.documentoID = 0;
                                    objNuevaAlerta.moduloID = 0;
                                    _ctx.tblP_Alerta.Add(objNuevaAlerta);
                                    _ctx.SaveChanges();
                                    #endregion //ALERTA SIGPLAN

                                    //AGREGAR SIGUIENTE NOTIFICANTE
                                    lstCorreosNotificarRestantes.Add(objUsuario.correo);

                                    //NOTIFICADA
                                    notifyNextAuth = false;
                                    totalAlertas++;
                                }

                                #endregion

                                if (item.autorizado == EstatusGestionAutorizacionEnum.AUTORIZADO)
                                {
                                    totalAuth++;

                                }
                                else
                                {
                                    if (sigAuth == null)
                                        sigAuth = item.FK_UsuarioAutorizacion;

                                    if (sigAuth.Value == vSesiones.sesionUsuarioDTO.id)
                                    {
                                        esSigAuth = true;
                                        break;
                                    }
                                    else
                                        esSigAuth = false;

                                    if (item.FK_UsuarioAutorizacion == vSesiones.sesionUsuarioDTO.id && esSigAuth)
                                    {
                                        totalAuth++;
                                        notifyNextAuth = true;
                                        esAutorizar = true;

                                        var objAlerta = _ctx.tblP_Alerta.FirstOrDefault(e => e.userRecibeID == item.FK_UsuarioAutorizacion && e.visto == false && e.objID == objReporte.id && e.obj == "AutorizacionReporteTabulador");
                                        if (objAlerta != null)
                                        {
                                            objAlerta.visto = true;
                                            _ctx.SaveChanges();
                                        }

                                        item.autorizado = EstatusGestionAutorizacionEnum.AUTORIZADO;
                                        item.fechaFirma = DateTime.Now;
                                        item.fechaModificacion = DateTime.Now;
                                        item.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                        _ctx.SaveChanges();
                                    }
                                }
                            }
                            #endregion

                            #region CUERPO CORREO
                            string cuerpo =
                                        @"<html>
                                            <head>
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
                                                        background-color: #ffcc5c;
                                                    }
                                                </style>
                                            </head>
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>

                                                    <p class=MsoNormal>
                                                        Buen día,<br><br>" +
                                                                (totalAuth == lstFirmas.Count() ? "Se ha autorizado un Reporte de Tabuladores por todos los firmantes" : "Se ha autorizado un Reporte de Tabuladores")
                                                                + @".<br><br>
                                                    </p>
                                                    <p class=MsoNormal style='font-weight:bold;'>" +
                                                        "LINEA DE NEGOCIOS: " + objLN.concepto + @".<o:p></o:p>
                                                    </p>
                                                    <br><br><br>
                                            ";

                            #region PROBABLEMENTE LO PIDEN

                            //                    var lstGrpTabuladoresDet = lstTabuladoresDet.GroupBy(e => e.FK_LineaNegocio).ToList();

                            //                    foreach (var item in lstGrpTabuladoresDet)
                            //                    {
                            //                        cuerpo += @"
                            //                                            <table>
                            //                                                <thead>
                            //                                                    <tr>
                            //                                                        <th>Linea Negocio</th>
                            //                                                        <th>Categoria</th>
                            //                                                        <th>Sueldo Base</th>
                            //                                                        <th>Complemento</th>
                            //                                                        <th>Total Nominal</th>
                            //                                                        <th>Sueldo Mensual</th>
                            //                                                        <th>Esquema de Pago</th>
                            //                                                    </tr>
                            //                                                </thead>
                            //                                                <tbody>
                            //                                                ";

                            //                        foreach (var itemDet in item)
                            //                        {
                            //                            cuerpo += "<tr>" +
                            //                                "<td>" + lstLineaNegocio.FirstOrDefault(e => e.id == itemDet.FK_LineaNegocio).concepto + "</td>" +
                            //                                "<td>" + lstCategorias.FirstOrDefault(e => e.id == itemDet.FK_Categoria).concepto + "</td>" +
                            //                                "<td>" + itemDet.sueldoBase.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                            //                                "<td>" + itemDet.complemento.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                            //                                "<td>" + itemDet.totalNominal.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                            //                                "<td>" + itemDet.sueldoMensual.ToString("C", CultureInfo.CreateSpecificCulture("es-MX")) + "</td>" +
                            //                                "<td>" + lstEsquemaPago.FirstOrDefault(e => e.id == itemDet.FK_EsquemaPago).concepto + "</td>" +
                            //                            "</tr>";
                            //                        }

                            //                        cuerpo += "</tbody>" +
                            //                                  "</table>" +
                            //                                  "<br><br><br>";
                            //                    }
                            #endregion

                            #region TABLA AUTORIZANTES

                            cuerpo += @"
                                        <table>
                                            <thead>
                                                <tr>
                                                    <th>Nombre</th>
                                                    <th>Tipo</th>
                                                    <th>Autorizo</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            ";

                            bool esAuth = false;
                            int totalSiguientes = 0;

                            //EstatusGestionAutorizacionEnum P 0 A 1 R 2 
                            foreach (var itemDet in lstFirmas)
                            {
                                tblP_Usuario objUsuario = _ctx.tblP_Usuario.FirstOrDefault(e => e.id == itemDet.FK_UsuarioAutorizacion);
                                string nombreCompleto = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                                cuerpo += "<tr>" +
                                            "<td>" + nombreCompleto + "</td>" +
                                            "<td>" + EnumHelper.GetDescription(itemDet.nivelAutorizante) + "</td>" +
                                            GetEstatusTabulador((int)itemDet.autorizado, esAuth) +
                                        "</tr>";

                                if (vSesiones.sesionUsuarioDTO.id == itemDet.FK_UsuarioAutorizacion && totalSiguientes == 0)
                                {
                                    esAuth = true;
                                    totalSiguientes++;
                                }
                                else
                                {
                                    if (esAuth)
                                    {
                                        esAuth = false;

                                        if (itemDet.autorizado == EstatusGestionAutorizacionEnum.AUTORIZADO)
                                        {
                                            esAuth = true;
                                            totalSiguientes = 0;
                                        }
                                    }
                                }
                            }

                            cuerpo += "</tbody>" +
                                        "</table>" +
                                        "<br><br><br>";


                            #endregion

                            cuerpo += "<br><br><br>" +
                                  "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                                  "Capital Humano > Administración de personal > Configuración > Estructura Salarial > Gestión de Reportes.<br><br>" +
                                  "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                                  "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias." +
                                " </body>" +
                              "</html>";

                            #endregion

                            if (totalAuth == lstFirmas.Count())
                            {
                                //esAuthCompleta = true;
                                objReporte.estatus = EstatusGestionAutorizacionEnum.AUTORIZADO;
                                objReporte.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                                objReporte.fechaModificacion = DateTime.Now;
                                _ctx.SaveChanges();

                                lstCorreosNotificarTodos.Add(_CORREO_DIANA_ALVAREZ);
                                lstCorreosNotificarTodos.Add(_CORREO_MANUEL_CRUZ);
#if DEBUG
                                lstCorreosNotificarTodos = new List<string>();
                                lstCorreosNotificarTodos.Add(_CORREO_OMAR_NUNEZ);
                                //lstCorreosNotificarTodos.Add("miguel.buzani@construplan.com.mx");
#endif
                                if (esAutorizar)
                                {
                                    GlobalUtils.sendEmail(string.Format("{0}: REPORTE TABULADORES AUTORIZADO POR TODOS LOS FIRMANTES EN LA LINEA DE NEGOCIOS: [{1}]", PersonalUtilities.GetNombreEmpresa(), objLN.concepto),
                                                           cuerpo, lstCorreosNotificarTodos);
                                }

                            }
                            else
                            {
                                #region CORREO ESTATUS
                                lstCorreosNotificarRestantes.Add(_CORREO_DIANA_ALVAREZ);
#if DEBUG
                                lstCorreosNotificarRestantes = new List<string>();
                                //lstCorreosNotificarRestantes.Add(_CORREO_OMAR_NUNEZ);
                                //lstCorreosNotificarRestantes.Add("miguel.buzani@construplan.com.mx");
#endif
                                if (esAutorizar)
                                {
                                    GlobalUtils.sendEmail(string.Format("{0}: REPORTE TABULADORES AUTORIZADO EN LA LINEA DE NEGOCIOS: [{1}]", PersonalUtilities.GetNombreEmpresa(), objLN.concepto),
                                                        cuerpo, lstCorreosNotificarRestantes);
                                }

                                #endregion
                            }
                        }

                        dbContextTransaction.Commit();
                        resultado.Clear();
                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Clear();
                        resultado.Add(MESSAGE, e.Message);
                        resultado.Add(SUCCESS, false);
                    }
                }
            }

            return resultado;
        }
        #endregion

        #region ACCESO MENU
        public Dictionary<string, object> GetAccesosMenu()
        {
            resultado.Clear();
            try
            {
                #region SE OBTIENE LOS MENUS A LOS QUE PUEDE ACCEDER EL USUARIO
                List<tblRH_TAB_AccesosMenu> lstAccesos = _context.tblRH_TAB_AccesosMenu.Where(w => w.FK_Usuario == (int)vSesiones.sesionUsuarioDTO.id && w.registroActivo).ToList();
                List<int> lstAccesosDTO = lstAccesos.Select(s => s.FK_Menu).ToList();

                tblP_Usuario objUsuario = _context.tblP_Usuario.Where(w => w.id == (int)vSesiones.sesionUsuarioDTO.id && w.estatus).FirstOrDefault();
                if (objUsuario.perfilID == 1)
                {
                    for (int i = 0; i <= 9; i++)
                    {
                        lstAccesosDTO.Add(i);
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("lstAccesosDTO", lstAccesosDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetAccesosMenu", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public bool VerificarAcceso(int FK_Menu)
        {
            bool tieneAcceso = false;
            try
            {
                #region SE VERIFICA SI EL USUARIO TIENE ACCESO A LA VISTA SOLICITADA
                tblP_Usuario objUsuario = _context.tblP_Usuario.Where(w => w.id == (int)vSesiones.sesionUsuarioDTO.id && w.estatus).FirstOrDefault();

                tblRH_TAB_AccesosMenu objAccesoMenu = _context.tblRH_TAB_AccesosMenu.Where(w => w.FK_Usuario == (int)vSesiones.sesionUsuarioDTO.id && w.FK_Menu == FK_Menu && w.registroActivo).FirstOrDefault();
                if (objAccesoMenu != null)
                    tieneAcceso = true;
                else if (objUsuario.perfilID == 1)
                    tieneAcceso = true;
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "VerificarAcceso", e, AccionEnum.CONSULTA, 0, 0);
                return false;
            }
            return tieneAcceso;
        }
        #endregion

        #region GENERALES
        public Dictionary<string, object> FillCboPuestos()
        {
            resultado.Clear();
            try
            {
                #region FILL COMBO PUESTOS
                List<tblRH_EK_Puestos> lstPuestos = _context.Select<tblRH_EK_Puestos>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT * FROM tblRH_EK_Puestos
	                                    WHERE descripcion NOT LIKE '%NO USAR%' AND descripcion NOT LIKE '%(NO US)%' AND descripcion NOT LIKE '%(NO USA)%' AND puesto != 65 AND registroActivo = @registroActivo
		                                    ORDER BY descripcion",
                    parametros = new { registroActivo = true }
                }).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstPuestos)
                {
                    if (item.puesto > 0 && !string.IsNullOrEmpty(item.descripcion))
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = item.puesto.ToString();
                        objComboDTO.Text = string.Format("[{0}] {1}", item.puesto, item.descripcion.Trim());
                        objComboDTO.Prefijo = item.descripcion.Trim();
                        lstComboDTO.Add(objComboDTO);
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboPuestos", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Clear();
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetInformacionPuesto(PuestoDTO objFiltroDTO)
        {
            resultado.Clear();
            try
            {
                #region VALIDACIONES
                string mensajeError = "Ocurrió un error al obtener la información del puesto.";
                if (objFiltroDTO.puesto <= 0) { throw new Exception(mensajeError); }
                #endregion

                #region SE OBTIENE LA INFORMACIÓN DEL PUESTO
                tblRH_EK_Puestos objPuesto = _context.tblRH_EK_Puestos.Where(w => w.puesto == objFiltroDTO.puesto && w.registroActivo).FirstOrDefault();
                if (objPuesto == null)
                    throw new Exception(mensajeError);

                //if (objPuesto.FK_AreaDepartamento <= 0) { throw new Exception("El puesto no cuenta con área/departamento asignado, favor de asignarlo para poder continuar."); }
                if (objPuesto.FK_TipoNomina <= 0) { throw new Exception("El puesto no cuenta con el tipo de nómina asignado, favor de asignarlo para poder continuar."); }
                //if (objPuesto.FK_Sindicato <= 0) { throw new Exception("El puesto no cuenta con el estatus de sindicato asignado, favor de asignarlo para poder continuar."); }
                //if (objPuesto.FK_NivelMando <= 0) { throw new Exception("El puesto no cuenta con nivel de mando asignado, favor de asignarlo para poder continuar."); }

                PuestoDTO objPuestoDTO = new PuestoDTO();
                objPuestoDTO.puesto = objPuesto.puesto;
                objPuestoDTO.FK_AreaDepartamento = objPuesto.FK_AreaDepartamento;
                objPuestoDTO.FK_TipoNomina = objPuesto.FK_TipoNomina;
                objPuestoDTO.FK_Sindicato = objPuesto.FK_Sindicato;
                objPuestoDTO.FK_NivelMando = objPuesto.FK_NivelMando;
                objPuestoDTO.nivelMando = _context.tblRH_TAB_CatNivelMando.Where(w => w.id == objPuesto.FK_NivelMando && w.registroActivo).Select(s => s.nivel).FirstOrDefault();

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("objPuestoDTO", objPuestoDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetInformacionPuesto", e, AccionEnum.CONSULTA, objFiltroDTO.puesto, objFiltroDTO);
                resultado.Clear();
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboCategorias()
        {
            resultado.Clear();
            try
            {
                #region FILL COMBO CATEGORIAS
                List<tblRH_TAB_CatCategorias> lstCategorias = _context.tblRH_TAB_CatCategorias.Where(w => w.registroActivo).ToList();

                int contador = 1;
                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstCategorias)
                {
                    if (contador <= 27)
                    {
                        if (item.id > 0 && !string.IsNullOrEmpty(item.concepto))
                        {
                            objComboDTO = new ComboDTO();
                            objComboDTO.Value = item.id.ToString();
                            objComboDTO.Text = item.concepto.Trim();
                            objComboDTO.Prefijo = item.concepto.Trim();
                            lstComboDTO.Add(objComboDTO);
                            contador++;
                        }
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboCategorias", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboEsquemaPagos()
        {
            resultado.Clear();
            try
            {
                #region FILL COMBO CATEGORIAS
                List<tblRH_TAB_CatEsquemaPago> lstEsquemaPagos = _context.tblRH_TAB_CatEsquemaPago.Where(w => w.registroActivo).OrderBy(o => o.concepto).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstEsquemaPagos)
                {
                    if (item.id > 0 && !string.IsNullOrEmpty(item.concepto))
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = item.id.ToString();
                        objComboDTO.Text = item.concepto.Trim();
                        objComboDTO.Prefijo = item.concepto.Trim();
                        lstComboDTO.Add(objComboDTO);
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboEsquemaPagos", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboLineaNegocios(TabuladorDTO objParamDTO)
        {
            resultado.Clear();
            try
            {
                #region FILL CBO
                List<tblRH_TAB_CatLineaNegocio> lstCatLineaNegocios = _context.tblRH_TAB_CatLineaNegocio.Where(w => w.registroActivo).OrderBy(o => o.concepto).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstCatLineaNegocios)
                {
                    if (item.id > 0 && !string.IsNullOrEmpty(item.concepto))
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = item.id.ToString();
                        objComboDTO.Text = item.concepto.Trim();
                        lstComboDTO.Add(objComboDTO);
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboLineaNegocios", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Clear();
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboAreasDepartamentos()
        {
            resultado.Clear();
            try
            {
                #region FILL CBO
                List<tblRH_TAB_CatAreaDepartamento> lstAreasDepartamentos = _context.tblRH_TAB_CatAreaDepartamento.Where(w => w.registroActivo).OrderBy(o => o.concepto).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstAreasDepartamentos)
                {
                    if (item.id > 0 && !string.IsNullOrEmpty(item.concepto))
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = item.id.ToString();
                        objComboDTO.Text = item.concepto.Trim();
                        lstComboDTO.Add(objComboDTO);
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboAreasDepartamentos", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Clear();
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboTipoNomina()
        {
            resultado.Clear();
            try
            {
                #region FILL CBO
                List<tblRH_EK_Tipos_Nomina> lstTipoNominas = _context.tblRH_EK_Tipos_Nomina.Where(w => w.esActivo == true).OrderBy(o => o.descripcion).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstTipoNominas)
                {
                    if (item.tipo_nomina > 0 && !string.IsNullOrEmpty(item.descripcion))
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = item.tipo_nomina.ToString();
                        objComboDTO.Text = item.descripcion.Trim();
                        lstComboDTO.Add(objComboDTO);
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboTipoNomina", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Clear();
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboSindicatos()
        {
            resultado.Clear();
            try
            {
                #region FILL CBO
                List<tblRH_TAB_CatSindicato> lstCatSindicatos = _context.tblRH_TAB_CatSindicato.Where(w => w.registroActivo).OrderBy(o => o.concepto).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstCatSindicatos)
                {
                    if (item.id > 0 && !string.IsNullOrEmpty(item.concepto))
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = item.id.ToString();
                        objComboDTO.Text = item.concepto.Trim();
                        lstComboDTO.Add(objComboDTO);
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboSindicatos", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Clear();
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboNivelMando()
        {
            resultado.Clear();
            try
            {
                #region FILL CBO
                List<tblRH_TAB_CatNivelMando> lstCatNivelMando = _context.tblRH_TAB_CatNivelMando.Where(w => w.registroActivo).OrderBy(o => o.concepto).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstCatNivelMando)
                {
                    if (item.id > 0 && !string.IsNullOrEmpty(item.concepto))
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = item.id.ToString();
                        objComboDTO.Text = item.concepto.Trim();
                        lstComboDTO.Add(objComboDTO);
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboNivelMando", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Clear();
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboUsuarios()
        {
            resultado.Clear();
            try
            {
                #region FILL CBO
                List<tblP_Usuario> lstUsuarios = _context.tblP_Usuario.Where(w => w.estatus).OrderBy(o => o.nombre).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                string nombreCompleto = string.Empty;
                foreach (var item in lstUsuarios)
                {
                    nombreCompleto = string.Empty;
                    if (!string.IsNullOrEmpty(item.nombre))
                        nombreCompleto = item.nombre.Trim();
                    if (!string.IsNullOrEmpty(item.apellidoPaterno))
                        nombreCompleto += string.Format(" {0}", item.apellidoPaterno.Trim());
                    if (!string.IsNullOrEmpty(item.apellidoMaterno))
                        nombreCompleto += string.Format(" {0}", item.apellidoMaterno.Trim());

                    if (!string.IsNullOrEmpty(nombreCompleto))
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = item.id.ToString();
                        objComboDTO.Text = nombreCompleto;
                        lstComboDTO.Add(objComboDTO);
                    }
                }

                resultado.Clear();
                resultado.Add(ITEMS, lstComboDTO);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboUsuarios", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Clear();
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboCC(List<int> lstFK_LineaNegocio)
        {
            resultado.Clear();
            try
            {
                #region FILL COMBO CC CON LINEA DE NEGOCIO
                List<string> lstCC_LineaNegocio = _context.tblRH_TAB_CatLineaNegocioDet.Where(w => (lstFK_LineaNegocio.Count() > 0 ? lstFK_LineaNegocio.Contains(w.FK_LineaNegocio) : true) && w.registroActivo).Select(s => s.cc).ToList();
                List<tblC_Nom_CatalogoCC> lstCC = _context.tblC_Nom_CatalogoCC.Where(w => lstCC_LineaNegocio.Contains(w.cc) && w.estatus).OrderBy(o => o.cc).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstCC)
                {
                    if (!string.IsNullOrEmpty(item.cc) && !string.IsNullOrEmpty(item.ccDescripcion))
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = item.cc.ToString();
                        objComboDTO.Text = string.Format("[{0}] {1}", item.cc.Trim(), item.ccDescripcion.Trim());
                        lstComboDTO.Add(objComboDTO);
                    }
                }

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboFiltroCC", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Clear();
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboGestionEstatus()
        {
            resultado.Clear();
            try
            {
                #region FILL CBO
                List<ComboDTO> lstComboDTO = new List<ComboDTO>();

                // PENDIENTE
                int estatusGestion = (int)EstatusGestionAutorizacionEnum.PENDIENTE;
                ComboDTO objComboDTO = new ComboDTO();
                objComboDTO.Value = estatusGestion.ToString();
                objComboDTO.Text = EnumHelper.GetDescription((EstatusGestionAutorizacionEnum.PENDIENTE));
                lstComboDTO.Add(objComboDTO);

                // AUTORIZADO
                estatusGestion = (int)EstatusGestionAutorizacionEnum.AUTORIZADO;
                objComboDTO = new ComboDTO();
                objComboDTO.Value = estatusGestion.ToString();
                objComboDTO.Text = EnumHelper.GetDescription((EstatusGestionAutorizacionEnum.AUTORIZADO));
                lstComboDTO.Add(objComboDTO);

                // RECHAZADO
                estatusGestion = (int)EstatusGestionAutorizacionEnum.RECHAZADO;
                objComboDTO = new ComboDTO();
                objComboDTO.Value = estatusGestion.ToString();
                objComboDTO.Text = EnumHelper.GetDescription((EstatusGestionAutorizacionEnum.RECHAZADO));
                lstComboDTO.Add(objComboDTO);

                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "FillCboGestionEstatus", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Clear();
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public string SetPuestoEmpleado(tblRH_EK_Puestos objPuesto)
        {
            string puestoEmpleado = string.Empty;
            try
            {
                #region SE OBTIENE EL ID Y NOMBRE DEL PUESTO
                if (objPuesto != null)
                {
                    if (objPuesto.puesto > 0 && !string.IsNullOrEmpty(objPuesto.descripcion))
                        puestoEmpleado = string.Format("[{0}] {1}", objPuesto.puesto, objPuesto.descripcion.Trim());
                }
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "SetPuestoEmpleado", e, AccionEnum.CONSULTA, objPuesto.puesto, objPuesto);
                return puestoEmpleado;
            }
            return puestoEmpleado;
        }

        public DataTable getInfoEnca(string nombreReporte, string area)
        {
            string RutaServidor = "";

#if DEBUG
            RutaServidor = @"C:\Proyectos\SIGOPLANv2\ENCABEZADOS";
#else
            RutaServidor = @"\\10.1.0.112\Proyecto\SIGOPLAN\ENCABEZADOS";
#endif

            DataTable tableEncabezado = new DataTable();

            tableEncabezado.Columns.Add("logo", System.Type.GetType("System.Byte[]"));
            tableEncabezado.Columns.Add("nombreEmpresa", System.Type.GetType("System.String"));
            tableEncabezado.Columns.Add("nombreReporte", System.Type.GetType("System.String"));
            tableEncabezado.Columns.Add("area", System.Type.GetType("System.String"));

            var data = encabezadoFactoryServices.getEncabezadoDatos();
            string path = data.logo;
            //string path = Path.Combine(RutaServidor, Path.GetFileName(data.logo));
            byte[] imgdata = System.IO.File.ReadAllBytes(HostingEnvironment.MapPath(path));
            string empresa = data.nombreEmpresa;

            tableEncabezado.Rows.Add(imgdata, empresa, nombreReporte, area);

            return tableEncabezado;
        }

        public List<string> GetDescLineaNegocio(List<int> lstLineasNegocio)
        {
            var lstDesc = new List<string>();

            try
            {
                if (lstLineasNegocio != null && lstLineasNegocio.Count > 0)
                {
                    lstDesc = _context.tblRH_TAB_CatLineaNegocio.Where(e => lstLineasNegocio.Contains(e.id)).Select(e => e.concepto.Trim()).ToList();

                }
            }
            catch (Exception e)
            {

                throw e;
            }

            return lstDesc;
        }

        public List<string> GetDescCCs(List<string> lstCCs)
        {
            var lstDesc = new List<string>();
            try
            {
                if (lstCCs != null && lstCCs.Count > 0)
                {
                    if (lstCCs.Count > 13)
                    {
                        lstDesc = _context.tblC_Nom_CatalogoCC.Where(e => lstCCs.Contains(e.cc)).Select(e => (e.cc.Trim() + " - " + e.ccDescripcion.Trim())).ToList().Take(13).ToList();
                        lstDesc.Add("etc.");
                    }
                    else
                        lstDesc = _context.tblC_Nom_CatalogoCC.Where(e => lstCCs.Contains(e.cc)).Select(e => (e.cc.Trim() + " - " + e.ccDescripcion.Trim())).ToList();
                }
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
            }

            return lstDesc;
        }

        public List<RepAutorizantesTABDTO> GetAutorizantesReporte(List<GestionAutorizanteDTO> lstGestionAutorizantesDTO)
        {
            List<RepAutorizantesTABDTO> lstRepAuth = new List<RepAutorizantesTABDTO>();

            try
            {
                if (lstGestionAutorizantesDTO != null)
                {
                    foreach (var item in lstGestionAutorizantesDTO)
                    {
                        var objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == item.FK_UsuarioAutorizacion);
                        string nombreCompletoAuth = "";

                        if (objUsuario != null)
                        {
                            nombreCompletoAuth = objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
                        }

                        lstRepAuth.Add(new RepAutorizantesTABDTO
                        {
                            autorizado = EnumHelper.GetDescription(item.autorizado),
                            puestoAutorizante = EnumHelper.GetDescription(item.nivelAutorizante),
                            nombreAutorizante = nombreCompletoAuth,
                            firma = item.fechaFirma.HasValue ? GlobalUtils.CrearFirmaDigitalConFecha(item.FK_Registro, DocumentosEnum.TabuladoresPuestos, item.FK_UsuarioAutorizacion, item.fechaFirma.Value) : ""
                        });
                    }
                }

            }
            catch (Exception e)
            {

                throw e;
            }

            return lstRepAuth;

        }

        public Dictionary<string, object> GetFechaActual()
        {
            resultado.Clear();
            try
            {
                #region SE OBTIENE LA FECHA ACTUAL
                resultado.Clear();
                resultado.Add(SUCCESS, true);
                resultado.Add("fechaActual", DateTime.Now);
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetFechaActual", e, AccionEnum.CONSULTA, 0, null);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        private string SetNombrePuesto(int FK_Puesto, List<tblRH_EK_Puestos> lstPuestos)
        {
            string puestoDesc = string.Empty;
            try
            {
                #region SE OBTIENE LA DESCRIPCIÓN DEL PUESTO
                tblRH_EK_Puestos objPuesto = lstPuestos.Where(w => w.puesto == FK_Puesto && w.registroActivo).FirstOrDefault();
                if (objPuesto == null)
                    throw new Exception("Ocurrió un error al obtener la descripción del puesto.");

                puestoDesc = string.Format("[{0}] {1}", objPuesto.puesto, objPuesto.descripcion.Trim());
                #endregion
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "", e, AccionEnum.CONSULTA, 0, null);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return puestoDesc;
        }

        private tblRH_EK_Empleados GetNombreEmpleado(int clave_empleado)
        {
            tblRH_EK_Empleados objEmpleado = new tblRH_EK_Empleados();
            try
            {
                objEmpleado = _context.tblRH_EK_Empleados.Where(w => w.clave_empleado == clave_empleado).FirstOrDefault();
                if (objEmpleado == null)
                    throw new Exception("Ocurrió un error al obtener el nombre del empleado.");
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, "GetNombreEmpleado", e, AccionEnum.CONSULTA, 0, new { clave_empleado = clave_empleado });
            }
            return objEmpleado;
        }

        private dynamic consultaCheckProductivo(string consulta)
        {
            return _contextEnkontrol.WhereComprasOrigen(consulta);
        }

        private EstatusGestionAutorizacionEnum VerificarEstatusTabulador(int FK_Tabulador, List<int> lstFK_TabuladorDet, List<tblRH_TAB_Tabuladores> lstTabuladoresInfo)
        {
            EstatusGestionAutorizacionEnum estatusTabulador = EstatusGestionAutorizacionEnum.AUTORIZADO;
            try
            {
                // SE VERIFICA SI EL TABULADOR ESTA AUTORIZADO
                string strQuery = string.Empty;
                if (lstTabuladoresInfo.Any(w => w.id == FK_Tabulador && w.tabuladorAutorizado == EstatusGestionAutorizacionEnum.PENDIENTE && w.registroActivo))
                    strQuery = string.Format(@"SELECT autorizado FROM tblRH_TAB_GestionAutorizantes WHERE FK_Registro = @FK_Tabulador AND registroActivo = @registroActivo");
                else
                {
                    strQuery = string.Format(@"SELECT autorizado FROM tblRH_TAB_GestionAutorizantes WHERE FK_Registro = @FK_Tabulador {0} AND registroActivo = @registroActivo",
                                                lstFK_TabuladorDet.Count() > 0 ? string.Format("AND FK_TabuladorDet IN ({0})", string.Join(",", lstFK_TabuladorDet)) : string.Empty);
                }

                List<int> lstEstatusGestion = _context.Select<int>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { FK_Tabulador = FK_Tabulador, registroActivo = true }
                }).ToList();

                foreach (var itemEstatusGestion in lstEstatusGestion)
                {
                    if (itemEstatusGestion == (int)EstatusGestionAutorizacionEnum.RECHAZADO)
                    {
                        estatusTabulador = EstatusGestionAutorizacionEnum.RECHAZADO;
                        break;
                    }
                    else if (itemEstatusGestion == (int)EstatusGestionAutorizacionEnum.PENDIENTE)
                    {
                        estatusTabulador = EstatusGestionAutorizacionEnum.PENDIENTE;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, new { FK_Tabulador = FK_Tabulador, lstFK_TabuladorDet = lstFK_TabuladorDet });
                estatusTabulador = EstatusGestionAutorizacionEnum.PENDIENTE;
            }
            return estatusTabulador;
        }

        private List<CategoriaDTO> GetListaCategorias(CategoriaDTO objParamsDTO = null)
        {
            List<CategoriaDTO> lstCategoriasDTO = new List<CategoriaDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = @"SELECT * FROM tblRH_TAB_CatCategorias WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                #endregion

                #region CONSULTA
                lstCategoriasDTO = _context.Select<CategoriaDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstCategoriasDTO;
        }

        private List<LineaNegocioDTO> GetListaLineasNegocios(LineaNegocioDTO objParamsDTO = null)
        {
            List<LineaNegocioDTO> lstLineasNegociosDTO = new List<LineaNegocioDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = @"SELECT * FROM tblRH_TAB_CatLineaNegocio WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                #endregion

                #region CONSULTA
                lstLineasNegociosDTO = _context.Select<LineaNegocioDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstLineasNegociosDTO;
        }

        private List<EsquemaPagoDTO> GetListaEsquemasPagos(EsquemaPagoDTO objParamsDTO = null)
        {
            List<EsquemaPagoDTO> lstEsquemasPagosDTO = new List<EsquemaPagoDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = @"SELECT * FROM tblRH_TAB_CatEsquemaPago WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                #endregion

                #region CONSULTA
                lstEsquemasPagosDTO = _context.Select<EsquemaPagoDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstEsquemasPagosDTO;
        }

        private List<TabuladorDetDTO> GetListaTabuladoresDet(TabuladorDetDTO objParamsDTO = null)
        {
            List<TabuladorDetDTO> lstTabuladoresDetDTO = new List<TabuladorDetDTO>();
            try
            {
                #region QUERY Y CONDICIONES
                string strQuery = "SELECT * FROM tblRH_TAB_TabuladoresDet WHERE registroActivo = @registroActivo";

                if (objParamsDTO != null)
                {
                    strQuery += objParamsDTO.id > 0 ? string.Format(" AND id = {0}", objParamsDTO.id) : string.Empty;
                    strQuery += objParamsDTO.FK_LineaNegocio > 0 ? string.Format(" AND FK_LineaNegocio = {0}", objParamsDTO.FK_LineaNegocio) : string.Empty;
                }
                #endregion

                #region CONSULTA
                lstTabuladoresDetDTO = _context.Select<TabuladorDetDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).ToList();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, objParamsDTO.id, objParamsDTO);
            }
            return lstTabuladoresDetDTO;
        }
        #endregion
    }
}