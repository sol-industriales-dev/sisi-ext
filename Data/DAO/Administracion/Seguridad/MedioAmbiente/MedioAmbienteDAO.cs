using Core.DAO.Administracion.Seguridad.MedioAmbiente;
using Core.DTO;
using Core.DTO.Administracion.Seguridad.MedioAmbiente;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Seguridad.MedioAmbiente;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Utils;
using MoreLinq;
using Core.Enum.Multiempresa;
using Core.DTO.Utils.Data;
using Data.EntityFramework.Context;
using System.IO;
using System.Web;
using Core.Enum.Principal;
using Core.Enum.Administracion.Seguridad.MedioAmbiente;
using Core.Enum;
using Core.Entity.Administrativo.Seguridad.Indicadores;

namespace Data.DAO.Administracion.Seguridad.MedioAmbiente
{
    public class MedioAmbienteDAO : GenericDAO<tblS_MedioAmbienteAspectoAmbiental>, IMedioAmbienteDAO
    {
        #region VARIABLES GLOBALES Y RUTAS DE CARPETAS
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        private const string NombreControlador = "MedioAmbienteController";
        private const string NombreBaseEvidencia = @"Evidencia";
        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\SEGURIDAD_MEDIO_AMBIENTE";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\SEGURIDAD_MEDIO_AMBIENTE";
        private readonly string RutaEvidencia;
        private const MainContextEnum idEmpresaActual = MainContextEnum.Construplan;
        #endregion

        #region CONSTRUCTOR
        public MedioAmbienteDAO()
        {
            resultado.Clear();

#if DEBUG
            RutaBase = RutaLocal;
#endif

            RutaEvidencia = Path.Combine(RutaBase, "EVIDENCIA");
        }
        #endregion

        #region ASPECTOS AMBIENTALES
        public Dictionary<string, object> getAspectosAmbientales(int tipoCaptura = 0)
        {
            #region SE OBTIENE LOS ASPECTOS AMBIENTALES
            try
            {
                var listaClasificaciones = _context.tblS_MedioAmbienteClasificacion.Where(x => x.esActivo).ToList();
                var listaFactoresPeligro = _context.tblS_MedioAmbienteResiduoFactorPeligro.Where(x => x.esActivo).ToList().Select(x => new ResiduoFactorPeligroDTO
                {
                    id = x.id,
                    factorPeligro = x.factorPeligro,
                    factorPeligroDesc = x.factorPeligro.GetDescription(),
                    residuoID = x.residuoID
                }).ToList();

                List<int> lstTipoAspectosAmbientales = new List<int>();
                switch (tipoCaptura)
                {
                    case 1:
                        lstTipoAspectosAmbientales.Add(1);
                        break;
                    case 2:
                        lstTipoAspectosAmbientales.Add(2);
                        lstTipoAspectosAmbientales.Add(3);
                        break;
                    case 3:
                        lstTipoAspectosAmbientales.Add(4);
                        lstTipoAspectosAmbientales.Add(5);
                        break;
                    default:
                        break;
                }

                var data = _context.tblS_MedioAmbienteAspectoAmbiental.Where(x => (lstTipoAspectosAmbientales.Count() > 0 ? lstTipoAspectosAmbientales.Contains(x.clasificacion) : true) && x.esActivo).ToList().Select(x => new AspectoAmbientalDTO
                {
                    id = x.id,
                    descripcion = x.descripcion,
                    peligroso = x.peligroso,
                    unidad = x.unidad,
                    unidadDesc = x.unidad.GetDescription(),
                    clasificacion = x.clasificacion,
                    clasificacionDesc = listaClasificaciones.Where(y => y.id == x.clasificacion).Select(z => z.descripcion).FirstOrDefault(),
                    factoresPeligro = listaFactoresPeligro.Where(y => y.residuoID == x.id).ToList(),
                    esSolidoImpregnadoHidrocarburo = x.esSolidoImpregnadoHidrocarburo
                }).ToList();

                var listaCombo = data.Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.descripcion,
                    Prefijo = EnumHelper.GetDescription((UnidadEnum)x.unidad).ToString()
                });

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, listaCombo);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "getAspectosAmbientales", e, AccionEnum.CONSULTA, 0, tipoCaptura);
            }
            return resultado;
            #endregion
        }

        public Dictionary<string, object> guardarNuevoAspectoAmbiental(tblS_MedioAmbienteAspectoAmbiental aspectoAmbiental)
        {
            #region GUARDAR ASPECTO AMBIENTAL
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                    DateTime fechaCreacion = DateTime.Now;

                    if (aspectoAmbiental.factoresPeligro != null)
                    {
                        foreach (var fac in aspectoAmbiental.factoresPeligro)
                        {
                            fac.idUsuarioCreacion = idUsuarioCreacion;
                            fac.idUsuarioModificacion = 0;
                            fac.fechaCreacion = fechaCreacion;
                            fac.fechaModificacion = new DateTime(2000, 01, 01);
                            fac.esActivo = true;
                        }
                    }

                    aspectoAmbiental.idUsuarioCreacion = idUsuarioCreacion;
                    aspectoAmbiental.fechaCreacion = DateTime.Now;
                    aspectoAmbiental.esActivo = true;

                    _context.tblS_MedioAmbienteAspectoAmbiental.Add(aspectoAmbiental);
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "guardarNuevoAspectoAmbiental", e, AccionEnum.AGREGAR, aspectoAmbiental.id, aspectoAmbiental);
                }
            }
            return resultado;
            #endregion
        }

        public Dictionary<string, object> editarAspectoAmbiental(tblS_MedioAmbienteAspectoAmbiental aspectoAmbiental)
        {
            #region EDITAR ASPECTO AMBIENTAL
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    tblS_MedioAmbienteAspectoAmbiental aspectoAmbientalSIGOPLAN = _context.tblS_MedioAmbienteAspectoAmbiental.FirstOrDefault(x => x.id == aspectoAmbiental.id);

                    if (aspectoAmbientalSIGOPLAN == null)
                        throw new Exception("Ocurrió un error al actualizar la información del registro seleccionado.");

                    aspectoAmbientalSIGOPLAN.descripcion = aspectoAmbiental.descripcion;
                    aspectoAmbientalSIGOPLAN.peligroso = aspectoAmbiental.peligroso;
                    aspectoAmbientalSIGOPLAN.unidad = aspectoAmbiental.unidad;
                    aspectoAmbientalSIGOPLAN.clasificacion = aspectoAmbiental.clasificacion;
                    aspectoAmbientalSIGOPLAN.esSolidoImpregnadoHidrocarburo = aspectoAmbiental.esSolidoImpregnadoHidrocarburo;
                    aspectoAmbientalSIGOPLAN.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    aspectoAmbientalSIGOPLAN.fechaModificacion = DateTime.Now;
                    _context.SaveChanges();

                    #region Editar factores de peligro.
                    List<tblS_MedioAmbienteResiduoFactorPeligro> listaFactoresAnteriores = _context.tblS_MedioAmbienteResiduoFactorPeligro.Where(x => x.esActivo && x.residuoID == aspectoAmbiental.id).ToList();

                    foreach (var fac in listaFactoresAnteriores)
                    {
                        fac.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        fac.fechaModificacion = DateTime.Now;
                        fac.esActivo = false;
                        _context.SaveChanges();
                    }

                    if (aspectoAmbiental.factoresPeligro != null)
                    {
                        foreach (var fac in aspectoAmbiental.factoresPeligro)
                        {
                            fac.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                            fac.fechaModificacion = DateTime.Now;
                            fac.esActivo = true;
                        }

                        if (aspectoAmbiental.factoresPeligro.Count() > 0)
                        {
                            _context.tblS_MedioAmbienteResiduoFactorPeligro.AddRange(aspectoAmbiental.factoresPeligro);
                            _context.SaveChanges();
                        }
                    }
                    #endregion

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "editarAspectoAmbiental", e, AccionEnum.ACTUALIZAR, aspectoAmbiental.id, aspectoAmbiental);
                }
            }

            return resultado;
            #endregion
        }

        public Dictionary<string, object> eliminarAspectoAmbiental(tblS_MedioAmbienteAspectoAmbiental aspectoAmbiental)
        {
            #region ELIMINAR ASPECTO AMBIENTAL
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    tblS_MedioAmbienteAspectoAmbiental aspectoAmbientalSIGOPLAN = _context.tblS_MedioAmbienteAspectoAmbiental.FirstOrDefault(x => x.id == aspectoAmbiental.id);

                    if (aspectoAmbientalSIGOPLAN == null)
                        throw new Exception("Ocurrió un error al eliminar el registro seleccionado.");

                    aspectoAmbientalSIGOPLAN.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    aspectoAmbientalSIGOPLAN.fechaModificacion = DateTime.Now;
                    aspectoAmbientalSIGOPLAN.esActivo = false;
                    _context.SaveChanges();

                    #region SE ELIMINAN LOS FACTORES DE PELIGRO EN CASO QUE EXISTAN
                    List<tblS_MedioAmbienteResiduoFactorPeligro> listaFactores = _context.tblS_MedioAmbienteResiduoFactorPeligro.Where(x => x.esActivo && x.residuoID == aspectoAmbiental.id).ToList();

                    foreach (var fac in listaFactores)
                    {
                        fac.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        fac.fechaModificacion = DateTime.Now;
                        fac.esActivo = false;
                        _context.SaveChanges();
                    }
                    #endregion

                    #region SE ELIMINAN LAS CAPTURAS RELACIONADAS A ESTE ASPECTO AMBIENTAL //TODO
                    //List<tblS_MedioAmbienteCaptura> listaCapturas = _context.tblS_MedioAmbienteCaptura.Where(x => x.esActivo && x.idAspectoAmbiental == aspectoAmbiental.id).ToList();

                    //foreach (var cap in listaCapturas)
                    //{
                    //    cap.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    //    cap.fechaModificacion = DateTime.Now;
                    //    cap.esActivo = false;
                    //    _context.SaveChanges();
                    //}
                    #endregion

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "eliminarAspectoAmbiental", e, AccionEnum.ELIMINAR, aspectoAmbiental.id, aspectoAmbiental);
                }
            }
            return resultado;
            #endregion
        }

        public Dictionary<string, object> getClasificacionCombo()
        {
            #region FILL COMBO CLASIFICACIONES
            try
            {
                List<ComboDTO> listaClasificaciones = _context.tblS_MedioAmbienteClasificacion.Where(x => x.esActivo).Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.descripcion
                }).ToList();

                resultado.Add(ITEMS, listaClasificaciones);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "getClasificacionCombo", e, AccionEnum.FILLCOMBO, 0, 0);
            }
            return resultado;
            #endregion
        }
        #endregion

        #region CAPTURAS
        #region ACOPIO
        public Dictionary<string, object> GetCapturas(CapturaDTO _objFiltroDTO)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region SE OBTIENE LISTADO DE USUARIOS || CP-EK/ARR-EK
                    List<dynamic> lstUsuarios = new List<dynamic>();

                    var lstUsuariosCP = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT clave_empleado, CONVERT(VARCHAR(20), nombre) + ' ' + CONVERT(VARCHAR(20), ape_paterno) + ' ' + CONVERT(VARCHAR(20), ape_materno) AS nombreCompleto 
                                    FROM tblRH_EK_Empleados
                                    WHERE estatus_empleado = 'A'
                                    ORDER BY nombreCompleto",
                    });

                    var lstUsuariosArr = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT clave_empleado, CONVERT(VARCHAR(20), nombre) + ' ' + CONVERT(VARCHAR(20), ape_paterno) + ' ' + CONVERT(VARCHAR(20), ape_materno) AS nombreCompleto 
                                    FROM tblRH_EK_Empleados
                                    WHERE estatus_empleado = 'A'
                                    ORDER BY nombreCompleto",
                    });

                    // MERGE LISTADO USUARIOS
                    lstUsuarios.AddRange(lstUsuariosCP);
                    lstUsuarios.AddRange(lstUsuariosArr);
                    #endregion

                    #region SE OBTIENE EL ULTIMO DÍA DEL MES
                    DateTime mesInicio = new DateTime(_objFiltroDTO.mesInicio.Year, _objFiltroDTO.mesInicio.Month, 1);
                    DateTime mesFinal = _objFiltroDTO.mesFinal.AddMonths(1).AddDays(-1);

                    string ddInicio = Convert.ToInt32(mesInicio.Day) >= 1 && Convert.ToInt32(mesInicio.Day) <= 9 ? "0" + mesInicio.Day.ToString() : mesInicio.Day.ToString();
                    string mmInicio = Convert.ToInt32(mesInicio.Month) >= 1 && Convert.ToInt32(mesInicio.Month) <= 9 ? "0" + mesInicio.Month.ToString() : mesInicio.Month.ToString();
                    string fechaInicio = mesInicio.Year + "-" + mmInicio + "-" + ddInicio + "T00:00:00";

                    string ddFinal = Convert.ToInt32(mesFinal.Day) >= 1 && Convert.ToInt32(mesFinal.Day) <= 9 ? "0" + mesFinal.Day.ToString() : mesFinal.Day.ToString();
                    string mmFinal = Convert.ToInt32(mesFinal.Month) >= 1 && Convert.ToInt32(mesFinal.Month) <= 9 ? "0" + mesFinal.Month.ToString() : mesFinal.Month.ToString();
                    string fechaFinal = mesFinal.Year + "-" + mmFinal + "-" + ddFinal + "T00:00:00";
                    #endregion

                    #region SE OBTIENE LISTADO DE CAPTURAS
                    string strQuery = "";
                    strQuery += @"SELECT t1.id, t1.folio, t1.idAgrupacion, t1.fechaEntrada, t2.nomAgrupacion, t1.idResponsableTecnico, t1.estatusCaptura, t1.tipoCaptura, t1.cantidadContenedor
                                        FROM tblS_MedioAmbienteCaptura AS t1 
                                        INNER JOIN tblS_IncidentesAgrupacionCC AS t2 ON t1.idAgrupacion = t2.id 
                                            WHERE t1.esActivo = @esActivo AND (t1.fechaEntrada BETWEEN @fechaInicio AND @fechaFinal)";

                    if (_objFiltroDTO.estatusCaptura > 0)
                        strQuery += " AND estatusCaptura = " + _objFiltroDTO.estatusCaptura;

                    strQuery += " ORDER BY estatusCaptura";

                    List<CapturaDTO> lstCapturas = _context.Select<CapturaDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = strQuery,
                        parametros = new { esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                    }).ToList();

                    lstCapturas = lstCapturas.Where(x => (_objFiltroDTO.idAgrupacion > 0 ? x.idAgrupacion == _objFiltroDTO.idAgrupacion : true) && x.fechaEntrada.Date >= mesInicio.Date && x.fechaEntrada.Date <= mesFinal.Date).ToList();

                    foreach (var item in lstCapturas)
                    {
                        #region SE OBTIENE NOMBRE DEL RESPONSABLE TECNICO
                        if (item.idResponsableTecnico > 0)
                            item.responsableTecnico = lstUsuarios.Where(w => w.clave_empleado == item.idResponsableTecnico).Select(s => s.nombreCompleto).FirstOrDefault();
                        #endregion

                        #region SE OBTIENE ESTATUS DE LA CAPTURA
                        switch (item.estatusCaptura)
                        {
                            case (int)EstatusCapturaEnum.acopio:
                                item.strEstatusCaptura = EnumHelper.GetDescription((EstatusCapturaEnum)item.estatusCaptura);
                                break;
                            case (int)EstatusCapturaEnum.trayecto:
                                item.strEstatusCaptura = EnumHelper.GetDescription((EstatusCapturaEnum)item.estatusCaptura);
                                break;
                            case (int)EstatusCapturaEnum.destinoFinal:
                                item.strEstatusCaptura = EnumHelper.GetDescription((EstatusCapturaEnum)item.estatusCaptura);
                                break;
                            default:
                                break;
                        }
                        #endregion

                        #region SE OBTIENE NOMBRE DEL TIPO DE CAPTURA
                        switch (item.tipoCaptura)
                        {
                            case 1:
                                item.captura = "Aspecto ambiental";
                                break;
                            case 2:
                                item.captura = "Residuo peligroso";
                                break;
                            case 3:
                                item.captura = "RSU y RME";
                                break;
                            default:
                                item.captura = "N/A";
                                break;
                        }
                        #endregion

                        #region SE OBTIENE LA CANTIDAD DEL CONTENEDOR
                        if (item.cantidadContenedor > 0)
                            item.strCantidadContenedor = item.cantidadContenedor.ToString();
                        else
                            item.strCantidadContenedor = "N/A";
                        #endregion
                    }

                    #region SE VERIFICA EN QUE ESTATUS SE ENCUENTRAN LOS AA DE CADA CAPTURA
                    #endregion

                    resultado.Add("data", lstCapturas);
                    resultado.Add(SUCCESS, true);

                    dbContextTransaction.Commit();
                    #endregion

                    #region SE CREA BITACORA
                    SaveBitacora(16, (int)AccionEnum.CONSULTA, 0, JsonUtils.convertNetObjectToJson(lstCapturas));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "MedioAmbienteController", "GetCapturas", e, AccionEnum.CONSULTA, 0, _objFiltroDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }

        public Dictionary<string, object> CrearEditarCaptura(CapturaDTO _objCEDTO, HttpPostedFileBase _objFile)
        {
            resultado = new Dictionary<string, object>();
            bool esCrear = _objCEDTO.id <= 0 ? true : false;
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region VALIDACIÓN DE CAMPOS VACIOS
                    bool errorCrearEditar = false;
                    string strMensajeError = string.Empty;

                    if (_objCEDTO.idAgrupacion <= 0 || _objCEDTO.idResponsableTecnico <= 0 || _objCEDTO.fechaEntrada == null || string.IsNullOrEmpty(_objCEDTO.tipoContenedor) ||
                        string.IsNullOrEmpty(_objCEDTO.plantaProcesoGeneracion) || (esCrear && _objFile == null))
                    {
                        errorCrearEditar = true;
                        strMensajeError += "Es necesario indicar lo siguiente:";
                        strMensajeError += _objCEDTO.idAgrupacion <= 0 ? "<br>- Agrupación." : string.Empty;
                        strMensajeError += _objCEDTO.idResponsableTecnico <= 0 ? "<br>- Técnico responsable." : string.Empty;
                        strMensajeError += _objCEDTO.fechaEntrada == null ? "<br>- Fecha entrada." : string.Empty;
                        strMensajeError += _objCEDTO.cantidadContenedor <= 0 && _objCEDTO.tipoCaptura != 2 ? "<br>- Cantidad del contenedor." : string.Empty;
                        strMensajeError += string.IsNullOrEmpty(_objCEDTO.codigoContenedor) ? "<br>- Código del contenedor." : string.Empty;
                        strMensajeError += string.IsNullOrEmpty(_objCEDTO.tipoContenedor) ? "<br>- Tipo de contenedor" : string.Empty;
                        strMensajeError += string.IsNullOrEmpty(_objCEDTO.plantaProcesoGeneracion) ? "<br>- Planta/Proceso de generación" : string.Empty;
                        strMensajeError += _objFile == null ? "<br>- Archivo" : string.Empty;

                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, strMensajeError);
                    }
                    #endregion

                    #region CREAR/EDITAR CAPTURA
                    tblS_MedioAmbienteCaptura objCECaptura = new tblS_MedioAmbienteCaptura();
                    if (_objCEDTO.id > 0 && !errorCrearEditar)
                    {
                        #region ACTUALIZAR CAPTURA
                        objCECaptura = _context.tblS_MedioAmbienteCaptura.Where(w => w.id == _objCEDTO.id).FirstOrDefault();
                        if (objCECaptura == null)
                            throw new Exception("Ocurrió un error al actualizar el registro");

                        objCECaptura.tipoCaptura = _objCEDTO.tipoCaptura;
                        objCECaptura.idAgrupacion = _objCEDTO.idAgrupacion;
                        objCECaptura.idResponsableTecnico = _objCEDTO.idResponsableTecnico;
                        objCECaptura.fechaEntrada = _objCEDTO.fechaEntrada;
                        objCECaptura.cantidadContenedor = _objCEDTO.tipoCaptura == 2 ? _objCEDTO.lstAspectosAmbientalesID.Count() : 0;
                        objCECaptura.tipoContenedor = _objCEDTO.tipoContenedor.Trim();
                        objCECaptura.plantaProcesoGeneracion = _objCEDTO.plantaProcesoGeneracion.Trim();
                        objCECaptura.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCECaptura.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();
                        #endregion

                        #region ACTUALIZAR CAPTURA DETALLE

                        #region SE ELIMINA LOS ASPECTOS AMBIENTALES RELACIONADOS A LA CAPTURA PRINICIPAL
                        List<tblS_MedioAmbienteCapturaDet> objEliminarDet = _context.tblS_MedioAmbienteCapturaDet.Where(w => w.idCaptura == _objCEDTO.id && w.esActivo).ToList();
                        foreach (var item in objEliminarDet)
                        {
                            item.fechaModificacion = DateTime.Now;
                            item.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                            item.esActivo = false;
                        }
                        _context.SaveChanges();
                        #endregion

                        #region SE REGISTRA LOS ASPECTOS AMBIENTALES RELACIONADOS A LA CAPTURA PRINCIPAL
                        List<tblS_IncidentesAgrupacionCC> lstIndicentesAgrupacionCC = _context.tblS_IncidentesAgrupacionCC.Where(w => w.id == _objCEDTO.id && w.esActivo).ToList();
                        List<tblS_MedioAmbienteAspectoAmbiental> lstMedioAmbienteAspectoAmbiental = _context.tblS_MedioAmbienteAspectoAmbiental.Where(w => w.esActivo).ToList();
                        List<tblS_MedioAmbienteCapturaDet> lstMedioAmbienteCapturaDet = _context.tblS_MedioAmbienteCapturaDet.Where(w => w.esActivo).ToList();

                        List<tblS_MedioAmbienteCapturaDet> objCapturaDet = new List<tblS_MedioAmbienteCapturaDet>();
                        for (int i = 0; i < _objCEDTO.lstAspectosAmbientalesID.Count(); i++)
                        {
                            Dictionary<string, object> lstCodigosContenedores = new Dictionary<string, object>();
                            lstCodigosContenedores = GetUltimoConsecutivoCodContenedor(_objCEDTO.idAgrupacion, _objCEDTO.lstAspectosAmbientalesID[i], lstIndicentesAgrupacionCC, lstMedioAmbienteAspectoAmbiental, lstMedioAmbienteCapturaDet);
                            string codigoContenedor = lstCodigosContenedores["codigoContenedor"] as string;
                            int codigoContenedorConsecutivo = (int)lstCodigosContenedores["codigoContenedorConsecutivo"];

                            tblS_MedioAmbienteCapturaDet obj = new tblS_MedioAmbienteCapturaDet();
                            obj.idCaptura = _objCEDTO.id;
                            obj.codigoContenedor = codigoContenedor;
                            obj.consecutivoCodContenedor = codigoContenedorConsecutivo;
                            obj.idAspectoAmbiental = _objCEDTO.lstAspectosAmbientalesID[i];
                            obj.cantAspectoAmbiental = _objCEDTO.tipoCaptura != 2 ? _objCEDTO.lstCantidadAspectosAmbientales[i] : 0;
                            obj.estatusAspectoAmbiental = (int)EstatusCapturaEnum.acopio;
                            obj.fechaCreacion = objCECaptura.fechaCreacion;
                            obj.fechaModificacion = DateTime.Now;
                            obj.idUsuarioCreacion = objCECaptura.idUsuarioCreacion;
                            obj.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                            obj.esActivo = true;
                            objCapturaDet.Add(obj);
                        }
                        _context.tblS_MedioAmbienteCapturaDet.AddRange(objCapturaDet);
                        _context.SaveChanges();
                        #endregion

                        #endregion

                        #region ARCHIVOS - ACOPIO
                        if (_objFile != null)
                        {
                            #region SE ELIMINA ARCHIVO QUE SE ENCUENTRA YA REGISTRADO
                            tblS_MedioAmbienteArchivos objEliminarArchivo = _context.tblS_MedioAmbienteArchivos.Where(w => w.idCaptura == _objCEDTO.id && w.registroActivo && w.tipoArchivo == (int)TipoArchivoEnum.acopio).FirstOrDefault();
                            if (objEliminarArchivo == null)
                                throw new Exception("Ocurrió un error al actualizar el archivo acopio.");

                            objEliminarArchivo.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objEliminarArchivo.fechaModificacion = DateTime.Now;
                            objEliminarArchivo.registroActivo = false;
                            _context.SaveChanges();
                            #endregion

                            #region SE REGISTRA ARCHIVO NUEVO
                            bool registroSatisfatorio = RegistrarArchivo(_objCEDTO, _objFile, (int)TipoArchivoEnum.acopio);
                            if (!registroSatisfatorio)
                            {
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ocurrió un error al registrar el archivo en el servidor.");
                            }
                            #endregion
                        }
                        #endregion

                        resultado = new Dictionary<string, object>();
                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha actualizado con éxito la captura.");
                    }
                    else if (!errorCrearEditar)
                    {
                        #region SE OBTIENE DATOS DE LA AGRUPACIÓN
                        tblS_IncidentesAgrupacionCC objAgrupacion = _context.Select<tblS_IncidentesAgrupacionCC>(new DapperDTO
                        {
                            baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                            consulta = @"SELECT id, codigo, nomAgrupacion FROM tblS_IncidentesAgrupacionCC WHERE id = @idAgrupacion AND esActivo = @esActivo",
                            parametros = new { idAgrupacion = _objCEDTO.idAgrupacion, esActivo = true }
                        }).FirstOrDefault();

                        if (objAgrupacion == null)
                            throw new Exception("No se encontro la agrupación seleccionada para crear el folio.");
                        #endregion

                        #region SE OBTIENE EL ULTIMO FOLIO DE LA CAPTURA, DE LO CONTRARIO SE CREA FOLIO INICIAL
                        tblS_MedioAmbienteCaptura ultimoFolio = _context.Select<tblS_MedioAmbienteCaptura>(new DapperDTO
                        {
                            baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                            consulta = @"SELECT folio, consecutivo FROM tblS_MedioAmbienteCaptura WHERE idAgrupacion = @idAgrupacion AND esActivo = @esActivo ORDER BY consecutivo DESC",
                            parametros = new { idAgrupacion = _objCEDTO.idAgrupacion, esActivo = true }
                        }).FirstOrDefault();

                        if (ultimoFolio != null)
                        {
                            string consecutivoLength = ultimoFolio.consecutivo.ToString();
                            int iConsecutivo = Convert.ToInt32(ultimoFolio.consecutivo);
                            iConsecutivo++;

                            switch (iConsecutivo.ToString().Length)
                            {
                                case 1:
                                    _objCEDTO.consecutivo = "0000" + iConsecutivo;
                                    break;
                                case 2:
                                    _objCEDTO.consecutivo = "000" + iConsecutivo;
                                    break;
                                case 3:
                                    _objCEDTO.consecutivo = "00" + iConsecutivo;
                                    break;
                                case 4:
                                    _objCEDTO.consecutivo = "0" + iConsecutivo;
                                    break;
                                default:
                                    _objCEDTO.consecutivo = iConsecutivo.ToString();
                                    break;
                            }
                            _objCEDTO.folio = objAgrupacion.nomAgrupacion + " - " + _objCEDTO.consecutivo;

                            if (string.IsNullOrEmpty(_objCEDTO.folio) || string.IsNullOrEmpty(_objCEDTO.consecutivo))
                                throw new Exception("Ocurrió un error al generar el folio de la captura.");
                        }
                        else
                        {
                            _objCEDTO.consecutivo = "00001";
                            _objCEDTO.folio = objAgrupacion.nomAgrupacion + " - " + _objCEDTO.consecutivo;
                        }
                        #endregion

                        #region GUARDAR NUEVA CAPTURA
                        objCECaptura.folio = _objCEDTO.folio;
                        objCECaptura.consecutivo = _objCEDTO.consecutivo;
                        objCECaptura.tipoCaptura = _objCEDTO.tipoCaptura;
                        objCECaptura.idEmpresa = (int)idEmpresaActual;
                        objCECaptura.idAgrupacion = _objCEDTO.idAgrupacion;
                        objCECaptura.idResponsableTecnico = _objCEDTO.idResponsableTecnico;
                        objCECaptura.fechaEntrada = _objCEDTO.fechaEntrada;
                        objCECaptura.cantidadContenedor = _objCEDTO.tipoCaptura == 2 ? _objCEDTO.lstAspectosAmbientalesID.Count() : 0;
                        objCECaptura.tipoContenedor = _objCEDTO.tipoContenedor.Trim();
                        objCECaptura.plantaProcesoGeneracion = _objCEDTO.plantaProcesoGeneracion.Trim();
                        objCECaptura.estatusCaptura = (int)EstatusCapturaEnum.acopio;
                        objCECaptura.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCECaptura.fechaCreacion = DateTime.Now;
                        objCECaptura.esActivo = true;
                        _context.tblS_MedioAmbienteCaptura.Add(objCECaptura);
                        _context.SaveChanges();
                        #endregion

                        #region SE REGISTRA LAS ASPECTOS AMBIENTALES RELACIONADOS A LA CAPTURA PRINCIPAL
                        _objCEDTO.id = objCECaptura.id;

                        List<tblS_IncidentesAgrupacionCC> lstIndicentesAgrupacionCC = _context.tblS_IncidentesAgrupacionCC.Where(w => w.id == _objCEDTO.id && w.esActivo).ToList();
                        List<tblS_MedioAmbienteAspectoAmbiental> lstMedioAmbienteAspectoAmbiental = _context.tblS_MedioAmbienteAspectoAmbiental.Where(w => w.esActivo).ToList();
                        List<tblS_MedioAmbienteCapturaDet> lstMedioAmbienteCapturaDet = _context.tblS_MedioAmbienteCapturaDet.Where(w => w.esActivo).ToList();
                        List<tblS_MedioAmbienteCapturaDet> objCapturaDet = new List<tblS_MedioAmbienteCapturaDet>();
                        for (int i = 0; i < _objCEDTO.lstAspectosAmbientalesID.Count(); i++)
                        {
                            Dictionary<string, object> lstCodigosContenedores = new Dictionary<string, object>();
                            lstCodigosContenedores = GetUltimoConsecutivoCodContenedor(_objCEDTO.idAgrupacion, _objCEDTO.lstAspectosAmbientalesID[i], lstIndicentesAgrupacionCC, lstMedioAmbienteAspectoAmbiental, lstMedioAmbienteCapturaDet);
                            string codigoContenedor = lstCodigosContenedores["codigoContenedor"] as string;
                            int codigoContenedorConsecutivo = (int)lstCodigosContenedores["codigoContenedorConsecutivo"];

                            tblS_MedioAmbienteCapturaDet obj = new tblS_MedioAmbienteCapturaDet();
                            obj.idCaptura = _objCEDTO.id;
                            obj.codigoContenedor = codigoContenedor;
                            obj.consecutivoCodContenedor = codigoContenedorConsecutivo;
                            obj.idAspectoAmbiental = _objCEDTO.lstAspectosAmbientalesID[i];
                            obj.cantAspectoAmbiental = _objCEDTO.tipoCaptura != 2 ? _objCEDTO.lstCantidadAspectosAmbientales[i] : 0;
                            obj.estatusAspectoAmbiental = (int)EstatusCapturaEnum.acopio;
                            obj.fechaCreacion = objCECaptura.fechaCreacion;
                            obj.idUsuarioCreacion = objCECaptura.idUsuarioCreacion;
                            obj.esActivo = true;
                            objCapturaDet.Add(obj);
                        }
                        _context.tblS_MedioAmbienteCapturaDet.AddRange(objCapturaDet);
                        _context.SaveChanges();
                        #endregion

                        #region SE REGISTRA ARCHIVO EN CAPTURA - ACOPIO
                        resultado = new Dictionary<string, object>();
                        bool registroSatisfatorio = RegistrarArchivo(_objCEDTO, _objFile, (int)TipoArchivoEnum.acopio);
                        if (!registroSatisfatorio)
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ocurrió un error al registrar el archivo en el servidor.");
                        }
                        #endregion

                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha registrado con éxito la captura.");
                    }

                    dbContextTransaction.Commit();
                    #endregion

                    #region SE CREA BITACORA
                    SaveBitacora(16, !esCrear ? (int)AccionEnum.ACTUALIZAR : (int)AccionEnum.AGREGAR, _objCEDTO.id, JsonUtils.convertNetObjectToJson(objCECaptura));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "MedioAmbienteController", "CrearEditarCaptura", e, !esCrear ? AccionEnum.ACTUALIZAR : AccionEnum.AGREGAR, !esCrear ? _objCEDTO.id : 0, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }

        public Dictionary<string, object> EliminarCaptura(int _idCaptura)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region VALIDACIONES
                    if (_idCaptura <= 0)
                        throw new Exception("Ocurrió un error al eliminar la captura.");
                    #endregion

                    #region SE ELIMINA LA CAPTURA
                    tblS_MedioAmbienteCaptura objEliminar = _context.tblS_MedioAmbienteCaptura.Where(w => w.id == _idCaptura).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception("Ocurrió un error al eliminar la captura.");
                    else
                    {
                        objEliminar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objEliminar.fechaModificacion = DateTime.Now;
                        objEliminar.esActivo = false;
                        _context.SaveChanges();

                        #region SE ELIMINA LOS ASPECTOS AMBIENTALES RELACIONADOS A LA CAPTURA EN SU TABLA DETALLE
                        List<tblS_MedioAmbienteCapturaDet> objEliminarDet = _context.tblS_MedioAmbienteCapturaDet.Where(w => w.idCaptura == _idCaptura && w.esActivo).ToList();
                        foreach (var item in objEliminarDet)
                        {
                            item.fechaModificacion = DateTime.Now;
                            item.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                            item.esActivo = false;
                        }
                        _context.SaveChanges();
                        #endregion

                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                    }
                    dbContextTransaction.Commit();
                    #endregion

                    #region SE CREA BITACORA
                    SaveBitacora(16, (int)AccionEnum.ELIMINAR, _idCaptura, JsonUtils.convertNetObjectToJson(objEliminar));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "MedioAmbienteController", "EliminarCaptura", e, AccionEnum.ELIMINAR, _idCaptura, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }

        private string ObtenerFormatoNombreArchivo(string nombreBase, string fileName)
        {
            return String.Format("{0} {1}{2}", nombreBase, ObtenerFormatoCarpetaFechaActual(), Path.GetExtension(fileName));
        }

        private string ObtenerFormatoCarpetaFechaActual()
        {
            return DateTime.Now.ToString("dd-MM-y HH:mm:ss").Replace(":", "-");
        }

        public Dictionary<string, object> GetDatosActualizarCaptura(int _idCaptura)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region SE OBTIENE INFORMACIÓN A ACTUALIZAR DE LA CAPTURA SELECCIONADA
                    CapturaDTO objCaptura = _context.Select<CapturaDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT idAgrupacion, idResponsableTecnico, fechaEntrada, cantidadContenedor, tipoContenedor, plantaProcesoGeneracion, tipoCaptura
	                                        FROM tblS_MedioAmbienteCaptura 
		                                        WHERE id = @id",
                        parametros = new { id = _idCaptura }
                    }).FirstOrDefault();
                    resultado.Add("objCaptura", objCaptura);
                    #endregion

                    #region SE OBTIENE LOS ASPECTOS AMBIENTALES A ACTUALIZAR RELACIONADOS A LA CAPTURA
                    List<CapturaDTO> objCapturaDet = _context.Select<CapturaDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT t1.id, t1.codigoContenedor, t1.idAspectoAmbiental AS aspectoAmbientalID, t1.cantAspectoAmbiental AS cantidad, t2.unidad
	                                        FROM tblS_MedioAmbienteCapturaDet AS t1
	                                        INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t2 ON t1.idAspectoAmbiental = t2.id
		                                        WHERE idCaptura = @idCaptura AND t1.esActivo = @esActivo AND t2.esActivo = @esActivo",
                        parametros = new { idCaptura = _idCaptura, esActivo = true }
                    }).ToList();
                    foreach (var item in objCapturaDet)
                    {
                        item.unidadMedida = EnumHelper.GetDescription((UnidadEnum)item.unidad);
                    }
                    resultado.Add("objCapturaDet", objCapturaDet);
                    #endregion

                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();

                    #region SE CREA BITACORA
                    SaveBitacora(16, (int)AccionEnum.CONSULTA, 0, JsonUtils.convertNetObjectToJson(objCaptura));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "MedioAmbienteController", "GetDatosActualizarCaptura", e, AccionEnum.CONSULTA, _idCaptura, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }

        public Dictionary<string, object> GetUltimoConsecutivoCodContenedor(int idAgrupacion, int idAspectoAmbiental,
                                                                                List<tblS_IncidentesAgrupacionCC> lstIndicentesAgrupacionCC,
                                                                                List<tblS_MedioAmbienteAspectoAmbiental> lstMedioAmbienteAspectoAmbiental,
                                                                                List<tblS_MedioAmbienteCapturaDet> lstMedioAmbienteCapturaDet)
        {
            resultado = new Dictionary<string, object>();
            string codigoContenedor = string.Empty;
            int codigoContenedorConsecutivo = 0;

            #region SE OBTIENE DATOS DE LA AGRUPACIÓN
            //tblS_IncidentesAgrupacionCC objAgrupacion = lstIndicentesAgrupacionCC.Select<tblS_IncidentesAgrupacionCC>(new DapperDTO
            //{
            //                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
            //    consulta = @"SELECT id, codigo, nomAgrupacion FROM tblS_IncidentesAgrupacionCC WHERE id = @idAgrupacion AND esActivo = @esActivo",
            //    parametros = new { idAgrupacion = idAgrupacion, esActivo = true }
            //}).FirstOrDefault();
            tblS_IncidentesAgrupacionCC objAgrupacion = new tblS_IncidentesAgrupacionCC();
            if (lstIndicentesAgrupacionCC != null)
                objAgrupacion = lstIndicentesAgrupacionCC.Where(w => w.id == idAgrupacion && w.esActivo).FirstOrDefault();
            else
                objAgrupacion = _context.tblS_IncidentesAgrupacionCC.Where(w => w.id == idAgrupacion && w.esActivo).FirstOrDefault();
            #endregion

            #region SE OBTIENE DATOS DEL ASPECTO AMBIENTAL
            //tblS_MedioAmbienteAspectoAmbiental objAspectoAmbiental = _context.Select<tblS_MedioAmbienteAspectoAmbiental>(new DapperDTO
            //{
            //                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
            //    consulta = @"SELECT descripcion FROM tblS_MedioAmbienteAspectoAmbiental WHERE id = @idAspectoAmbiental AND esActivo = @esActivo",
            //    parametros = new { idAspectoAmbiental = idAspectoAmbiental, esActivo = true }
            //}).FirstOrDefault();
            tblS_MedioAmbienteAspectoAmbiental objAspectoAmbiental = new tblS_MedioAmbienteAspectoAmbiental();
            if (lstMedioAmbienteAspectoAmbiental != null)
                objAspectoAmbiental = lstMedioAmbienteAspectoAmbiental.Where(w => w.id == idAspectoAmbiental && w.esActivo).FirstOrDefault();
            else
                objAspectoAmbiental = _context.tblS_MedioAmbienteAspectoAmbiental.Where(w => w.id == idAspectoAmbiental && w.esActivo).FirstOrDefault();
            #endregion

            if (idAgrupacion <= 0 || idAspectoAmbiental <= 0)
            {
                resultado.Add(SUCCESS, true);
                resultado.Add("codigoContenedor", codigoContenedor);
                resultado.Add("codigoContenedorConsecutivo", codigoContenedorConsecutivo);
            }
            else
            {
                tblS_MedioAmbienteCapturaDet ultimoFolio = _context.Select<tblS_MedioAmbienteCapturaDet>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT t1.codigoContenedor, t1.consecutivoCodContenedor 
	                                FROM tblS_MedioAmbienteCapturaDet AS t1
	                                INNER JOIN tblS_MedioAmbienteCaptura AS t2 ON t1.idCaptura = t2.id
		                                WHERE t2.idAgrupacion = @idAgrupacion AND t1.idAspectoAmbiental = @idAspectoAmbiental AND t2.esActivo = @esActivo AND t1.esActivo = @esActivo
			                                ORDER BY t2.id DESC",
                    parametros = new { idAgrupacion = idAgrupacion, idAspectoAmbiental = idAspectoAmbiental, esActivo = true }
                }).FirstOrDefault();

                if (ultimoFolio != null)
                {
                    ultimoFolio.consecutivoCodContenedor++;
                    codigoContenedorConsecutivo = ultimoFolio.consecutivoCodContenedor;
                    codigoContenedor = objAspectoAmbiental.descripcion + "-" + codigoContenedorConsecutivo.ToString();
                }
                else
                {
                    codigoContenedor = objAspectoAmbiental.descripcion + "-1";
                    codigoContenedorConsecutivo = 1;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("codigoContenedor", codigoContenedor);
                resultado.Add("codigoContenedorConsecutivo", codigoContenedorConsecutivo);
            }
            return resultado;
        }
        #endregion

        #region TRAYECTOS
        public Dictionary<string, object> CrearEditarTrayecto(CapturaDTO _objCEDTO, HttpPostedFileBase _objFile)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region VALIDACIÓN DE CAMPOS VACIOS
                    bool errorCrearEditar = false;
                    string strMensajeError = string.Empty;
                    if (string.IsNullOrEmpty(_objCEDTO.tratamiento) || string.IsNullOrEmpty(_objCEDTO.manifiesto) || _objCEDTO.fechaEmbarque == null || string.IsNullOrEmpty(_objCEDTO.tipoTransporte) || _objCEDTO.idTransportistaTrayecto <= 0 &&
                        _objFile == null)
                    {
                        errorCrearEditar = true;
                        strMensajeError += "Es necesario indicar lo siguiente:";
                        strMensajeError += string.IsNullOrEmpty(_objCEDTO.tratamiento) ? "<br>- Tratamiento" : string.Empty;
                        strMensajeError += string.IsNullOrEmpty(_objCEDTO.manifiesto) ? "<br>- Manifiesto" : string.Empty;
                        strMensajeError += _objCEDTO.fechaEmbarque == null ? "<br>- Fecha embarque." : string.Empty;
                        strMensajeError += string.IsNullOrEmpty(_objCEDTO.tipoTransporte) ? "<br>- Tipo transporte." : string.Empty;
                        strMensajeError += _objCEDTO.idTransportistaTrayecto <= 0 ? "<br>- Transportista trayecto." : string.Empty;
                        strMensajeError += _objFile == null ? "<br>- Archivo." : string.Empty;

                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, strMensajeError);
                    }
                    #endregion

                    #region ACTUALIZAR TRAYECTO
                    tblS_MedioAmbienteCaptura objCECaptura = new tblS_MedioAmbienteCaptura();
                    if (_objCEDTO.id > 0 && !errorCrearEditar)
                    {
                        objCECaptura = _context.tblS_MedioAmbienteCaptura.Where(w => w.id == _objCEDTO.id).FirstOrDefault();
                        if (objCECaptura == null)
                            throw new Exception("Ocurrió un error al registrar la información del trayecto.");

                        #region SE VERIFICA SI SE ASIGNA POR PRIMERA VEZ A UN TRANSPORTISTA EN ESTA CAPTURA
                        bool primeraAsignacionTransportista = false;
                        primeraAsignacionTransportista = objCECaptura.idTransportistaTrayecto > 0 ? false : true;
                        #endregion

                        objCECaptura.tratamiento = _objCEDTO.tratamiento.Trim();
                        objCECaptura.manifiesto = _objCEDTO.manifiesto.Trim();
                        objCECaptura.fechaEmbarque = _objCEDTO.fechaEmbarque;
                        objCECaptura.tipoTransporte = _objCEDTO.tipoTransporte.Trim();
                        objCECaptura.idTransportistaTrayecto = _objCEDTO.idTransportistaTrayecto;

                        if (objCECaptura.estatusCaptura == (int)EstatusCapturaEnum.acopio)
                            objCECaptura.estatusCaptura = (int)EstatusCapturaEnum.trayecto;

                        objCECaptura.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCECaptura.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();

                        #region ARCHIVOS - TRAYECTO
                        if (_objFile != null)
                        {
                            #region SE ELIMINA ARCHIVO QUE SE ENCUENTRA YA REGISTRADO
                            tblS_MedioAmbienteArchivos objEliminarArchivo = _context.tblS_MedioAmbienteArchivos.Where(w => w.idCaptura == _objCEDTO.id && w.registroActivo && w.tipoArchivo == (int)TipoArchivoEnum.trayecto).FirstOrDefault();
                            if (objEliminarArchivo != null)
                            {
                                objEliminarArchivo.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                                objEliminarArchivo.fechaModificacion = DateTime.Now;
                                objEliminarArchivo.registroActivo = false;
                                _context.SaveChanges();
                            }
                            #endregion

                            #region SE REGISTRA ARCHIVO NUEVO
                            bool registroSatisfatorio = RegistrarArchivo(_objCEDTO, _objFile, (int)TipoArchivoEnum.trayecto);
                            if (!registroSatisfatorio)
                            {
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ocurrió un error al registrar el archivo en el servidor.");
                            }
                            #endregion
                        }
                        #endregion

                        resultado.Add(SUCCESS, true);
                        if (primeraAsignacionTransportista)
                            resultado.Add(MESSAGE, "Se ha registrado con éxito.");
                        else
                            resultado.Add(MESSAGE, "Se ha actualizado con éxito.");
                    }

                    dbContextTransaction.Commit();
                    #endregion

                    #region SE CREA BITACORA
                    SaveBitacora(16, (int)AccionEnum.ACTUALIZAR, objCECaptura.id, JsonUtils.convertNetObjectToJson(objCECaptura));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "MedioAmbienteController", "CrearEditarTrayecto", e, AccionEnum.ACTUALIZAR, _objCEDTO.id, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }

        public Dictionary<string, object> EliminarTrayecto(int _idCaptura)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region VALIDACIONES
                    if (_idCaptura <= 0)
                        throw new Exception("Ocurrió un error al eliminar el trayecto.");
                    #endregion

                    #region SE CAMBIA ESTATUS DE LA CAPTURA A ACOPIO
                    tblS_MedioAmbienteCaptura objActualizar = _context.tblS_MedioAmbienteCaptura.Where(w => w.id == _idCaptura).FirstOrDefault();
                    if (objActualizar == null)
                        throw new Exception("Ocurrió un error al eliminar el trayecto.");
                    else
                    {
                        objActualizar.estatusCaptura = (int)EstatusCapturaEnum.acopio;
                        objActualizar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objActualizar.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();

                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                    }

                    dbContextTransaction.Commit();
                    #endregion

                    #region SE CREA BITACORA
                    SaveBitacora(16, (int)AccionEnum.ELIMINAR, _idCaptura, JsonUtils.convertNetObjectToJson(objActualizar));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "MedioAmbienteController", "EliminarTrayecto", e, AccionEnum.ELIMINAR, _idCaptura, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarTrayecto(int _idCaptura)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region SE OBTIENE INFORMACIÓN A ACTUALIZAR DEL TRAYECTO SELECCIONADO
                    CapturaDTO objCaptura = _context.Select<CapturaDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT t1.id, t1.tratamiento, t1.manifiesto, t1.fechaEmbarque, t1.tipoTransporte, t1.idTransportistaTrayecto, t2.razonSocial, t1.estatusCaptura
	                                            FROM tblS_MedioAmbienteCaptura AS t1
	                                            INNER JOIN tblS_MedioAmbienteTransportistas AS t2 ON t1.idTransportistaTrayecto = t2.id
		                                            WHERE t1.esActivo = @esActivo AND t2.esActivo = @esActivo AND t1.id = @id",
                        parametros = new { esActivo = 1, id = _idCaptura }
                    }).FirstOrDefault();

                    if (objCaptura == null)
                        resultado.Add("objCaptura", "");
                    else if (objCaptura.estatusCaptura != (int)EstatusCapturaEnum.acopio)
                        resultado.Add("objCaptura", objCaptura);

                    resultado.Add(SUCCESS, true);

                    dbContextTransaction.Commit();
                    #endregion

                    #region SE CREA BITACORA
                    SaveBitacora(16, (int)AccionEnum.CONSULTA, 0, JsonUtils.convertNetObjectToJson(objCaptura));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "MedioAmbienteController", "GetDatosActualizarTrayecto", e, AccionEnum.CONSULTA, _idCaptura, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }

        public Dictionary<string, object> GetAspectosAmbientalesToTrayectos(int idAgrupacion, string consecutivo)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LOS ASPECTOS AMBIENTES CON ESTATUS ACOPIO Y DE LA AGRUPACIÓN SELECCIONADA
                List<AspectosAmbientalesToTrayectosDTO> lstAspectosAmbientales = _context.Select<AspectosAmbientalesToTrayectosDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT t2.id, t2.codigoContenedor, t2.idAspectoAmbiental, t3.descripcion AS aspectoAmbiental, t3.clasificacion, t2.idCaptura
	                                    FROM tblS_MedioAmbienteCaptura AS t1
	                                    INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
	                                    INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
		                                    WHERE t1.esActivo = @esActivo AND t2.esActivo = @esActivo AND t3.esActivo = @esActivo AND 
                                                    t1.idAgrupacion = @idAgrupacion AND t2.estatusAspectoAmbiental = @estatusAspectoAmbiental AND t1.consecutivo = @consecutivo",
                    parametros = new { esActivo = true, idAgrupacion = idAgrupacion, estatusAspectoAmbiental = (int)EstatusCapturaEnum.acopio, consecutivo = consecutivo }
                }).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add("lstAspectosAmbientales", lstAspectosAmbientales);
                #endregion
            }
            catch (Exception e)
            {
                LogError(16, 16, "MedioAmbienteController", "GetDatosActualizarTrayecto", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CrearAspectoAmbientalAcopioToTrayecto(AspectosAmbientalesToTrayectosDTO objParamDTO, HttpPostedFileBase objFile)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region SE VALIDA QUE LA INFORMACIÓN ESTE COMPLETA
                    bool errorCrear = false;
                    string strMensajeError = string.Empty;
                    if (objParamDTO.idAgrupacion <= 0 || objParamDTO.lstAspectosAmbientalesID.Count() <= 0 || string.IsNullOrEmpty(objParamDTO.tratamiento) || string.IsNullOrEmpty(objParamDTO.manifiesto) ||
                        objParamDTO.fechaEmbarque == null || string.IsNullOrEmpty(objParamDTO.tipoTransporte) || objParamDTO.idTransportistaTrayecto <= 0)
                    {
                        errorCrear = true;
                        strMensajeError += "Es necesario indicar lo siguiente:";
                        strMensajeError += objParamDTO.idAgrupacion <= 0 ? "<br>- Agrupación." : string.Empty;
                        strMensajeError += objParamDTO.lstAspectosAmbientalesID.Count() <= 0 ? "<br>- Seleccionar un aspecto ambiental." : string.Empty;
                        strMensajeError += string.IsNullOrEmpty(objParamDTO.tratamiento) ? "<br>- Tratamiento." : string.Empty;
                        strMensajeError += string.IsNullOrEmpty(objParamDTO.manifiesto) ? "<br>- Manifiesto." : string.Empty;
                        strMensajeError += objParamDTO.fechaEmbarque == null ? "<br>- Fecha embarque." : string.Empty;
                        strMensajeError += string.IsNullOrEmpty(objParamDTO.tipoTransporte) ? "<br>- Tipo transporte." : string.Empty;
                        strMensajeError += objParamDTO.idTransportistaTrayecto <= 0 ? "<br>- Transportista." : string.Empty;

                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, strMensajeError);
                    }
                    #endregion

                    if (!errorCrear)
                    {
                        #region SE ACTUALIZA ESTATUS DE LOS ASPECTOS AMBIENTALES SELECCIONADOS DE ACOPIO A TRAYECTO
                        List<tblS_MedioAmbienteCapturaDet> lstAspectosAmbientalesActualizar = _context.tblS_MedioAmbienteCapturaDet.Where(w => objParamDTO.lstAspectosAmbientalesID.Contains(w.id) && w.esActivo).ToList();
                        foreach (var item in lstAspectosAmbientalesActualizar)
                        {
                            item.estatusAspectoAmbiental = (int)EstatusCapturaEnum.trayecto;
                            item.fechaModificacion = DateTime.Now;
                            item.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        }
                        _context.SaveChanges();
                        #endregion

                        #region VERIFICAR ESTATUS DE LOS REGISTROS DETALLE PARA ACTUALIZAR EL REGISTRO PRINCIPAL
                        if (lstAspectosAmbientalesActualizar.Count() > 0)
                        {
                            var idCaptura = lstAspectosAmbientalesActualizar.FirstOrDefault().idCaptura;
                            var listaDetalle = _context.tblS_MedioAmbienteCapturaDet.Where(x => x.esActivo && x.idCaptura == idCaptura).ToList();

                            if (listaDetalle.Count() > 0)
                            {
                                var estatusPrincipal = 0;

                                if (listaDetalle.All(x => x.estatusAspectoAmbiental == (int)EstatusCapturaEnum.acopio))
                                    estatusPrincipal = (int)EstatusCapturaEnum.acopio;
                                else if (listaDetalle.All(x => x.estatusAspectoAmbiental == (int)EstatusCapturaEnum.trayecto))
                                    estatusPrincipal = (int)EstatusCapturaEnum.trayecto;
                                else if (listaDetalle.All(x => x.estatusAspectoAmbiental == (int)EstatusCapturaEnum.destinoFinal))
                                    estatusPrincipal = (int)EstatusCapturaEnum.destinoFinal;
                                else
                                    estatusPrincipal = listaDetalle.Min(x => x.estatusAspectoAmbiental); //Se escoge el estatus más atrasado.

                                var registroPrincipal = _context.tblS_MedioAmbienteCaptura.FirstOrDefault(x => x.id == idCaptura);

                                registroPrincipal.estatusCaptura = estatusPrincipal;
                                _context.SaveChanges();
                            }
                        }
                        #endregion

                        #region ARCHIVOS - TRAYECTO
                        CapturaDTO obj = new CapturaDTO();
                        if (objFile != null)
                        {
                            obj.id = objParamDTO.id;
                            bool registroSatisfatorio = RegistrarArchivo(obj, objFile, (int)TipoArchivoEnum.trayecto);
                            if (!registroSatisfatorio)
                            {
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ocurrió un error al registrar el archivo en el servidor.");
                            }
                        }

                        // SE OBTIENE EL ID DEL ULTIMO REGISTRO DE ARCHIVOS
                        int idArchivo = _context.tblS_MedioAmbienteArchivos.Select(s => s.id).OrderByDescending(o => o).FirstOrDefault();
                        #endregion

                        #region SE REGISTRA LOS ASPECTOS AMBIENTALES QUE SE ENCUENTRAN EN TRAYECTO
                        List<tblS_MedioAmbienteTrayectos> lstAspectosAmbientalesTrayecto = new List<tblS_MedioAmbienteTrayectos>();
                        for (int i = 0; i < objParamDTO.lstAspectosAmbientalesID.Count(); i++)
                        {
                            tblS_MedioAmbienteTrayectos objAATrayecto = new tblS_MedioAmbienteTrayectos();
                            objAATrayecto.idAgrupacion = objParamDTO.idAgrupacion;
                            objAATrayecto.idAspectoAmbiental = objParamDTO.lstAspectosAmbientalesID[i];
                            objAATrayecto.tratamiento = objParamDTO.tratamiento;
                            objAATrayecto.manifiesto = objParamDTO.manifiesto;
                            objAATrayecto.fechaEmbarque = objParamDTO.fechaEmbarque;
                            objAATrayecto.tipoTransporte = objParamDTO.tipoTransporte;
                            objAATrayecto.idTransportistaTrayecto = objParamDTO.idTransportistaTrayecto;
                            objAATrayecto.idArchivoTrayecto = idArchivo;
                            objAATrayecto.fechaCreacion = DateTime.Now;
                            objAATrayecto.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objAATrayecto.registroActivo = true;
                            lstAspectosAmbientalesTrayecto.Add(objAATrayecto);
                        }
                        _context.tblS_MedioAmbienteTrayectos.AddRange(lstAspectosAmbientalesTrayecto);
                        _context.SaveChanges();
                        #endregion

                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha registrado con éxito los AA de Acopio a Trayecto.");
                    }

                    dbContextTransaction.Commit();

                    #region SE CREA BITACORA
                    SaveBitacora(16, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objParamDTO));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "MedioAmbienteController", "CrearAspectoAmbientalAcopioToTrayecto", e, AccionEnum.CONSULTA, 0, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }
        #endregion

        #region DESTINO FINAL
        public Dictionary<string, object> CrearEditarDestinoFinal(CapturaDTO _objCEDTO, HttpPostedFileBase _objFile)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region VALIDACIÓN DE CAMPOS VACIOS
                    bool errorCrearEditar = false;
                    string strMensajeError = string.Empty;

                    if (_objCEDTO.fechaDestinoFinal == null || _objCEDTO.idTransportistaDestinoFinal <= 0)
                    {
                        errorCrearEditar = true;
                        strMensajeError += "Es necesario indicar lo siguiente:";
                        strMensajeError += _objCEDTO.fechaDestinoFinal == null ? "<br>- Fecha destino final." : string.Empty;
                        strMensajeError += _objCEDTO.idTransportistaDestinoFinal <= 0 ? "<br>- Transportista destino final." : string.Empty;

                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, strMensajeError);
                    }
                    #endregion

                    #region ACTUALIZAR DESTINO FINAL
                    tblS_MedioAmbienteCaptura objCECaptura = new tblS_MedioAmbienteCaptura();
                    if (_objCEDTO.id > 0 && !errorCrearEditar)
                    {
                        objCECaptura = _context.tblS_MedioAmbienteCaptura.Where(w => w.id == _objCEDTO.id).FirstOrDefault();
                        if (objCECaptura == null)
                            throw new Exception("Ocurrió un error al registrar la información del destino final.");

                        #region SE VERIFICA SI SE ASIGNA POR PRIMERA VEZ EL DESTINO FINAL A LA CAPTURA
                        bool primeraAsignacionDestinoFinal = false;
                        primeraAsignacionDestinoFinal = objCECaptura.idTransportistaDestinoFinal > 0 ? false : true;
                        #endregion

                        objCECaptura.fechaDestinoFinal = _objCEDTO.fechaDestinoFinal;
                        objCECaptura.idTransportistaDestinoFinal = _objCEDTO.idTransportistaDestinoFinal;

                        if (objCECaptura.estatusCaptura == (int)EstatusCapturaEnum.trayecto)
                            objCECaptura.estatusCaptura = (int)EstatusCapturaEnum.destinoFinal;

                        objCECaptura.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCECaptura.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();

                        #region ARCHIVOS - TRAYECTO
                        if (_objFile != null)
                        {
                            #region SE ELIMINA ARCHIVO QUE SE ENCUENTRA YA REGISTRADO
                            tblS_MedioAmbienteArchivos objEliminarArchivo = _context.tblS_MedioAmbienteArchivos.Where(w => w.idCaptura == _objCEDTO.id && w.registroActivo && w.tipoArchivo == (int)TipoArchivoEnum.destinoFinal).FirstOrDefault();
                            if (objEliminarArchivo != null)
                            {
                                objEliminarArchivo.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                                objEliminarArchivo.fechaModificacion = DateTime.Now;
                                objEliminarArchivo.registroActivo = false;
                                _context.SaveChanges();
                            }
                            #endregion

                            #region SE REGISTRA ARCHIVO NUEVO
                            bool registroSatisfatorio = RegistrarArchivo(_objCEDTO, _objFile, (int)TipoArchivoEnum.destinoFinal);
                            if (!registroSatisfatorio)
                            {
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ocurrió un error al registrar el archivo en el servidor.");
                            }
                            #endregion
                        }
                        #endregion

                        resultado.Add(SUCCESS, true);
                        if (primeraAsignacionDestinoFinal)
                            resultado.Add(MESSAGE, "Se ha registrado con éxito.");
                        else
                            resultado.Add(MESSAGE, "Se ha actualizado con éxito.");
                    }

                    dbContextTransaction.Commit();
                    #endregion

                    #region SE CREA BITACORA
                    SaveBitacora(16, (int)AccionEnum.ACTUALIZAR, objCECaptura.id, JsonUtils.convertNetObjectToJson(objCECaptura));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "MedioAmbienteController", "CrearEditarDestinoFinal", e, AccionEnum.ACTUALIZAR, _objCEDTO.id, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }

        public Dictionary<string, object> EliminarDestinoFinal(int _idCaptura)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region VALIDACIONES
                    if (_idCaptura <= 0)
                        throw new Exception("Ocurrió un error al eliminar el destino final.");
                    #endregion

                    #region SE CAMBIA ESTATUS DE LA CAPTURA A TRAYECTO
                    tblS_MedioAmbienteCaptura objActualizar = _context.tblS_MedioAmbienteCaptura.Where(w => w.id == _idCaptura).FirstOrDefault();
                    if (objActualizar == null)
                        throw new Exception("Ocurrió un error al eliminar el destino final.");
                    else
                    {
                        objActualizar.estatusCaptura = (int)EstatusCapturaEnum.trayecto;
                        objActualizar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objActualizar.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();

                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                    }

                    dbContextTransaction.Commit();
                    #endregion

                    #region SE CREA BITACORA
                    SaveBitacora(16, (int)AccionEnum.ELIMINAR, _idCaptura, JsonUtils.convertNetObjectToJson(objActualizar));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "MedioAmbienteController", "EliminarDestinoFinal", e, AccionEnum.ELIMINAR, _idCaptura, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarDestinoFinal(int _idCaptura)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region SE OBTIENE INFORMACIÓN A ACTUALIZAR DEL DESTINO FINAL SELECCIONADO
                    CapturaDTO objCaptura = _context.Select<CapturaDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT t1.id, t1.fechaDestinoFinal, t1.idTransportistaDestinoFinal, t2.razonSocial, t1.estatusCaptura
	                                            FROM tblS_MedioAmbienteCaptura AS t1
	                                            INNER JOIN tblS_MedioAmbienteTransportistas AS t2 ON t1.idTransportistaDestinoFinal = t2.id
		                                            WHERE t1.esActivo = @esActivo AND t2.esActivo = @esActivo AND t1.id = @id",
                        parametros = new { esActivo = true, id = _idCaptura }
                    }).FirstOrDefault();

                    if (objCaptura == null)
                        resultado.Add("objCaptura", "");
                    else if (objCaptura.estatusCaptura == (int)EstatusCapturaEnum.destinoFinal)
                        resultado.Add("objCaptura", objCaptura);

                    resultado.Add(SUCCESS, true);

                    dbContextTransaction.Commit();
                    #endregion

                    #region SE CREA BITACORA
                    SaveBitacora(16, (int)AccionEnum.CONSULTA, 0, JsonUtils.convertNetObjectToJson(objCaptura));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "MedioAmbienteController", "GetDatosActualizarDestinoFinal", e, AccionEnum.CONSULTA, _idCaptura, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }

        public Dictionary<string, object> GetAspectosAmbientalesToDestinoFinal(int idAgrupacion, string consecutivo)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region SE OBTIENE LOS ASPECTOS AMBIENTES CON ESTATUS TRAYECTO Y DE LA AGRUPACIÓN SELECCIONADA
                    List<AspectosAmbientalesToDestinoFinalDTO> lstAspectosAmbientales = _context.Select<AspectosAmbientalesToDestinoFinalDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT t2.id, t2.codigoContenedor, t2.idAspectoAmbiental, t3.descripcion AS aspectoAmbiental, t3.clasificacion, t2.idCaptura
	                                        FROM tblS_MedioAmbienteCaptura AS t1
	                                        INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
	                                        INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
		                                        WHERE t1.esActivo = @esActivo AND t2.esActivo = @esActivo AND t3.esActivo = @esActivo AND 
                                                      t1.idAgrupacion = @idAgrupacion AND t2.estatusAspectoAmbiental = @estatusAspectoAmbiental AND consecutivo = @consecutivo",
                        parametros = new { esActivo = true, idAgrupacion = idAgrupacion, estatusAspectoAmbiental = (int)EstatusCapturaEnum.trayecto, consecutivo = consecutivo }
                    }).ToList();

                    #region SE VERIFICA QUE ASPECTOS AMBIENTALES SON RP (RESIDUOS PELIGROSOS) Y CUENTAN CON EL ESTATUS DE SOLIDO IMPREGNADO CON HIDROCARBURO
                    List<int> lstAspectosAmbientalesID = lstAspectosAmbientales.Select(s => s.id).ToList();

                    List<int> lstAAClasificacionRP = new List<int>();
                    for (int i = 0; i < lstAspectosAmbientales.Count(); i++)
                    {
                        int idAspectoAmbiental = Convert.ToInt32(lstAspectosAmbientales[i].idAspectoAmbiental);
                        int esRP = _context.tblS_MedioAmbienteAspectoAmbiental.Where(w => w.id == idAspectoAmbiental && w.esSolidoImpregnadoHidrocarburo).Count();
                        if (esRP > 0)
                        {
                            int idCapDet = Convert.ToInt32(lstAspectosAmbientales[i].id);

                            lstAspectosAmbientales[i].quitarDelRow = true;
                            lstAAClasificacionRP.Add(idCapDet);
                        }
                    }
                    #endregion

                    #region EN CASO QUE SE ENCUENTREN RP CON HIDROCARBURO, SE CONCATENA LOS CODIGOS DE RESIDUO EN UN STRING DEL DTO
                    if (lstAAClasificacionRP.Count() > 0)
                    {
                        AspectosAmbientalesToDestinoFinalDTO obj = new AspectosAmbientalesToDestinoFinalDTO();
                        int contador = 0;
                        for (int i = 0; i < lstAAClasificacionRP.Count(); i++)
                        {
                            string codigoContenedor = lstAspectosAmbientales.Where(w => w.id == lstAAClasificacionRP[i]).Select(s => s.codigoContenedor).FirstOrDefault();
                            obj.codigoContenedor += "<br>" + codigoContenedor;

                            contador++;
                            if (contador == lstAAClasificacionRP.Count())
                            {
                                int idRP = lstAspectosAmbientales.Where(w => w.id == lstAAClasificacionRP[i]).Select(s => s.id).FirstOrDefault();
                                string aspectoAmbiental = lstAspectosAmbientales.Where(w => w.id == idRP).Select(s => s.aspectoAmbiental).FirstOrDefault();

                                obj.idRP += "|" + idRP;
                                obj.aspectoAmbiental += aspectoAmbiental;
                            }
                            else
                            {
                                int idRP = lstAspectosAmbientales.Where(w => w.id == lstAAClasificacionRP[i]).Select(s => s.id).FirstOrDefault();
                                string aspectoAmbiental = lstAspectosAmbientales.Where(w => w.id == idRP).Select(s => s.aspectoAmbiental).FirstOrDefault() + ", ";

                                obj.idRP += "|" + idRP;
                                obj.aspectoAmbiental += aspectoAmbiental;
                            }

                        }
                        obj.codigoContenedor = "Solidos Impregnados";
                        obj.esSolido = true;
                        obj.cantidad = 0;
                        lstAspectosAmbientales.Add(obj);
                    }
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add("lstAspectosAmbientales", lstAspectosAmbientales.Where(w => !w.quitarDelRow));
                    dbContextTransaction.Commit();
                    #endregion

                    #region SE CREA BITACORA
                    //SaveBitacora(16, (int)AccionEnum.CONSULTA, 0, JsonUtils.convertNetObjectToJson(lstAspectosAmbientales));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "MedioAmbienteController", "GetAspectosAmbientalesToDestinoFinal", e, AccionEnum.CONSULTA, 0, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }

        public Dictionary<string, object> CrearDestinoFinal(AspectosAmbientalesToDestinoFinalDTO objParamDTO, HttpPostedFileBase objFile)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    List<int> lstID = new List<int>();
                    string lista = string.Empty;
                    if (true)
                    {
                        for (int i = 0; i < objParamDTO.lstAspectosAmbientales.Count(); i++)
                        {
                            lista += "," + objParamDTO.lstAspectosAmbientales[i];
                            string split = objParamDTO.lstAspectosAmbientales[i];
                            List<string> list = new List<string>();
                            list = split.Split('|').ToList();
                            foreach (var l in list)
                            {
                                if (!string.IsNullOrEmpty(l) && Convert.ToInt32(l) > 0)
                                {
                                    lstID.Add(Convert.ToInt32(l));
                                }
                            }
                        }

                        #region SE ACTUALIZA ESTATUS DE LOS ASPECTOS AMBIENTALES SELECCIONADOS DE TRAYECTO A DESTINO FINAL
                        List<tblS_MedioAmbienteCapturaDet> lstAspectosAmbientalesActualizar = _context.tblS_MedioAmbienteCapturaDet.Where(w => lstID.Contains(w.id) && w.esActivo).ToList();
                        foreach (var item in lstAspectosAmbientalesActualizar)
                        {
                            item.estatusAspectoAmbiental = (int)EstatusCapturaEnum.destinoFinal;
                            item.fechaModificacion = DateTime.Now;
                            item.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        }
                        _context.SaveChanges();
                        #endregion

                        #region VERIFICAR ESTATUS DE LOS REGISTROS DETALLE PARA ACTUALIZAR EL REGISTRO PRINCIPAL
                        if (lstAspectosAmbientalesActualizar.Count() > 0)
                        {
                            var idCaptura = lstAspectosAmbientalesActualizar.FirstOrDefault().idCaptura;
                            var listaDetalle = _context.tblS_MedioAmbienteCapturaDet.Where(x => x.esActivo && x.idCaptura == idCaptura).ToList();

                            if (listaDetalle.Count() > 0)
                            {
                                var estatusPrincipal = 0;

                                if (listaDetalle.All(x => x.estatusAspectoAmbiental == (int)EstatusCapturaEnum.acopio))
                                {
                                    estatusPrincipal = (int)EstatusCapturaEnum.acopio;
                                }
                                else if (listaDetalle.All(x => x.estatusAspectoAmbiental == (int)EstatusCapturaEnum.trayecto))
                                {
                                    estatusPrincipal = (int)EstatusCapturaEnum.trayecto;
                                }
                                else if (listaDetalle.All(x => x.estatusAspectoAmbiental == (int)EstatusCapturaEnum.destinoFinal))
                                {
                                    estatusPrincipal = (int)EstatusCapturaEnum.destinoFinal;
                                }
                                else
                                {
                                    estatusPrincipal = listaDetalle.Min(x => x.estatusAspectoAmbiental); //Se escoge el estatus más atrasado.
                                }

                                var registroPrincipal = _context.tblS_MedioAmbienteCaptura.FirstOrDefault(x => x.id == idCaptura);

                                registroPrincipal.estatusCaptura = estatusPrincipal;
                                _context.SaveChanges();
                            }
                        }
                        #endregion

                        #region ARCHIVOS - DESTINO FINAL
                        CapturaDTO obj = new CapturaDTO();
                        if (objFile != null)
                        {
                            obj.id = objParamDTO.id;
                            bool registroSatisfatorio = RegistrarArchivo(obj, objFile, (int)TipoArchivoEnum.destinoFinal);
                            if (!registroSatisfatorio)
                            {
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ocurrió un error al registrar el archivo en el servidor.");
                            }
                        }
                        #endregion

                        var registroArchivo = _context.tblS_MedioAmbienteArchivos.OrderByDescending(x => x.id).FirstOrDefault();

                        #region SE REGISTRA LOS ASPECTOS AMBIENTALES QUE SE ENCUENTRAN EN TRAYECTO
                        List<tblS_MedioAmbienteDestinoFinal> lstAADestinoFinal = new List<tblS_MedioAmbienteDestinoFinal>();
                        for (int i = 0; i < objParamDTO.lstAspectosAmbientales.Count(); i++)
                        {
                            tblS_MedioAmbienteDestinoFinal objAADestinoFinal = new tblS_MedioAmbienteDestinoFinal();
                            objAADestinoFinal.idAgrupacion = objParamDTO.idAgrupacion;
                            objAADestinoFinal.idAspectoAmbiental = lstID[i];
                            objAADestinoFinal.fechaDestinoFinal = objParamDTO.fechaDestinoFinal;
                            objAADestinoFinal.idTransportistaDestinoFinal = objParamDTO.idTransportistaDestinoFinal;
                            objAADestinoFinal.idArchivoTrayecto = registroArchivo.id;
                            objAADestinoFinal.cantidad = objParamDTO.lstCantidad[i];
                            objAADestinoFinal.aaID = lista;
                            objAADestinoFinal.fechaCreacion = DateTime.Now;
                            objAADestinoFinal.fechaModificacion = DateTime.Now;
                            objAADestinoFinal.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objAADestinoFinal.idUsuarioModificacion = 0;
                            objAADestinoFinal.registroActivo = true;
                            _context.tblS_MedioAmbienteDestinoFinal.Add(objAADestinoFinal);
                            _context.SaveChanges();
                        }
                        #endregion

                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha registrado con éxito los AA a estatus Destino final.");
                    }

                    dbContextTransaction.Commit();

                    #region SE CREA BITACORA
                    SaveBitacora(16, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(objParamDTO));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "MedioAmbienteController", "CrearAspectoAmbientalAcopioToTrayecto", e, AccionEnum.CONSULTA, 0, objParamDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }

        #endregion

        #region ARCHIVOS REL CAPTURAS
        public Dictionary<string, object> GetArchivosRelCapturas(CapturaDTO objParamDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE ARCHIVOS CARGADOS A LA CAPTURA
                List<tblS_MedioAmbienteArchivos> lstArchivos = _context.tblS_MedioAmbienteArchivos.Where(w => w.idCaptura == objParamDTO._idCaptura && w.registroActivo).ToList();

                ArchivoDTO objArchivoDTO = new ArchivoDTO();
                List<ArchivoDTO> lstArchivosDTO = new List<ArchivoDTO>();
                foreach (var item in lstArchivos)
                {
                    objArchivoDTO = new ArchivoDTO();
                    objArchivoDTO.id = item.id;
                    objArchivoDTO.idCaptura = item.idCaptura;
                    objArchivoDTO.nombreArchivo = item.nombreArchivo.Trim().ToUpper();
                    objArchivoDTO.tipoArchivo = item.tipoArchivo;
                    objArchivoDTO.tipoArchivoDesc = EnumHelper.GetDescription((TipoArchivoEnum)item.tipoArchivo);
                    lstArchivosDTO.Add(objArchivoDTO);
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("lstArchivos", lstArchivosDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetArchivosRelCapturas", e, AccionEnum.CONSULTA, objParamDTO._idCaptura, objParamDTO);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> VisualizarArchivo(int idArchivo)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE VISUALIZA EL ARCHIVO CARGADO
                tblS_MedioAmbienteArchivos objArchivo = _context.tblS_MedioAmbienteArchivos.Where(w => w.id == idArchivo && w.registroActivo).FirstOrDefault();

                Stream fileStream = GlobalUtils.GetFileAsStream(objArchivo.rutaArchivo);
                var byteArray = GlobalUtils.ConvertFileToByte(fileStream);

                resultado.Add("archivo", byteArray);
                resultado.Add("extension", Path.GetExtension(objArchivo.rutaArchivo).ToUpper());
                resultado.Add(SUCCESS, true);
                #endregion
            }

            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }
        #endregion
        #endregion

        #region TRANSPORTISTAS
        public Dictionary<string, object> GetTransportistas(TransportistasDTO _objFiltroDTO)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region SE OBTIENE LISTADO DE TRANSPORTISTAS
                    List<TransportistasDTO> lstTransportistasDTO = _context.Select<TransportistasDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT t1.id, t1.razonSocial, t1.numAutorizacion, t1.idClasificacion, t2.clasificacion
	                                        FROM tblS_MedioAmbienteTransportistas AS t1
	                                        INNER JOIN tblS_MedioAmbienteClasificacionesTransportistas AS t2 ON t1.idClasificacion = t2.id
		                                        WHERE t1.esActivo = @esActivo AND t2.esActivo = @esActivo",
                        parametros = new { esActivo = true }
                    }).ToList();

                    resultado.Add("data", lstTransportistasDTO);
                    resultado.Add(SUCCESS, true);

                    dbContextTransaction.Commit();
                    #endregion

                    #region SE CREA BITACORA
                    SaveBitacora(16, (int)AccionEnum.CONSULTA, 0, JsonUtils.convertNetObjectToJson(lstTransportistasDTO));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "MedioAmbienteController", "GetTransportistas", e, AccionEnum.CONSULTA, 0, _objFiltroDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }

        public Dictionary<string, object> CrearEditarTransportista(TransportistasDTO _objTransportistaDTO)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region VALIDACIÓN DE CAMPOS VACIOS
                    bool errorCrearEditar = false;
                    string strMensajeError = string.Empty;
                    if (string.IsNullOrEmpty(_objTransportistaDTO.razonSocial) || string.IsNullOrEmpty(_objTransportistaDTO.numAutorizacion) || _objTransportistaDTO.idClasificacion <= 0)
                    {
                        errorCrearEditar = true;
                        strMensajeError += string.IsNullOrEmpty(_objTransportistaDTO.razonSocial) ? "Es necesario indicar la razón social." : string.Empty;
                        strMensajeError += string.IsNullOrEmpty(_objTransportistaDTO.numAutorizacion) ? "<br>Es necesario indicar número de autorización." : string.Empty;
                        strMensajeError += _objTransportistaDTO.idClasificacion <= 0 ? "<br>Es necesario indicar la clasificación." : string.Empty;

                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, strMensajeError);
                    }
                    #endregion

                    #region CREAR/EDITAR TRANSPORTISTA
                    tblS_MedioAmbienteTransportistas objCETransportista = new tblS_MedioAmbienteTransportistas();
                    if (_objTransportistaDTO.id > 0 && !errorCrearEditar)
                    {
                        #region ACTUALIZAR TRANSPORTISTA
                        objCETransportista = _context.tblS_MedioAmbienteTransportistas.Where(w => w.id == _objTransportistaDTO.id).FirstOrDefault();
                        if (objCETransportista == null)
                            throw new Exception("Ocurrió un error al actualizar el registro.");

                        objCETransportista.razonSocial = _objTransportistaDTO.razonSocial.Trim();
                        objCETransportista.numAutorizacion = _objTransportistaDTO.numAutorizacion;
                        objCETransportista.idClasificacion = _objTransportistaDTO.idClasificacion;
                        objCETransportista.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCETransportista.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();

                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha actualizado con éxito al transportista.");
                        #endregion
                    }
                    else if (!errorCrearEditar)
                    {
                        #region GUARDAR NUEVO TRANSPORTISTA
                        objCETransportista.razonSocial = _objTransportistaDTO.razonSocial.Trim();
                        objCETransportista.numAutorizacion = _objTransportistaDTO.numAutorizacion;
                        objCETransportista.idClasificacion = _objTransportistaDTO.idClasificacion;
                        objCETransportista.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCETransportista.idUsuarioModificacion = 0;
                        objCETransportista.fechaCreacion = DateTime.Now;
                        objCETransportista.fechaModificacion = new DateTime(2000, 01, 01);
                        objCETransportista.esActivo = true;
                        _context.tblS_MedioAmbienteTransportistas.Add(objCETransportista);
                        _context.SaveChanges();

                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha registrado con éxito al transportista.");
                        #endregion
                    }

                    dbContextTransaction.Commit();
                    #endregion

                    #region SE CREA BITACORA
                    int idTransportista = _objTransportistaDTO.id > 0 ? _objTransportistaDTO.id : 0;
                    if (idTransportista == 0)
                    {
                        idTransportista = _context.Select<int>(new DapperDTO
                        {
                            baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                            consulta = @"SELECT id FROM tblS_MedioAmbienteTransportistas WHERE razonSocial = '@razonSocial' AND esActivo = @esActivo ORDER BY id DESC",
                            parametros = new { razonSocial = _objTransportistaDTO.razonSocial.Trim(), esActivo = true }
                        }).FirstOrDefault();
                    }
                    SaveBitacora(16, _objTransportistaDTO.id > 0 ? (int)AccionEnum.ACTUALIZAR : (int)AccionEnum.AGREGAR, idTransportista, JsonUtils.convertNetObjectToJson(objCETransportista));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "MedioAmbienteController", "CrearEditarTransportista", e, _objTransportistaDTO.id > 0 ? AccionEnum.ACTUALIZAR : AccionEnum.AGREGAR, _objTransportistaDTO.id > 0 ? _objTransportistaDTO.id : 0, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }

        public Dictionary<string, object> EliminarTransportista(int _idTransportista)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region VALIDACIONES
                    if (_idTransportista <= 0)
                        throw new Exception("Ocurrió un error al eliminar al transportista.");
                    #endregion

                    #region SE ELIMINA AL TRANSPORTISTA SELECCIONADO
                    tblS_MedioAmbienteTransportistas objEliminar = _context.tblS_MedioAmbienteTransportistas.Where(w => w.id == _idTransportista).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception("Ocurrió un error al eliminar al transportista.");
                    else
                    {
                        objEliminar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objEliminar.fechaModificacion = DateTime.Now;
                        objEliminar.esActivo = false;
                        _context.SaveChanges();

                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                    }

                    dbContextTransaction.Commit();
                    #endregion

                    #region SE CREA BITACORA
                    SaveBitacora(16, (int)AccionEnum.ELIMINAR, _idTransportista, JsonUtils.convertNetObjectToJson(objEliminar));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "MedioAmbienteController", "EliminarTransportista", e, AccionEnum.ELIMINAR, _idTransportista, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }

        public Dictionary<string, object> FillCboClasificacionesTransportistas()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL COMBO CLASIFICACIONES TRANSPORTISTAS
                List<ComboDTO> lstClasificaciones = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT id AS Value, clasificacion AS Text FROM tblS_MedioAmbienteClasificacionesTransportistas WHERE esActivo = @esActivo",
                    parametros = new { esActivo = true }
                });
                resultado.Add(ITEMS, lstClasificaciones);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(16, 16, "MedioAmbienteController", "FillCboClasificacionesTransportistas", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #region CLASIFICACIÓN TRANSPORTISTAS
        public Dictionary<string, object> GetClasificacionesTransportistas(ClasificacionTransportistaDTO _objFiltroDTO)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region SE OBTIENE LISTADO DE CLASIFICACIONES TRANSPORTISTAS
                    List<ClasificacionTransportistaDTO> lstClasificacionesTransportistasDTO = _context.Select<ClasificacionTransportistaDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT id, clasificacion, descripcion FROM tblS_MedioAmbienteClasificacionesTransportistas WHERE esActivo = @esActivo",
                        parametros = new { esActivo = true }
                    }).ToList();

                    resultado.Add("data", lstClasificacionesTransportistasDTO);
                    resultado.Add(SUCCESS, true);

                    dbContextTransaction.Commit();
                    #endregion

                    #region SE CREA BITACORA
                    SaveBitacora(16, (int)AccionEnum.CONSULTA, 0, JsonUtils.convertNetObjectToJson(lstClasificacionesTransportistasDTO));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "MedioAmbienteController", "GetClasificacionesTransportistas", e, AccionEnum.CONSULTA, 0, _objFiltroDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }

        public Dictionary<string, object> CrearEditarClasificacionTransportista(ClasificacionTransportistaDTO _objClasificacionTransportistaDTO)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region VALIDACIÓN DE CAMPOS VACIOS
                    bool errorCrearEditar = false;
                    string strMensajeError = string.Empty;
                    if (string.IsNullOrEmpty(_objClasificacionTransportistaDTO.clasificacion))
                    {
                        errorCrearEditar = true;
                        strMensajeError += string.IsNullOrEmpty(_objClasificacionTransportistaDTO.clasificacion) ? "Es necesario indicar la clasificación." : string.Empty;

                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, strMensajeError);
                    }
                    #endregion

                    #region CREAR/EDITAR CLASIFICACIÓN TRANSPORTISTA
                    tblS_MedioAmbienteClasificacionesTransportistas objCEClasificacionTransportista = new tblS_MedioAmbienteClasificacionesTransportistas();
                    if (_objClasificacionTransportistaDTO.id > 0 && !errorCrearEditar)
                    {
                        #region ACTUALIZAR CLASIFICACIÓN TRANSPORTISTA
                        objCEClasificacionTransportista = _context.tblS_MedioAmbienteClasificacionesTransportistas.Where(w => w.id == _objClasificacionTransportistaDTO.id).FirstOrDefault();
                        if (objCEClasificacionTransportista == null)
                            throw new Exception("Ocurrió un error al actualizar el registro.");

                        objCEClasificacionTransportista.clasificacion = _objClasificacionTransportistaDTO.clasificacion.Trim();
                        objCEClasificacionTransportista.descripcion = !string.IsNullOrEmpty(_objClasificacionTransportistaDTO.descripcion) ? _objClasificacionTransportistaDTO.descripcion.Trim() : string.Empty;
                        objCEClasificacionTransportista.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCEClasificacionTransportista.fechaModificacion = DateTime.Now;
                        objCEClasificacionTransportista.esActivo = true;
                        _context.SaveChanges();

                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha actualizado con éxito la clasificación.");
                        #endregion
                    }
                    else if (!errorCrearEditar)
                    {
                        #region GUARDAR NUEVO TRANSPORTISTA
                        objCEClasificacionTransportista.clasificacion = _objClasificacionTransportistaDTO.clasificacion.Trim();
                        objCEClasificacionTransportista.descripcion = !string.IsNullOrEmpty(_objClasificacionTransportistaDTO.descripcion) ? _objClasificacionTransportistaDTO.descripcion.Trim() : string.Empty;
                        objCEClasificacionTransportista.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCEClasificacionTransportista.idUsuarioModificacion = 0;
                        objCEClasificacionTransportista.fechaCreacion = DateTime.Now;
                        objCEClasificacionTransportista.fechaModificacion = new DateTime(2000, 01, 01);
                        objCEClasificacionTransportista.esActivo = true;
                        _context.tblS_MedioAmbienteClasificacionesTransportistas.Add(objCEClasificacionTransportista);
                        _context.SaveChanges();

                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha registrado con éxito la clasificación.");
                        #endregion
                    }

                    dbContextTransaction.Commit();
                    #endregion

                    #region SE CREA BITACORA
                    int idClasificacionTransportista = _objClasificacionTransportistaDTO.id > 0 ? _objClasificacionTransportistaDTO.id : 0;
                    if (idClasificacionTransportista == 0)
                    {
                        idClasificacionTransportista = _context.Select<int>(new DapperDTO
                        {
                            baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                            consulta = @"SELECT id FROM tblS_MedioAmbienteClasificacionesTransportistas WHERE clasificacion = '@clasificacion' esActivo = @esActivo ORDER BY id DESC",
                            parametros = new { clasificacion = _objClasificacionTransportistaDTO.clasificacion.Trim(), esActivo = true }
                        }).FirstOrDefault();
                    }

                    SaveBitacora(16, _objClasificacionTransportistaDTO.id > 0 ? (int)AccionEnum.ACTUALIZAR : (int)AccionEnum.AGREGAR, idClasificacionTransportista, JsonUtils.convertNetObjectToJson(objCEClasificacionTransportista));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "MedioAmbienteController", "CrearEditarClasificacionTransportista", e,
                            _objClasificacionTransportistaDTO.id > 0 ? AccionEnum.ACTUALIZAR : AccionEnum.AGREGAR, _objClasificacionTransportistaDTO.id > 0 ? _objClasificacionTransportistaDTO.id : 0, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }

        public Dictionary<string, object> EliminarClasificacionTransportista(int _idClasificacionTransportista)
        {
            resultado = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region VALIDACIONES
                    if (_idClasificacionTransportista <= 0)
                        throw new Exception("Ocurrió un error al eliminar la categoría del transportista.");
                    #endregion

                    #region SE ELIMINA LA CATEGORÍA DEL TRANSPORTISTA SELECCIONADO
                    tblS_MedioAmbienteClasificacionesTransportistas objEliminar = _context.tblS_MedioAmbienteClasificacionesTransportistas.Where(w => w.id == _idClasificacionTransportista).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception("Ocurrió un error al eliminar la categoría del transportista.");
                    else
                    {
                        objEliminar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objEliminar.fechaModificacion = DateTime.Now;
                        objEliminar.esActivo = false;
                        _context.SaveChanges();

                        resultado.Add(SUCCESS, true);
                        resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                    }

                    dbContextTransaction.Commit();
                    #endregion

                    #region SE CREA BITACORA
                    SaveBitacora(16, (int)AccionEnum.ELIMINAR, _idClasificacionTransportista, JsonUtils.convertNetObjectToJson(objEliminar));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "MedioAmbienteController", "EliminarClasificacionTransportista", e, AccionEnum.ELIMINAR, _idClasificacionTransportista, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            return resultado;
        }
        #endregion

        #region DASHBOARD
        public Dictionary<string, object> GetGraficas(FiltroDTO objFiltroDTO)
        {
            try
            {
                #region SE OBTIENE EL ULTIMO DÍA DEL MES

                #region fecha acual en filtro
                DateTime mesInicio = new DateTime(objFiltroDTO.fechaInicio.Year, objFiltroDTO.fechaInicio.Month, 1);
                DateTime mesFinal = objFiltroDTO.fechaFin.AddMonths(1).AddDays(-1);

                string ddInicio = Convert.ToInt32(mesInicio.Day) >= 1 && Convert.ToInt32(mesInicio.Day) <= 9 ? "0" + mesInicio.Day.ToString() : mesInicio.Day.ToString();
                string mmInicio = Convert.ToInt32(mesInicio.Month) >= 1 && Convert.ToInt32(mesInicio.Month) <= 9 ? "0" + mesInicio.Month.ToString() : mesInicio.Month.ToString();
                string fechaInicio = mesInicio.Year + "-" + mmInicio + "-" + ddInicio + "T00:00:00";

                string ddFinal = Convert.ToInt32(mesFinal.Day) >= 1 && Convert.ToInt32(mesFinal.Day) <= 9 ? "0" + mesFinal.Day.ToString() : mesFinal.Day.ToString();
                string mmFinal = Convert.ToInt32(mesFinal.Month) >= 1 && Convert.ToInt32(mesFinal.Month) <= 9 ? "0" + mesFinal.Month.ToString() : mesFinal.Month.ToString();
                string fechaFinal = mesFinal.Year + "-" + mmFinal + "-" + ddFinal + "T00:00:00";
                #endregion

                #region fija año anterior
                DateTime mesInicio2022 = objFiltroDTO.fechaInicio.AddYears(-1);
                DateTime mesFinal2022 = objFiltroDTO.fechaFin.AddMonths(1).AddDays(-1).AddYears(-1);

                string ddInicio2022 = Convert.ToInt32(mesInicio2022.Day) >= 1 && Convert.ToInt32(mesInicio2022.Day) <= 9 ? "0" + mesInicio2022.Day.ToString() : mesInicio2022.Day.ToString();
                string mmInicio2022 = Convert.ToInt32(mesInicio2022.Month) >= 1 && Convert.ToInt32(mesInicio2022.Month) <= 9 ? "0" + mesInicio2022.Month.ToString() : mesInicio2022.Month.ToString();
                string fechaInicio2022 = mesInicio2022.Year + "-" + mmInicio2022 + "-" + ddInicio2022 + "T00:00:00";

                string ddFinal2022 = Convert.ToInt32(mesFinal2022.Day) >= 1 && Convert.ToInt32(mesFinal2022.Day) <= 9 ? "0" + mesFinal2022.Day.ToString() : mesFinal2022.Day.ToString();
                string mmFinal2022 = Convert.ToInt32(mesFinal2022.Month) >= 1 && Convert.ToInt32(mesFinal2022.Month) <= 9 ? "0" + mesFinal2022.Month.ToString() : mesFinal2022.Month.ToString();
                string fechaFinal2022 = mesFinal2022.Year + "-" + mmFinal2022 + "-" + ddFinal2022 + "T00:00:00";
                #endregion

       
                #endregion

                #region SE OBTIENE HORAS HOMBRE EN BASE A LA AGRUPACIÓN SELECCIONADA CODIGO para fecha que se ponga en el filtro
                string strQuery = string.Empty;

                strQuery = string.Format(@"SELECT *
	                                    FROM tblS_IncidentesInformacionColaboradores
		                                    WHERE (fechaInicio BETWEEN '{0}' AND '{1}')", fechaInicio, fechaFinal);

                var resultHHFechaFiltro = _context.Select<tblS_IncidentesInformacionColaboradores>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery
                });

                #region Filtrar por division y lineas de negocios
                //if (objFiltroDTO.listaDivisiones != null)
                //{
                //    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaDivisiones.Contains(x.division)).ToList();

                //    resultHHFechaFiltro = resultHHFechaFiltro.Join(
                //        listaCentrosCostoDivision,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}

                //if (objFiltroDTO.listaLineasNegocio != null)
                //{
                //    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                //    resultHHFechaFiltro = resultHHFechaFiltro.Join(
                //        listaCentrosCostoDivisionLineaNegocio,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}
                #endregion

                //if (objFiltroDTO.idAgrupacion > 0)
                //{
                //    resultHHFechaFiltro = resultHHFechaFiltro.Where(x => x.idAgrupacion == objFiltroDTO.idAgrupacion).ToList();
                //}

                decimal HHFechaFiltro = 0;

                if (resultHHFechaFiltro.Count() > 0)
                {
                    HHFechaFiltro = resultHHFechaFiltro.Sum(x => x.horasHombre);
                }            

                #endregion

                #endregion

                #region SE OBTIENE HORAS HOMBRE EN BASE A LA AGRUPACIÓN SELECCIONADA CODIGO para fecha año anterior
               var strQuery2= string.Format(@"SELECT *
	                                    FROM tblS_IncidentesInformacionColaboradores
		                                    WHERE (fechaInicio BETWEEN '{0}' AND '{1}')", fechaInicio2022, fechaFinal2022);

                var resultHH2022 = _context.Select<tblS_IncidentesInformacionColaboradores>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery2
                });

                #region Filtrar por division y lineas de negocios
                //if (objFiltroDTO.listaDivisiones != null)
                //{
                //    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaDivisiones.Contains(x.division)).ToList();

                //    resultHH2022 = resultHH2022.Join(
                //        listaCentrosCostoDivision,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}

                //if (objFiltroDTO.listaLineasNegocio != null)
                //{
                //    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                //    resultHH2022 = resultHH2022.Join(
                //        listaCentrosCostoDivisionLineaNegocio,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}
                #endregion

                //if (objFiltroDTO.idAgrupacion > 0)
                //{
                //    resultHH2022 = resultHH2022.Where(x => x.idAgrupacion == objFiltroDTO.idAgrupacion).ToList();
                //}

                decimal HH2022 = 0;

                if (resultHH2022 != null)
                {
                    HH2022 = resultHH2022.Sum(x => x.horasHombre);
                }
         
                #endregion
               
                #region SE OBTIENE CATALOGO DE CLASIFICICACIÓN DE RESIDUOS
                List<tblS_MedioAmbienteClasificacion> lstClasificacionResiduos = _context.Select<tblS_MedioAmbienteClasificacion>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT descripcion FROM tblS_MedioAmbienteClasificacion WHERE esActivo = @esActivo ORDER BY descripcion",
                    parametros = new { esActivo = true }
                }).ToList();
                #endregion

                #region SE OBTIENE LA CANTIDADES DE REGISTROS

                #region GENERACIÓN DE RESIDUOS BIOLOGICO INFECCIOSOS
               
                #region año actual del filtro
                List<CapturaDTO> lstCapturasRBIFechaFiltro = _context.Select<CapturaDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT t1.idAgrupacion, t1.fechaEntrada
                                            FROM tblS_MedioAmbienteCaptura AS t1 
                                            INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
		                                    INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
		                                    INNER JOIN tblS_MedioAmbienteClasificacion AS t4 ON t3.clasificacion = t4.id
			                                    WHERE t1.esActivo = @esActivo AND t4.id = @tipoResiduo AND (t1.fechaEntrada BETWEEN @fechaInicio AND @fechaFinal)
                                        GROUP BY t1.idAgrupacion, t1.fechaEntrada",
                    parametros = new { esActivo = true, tipoResiduo = 2, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                }).ToList();

                //lstCapturasRBIFechaFiltro = lstCapturasRBIFechaFiltro.Where(x => (objFiltroDTO.idAgrupacion > 0 ? x.idAgrupacion == objFiltroDTO.idAgrupacion : true) && x.fechaEntrada.Date >= mesInicio.Date && x.fechaEntrada.Date <= mesFinal.Date).ToList();

                #region Filtrar por division y lineas de negocios
                //if (objFiltroDTO.listaDivisiones != null)
                //{
                //    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaDivisiones.Contains(x.division)).ToList();

                //    lstCapturasRBIFechaFiltro = lstCapturasRBIFechaFiltro.Join(
                //        listaCentrosCostoDivision,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}

                //if (objFiltroDTO.listaLineasNegocio != null)
                //{
                //    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                //    lstCapturasRBIFechaFiltro = lstCapturasRBIFechaFiltro.Join(
                //        listaCentrosCostoDivisionLineaNegocio,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}
                #endregion
               
                #endregion
              
                #region año anterior del filtro

                List<CapturaDTO> lstCapturasRBI2022 = _context.Select<CapturaDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT t1.idAgrupacion, t1.idEmpresa, t1.fechaEntrada
                                                                FROM tblS_MedioAmbienteCaptura AS t1 
                                                                INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
		                                                        INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
		                                                        INNER JOIN tblS_MedioAmbienteClasificacion AS t4 ON t3.clasificacion = t4.id
			                                                        WHERE t1.esActivo = @esActivo AND t4.id = @tipoResiduo AND (t1.fechaEntrada BETWEEN @fechaInicio AND @fechaFinal)
                                                            GROUP BY t1.idAgrupacion, t1.fechaEntrada",
                        parametros = new { esActivo = true, tipoResiduo = 2, fechaInicio = fechaInicio2022, fechaFinal = fechaFinal2022 }
                    }).ToList();

                //lstCapturasRBI2022 = lstCapturasRBI2022.Where(x => (objFiltroDTO.idAgrupacion > 0 ? x.idAgrupacion == objFiltroDTO.idAgrupacion : true) && x.fechaEntrada.Date >= mesInicio.Date && x.fechaEntrada.Date <= mesFinal.Date).ToList();

                #region Filtrar por division y lineas de negocios
                //if (objFiltroDTO.listaDivisiones != null)
                //{
                //    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaDivisiones.Contains(x.division)).ToList();

                //    lstCapturasRBI2022 = lstCapturasRBI2022.Join(
                //        listaCentrosCostoDivision,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}

                //if (objFiltroDTO.listaLineasNegocio != null)
                //{
                //    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                //    lstCapturasRBI2022 = lstCapturasRBI2022.Join(
                //        listaCentrosCostoDivisionLineaNegocio,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}

                #endregion

                #endregion
           
                #endregion
               
                #region GENERACIÓN DE RESIDUOS PELIGROSOS
              
                #region año actual del filtro
                List<CapturaDTO> lstCapturasRP2023 = _context.Select<CapturaDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT t1.idAgrupacion, t1.fechaEntrada
                                            FROM tblS_MedioAmbienteCaptura AS t1 
                                            INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
		                                    INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
		                                    INNER JOIN tblS_MedioAmbienteClasificacion AS t4 ON t3.clasificacion = t4.id
			                                    WHERE t1.esActivo = @esActivo AND t4.id = @tipoResiduo AND (t1.fechaEntrada BETWEEN @fechaInicio AND @fechaFinal)
                                        GROUP BY t1.idAgrupacion, t1.fechaEntrada",
                    parametros = new { esActivo = true, tipoResiduo = 3, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                }).ToList();

                //lstCapturasRP2023 = lstCapturasRP2023.Where(x => (objFiltroDTO.idAgrupacion > 0 ? x.idAgrupacion == objFiltroDTO.idAgrupacion : true) && x.fechaEntrada.Date >= mesInicio.Date && x.fechaEntrada.Date <= mesFinal.Date).ToList();
             
                #region Filtrar por division y lineas de negocios
                //if (objFiltroDTO.listaDivisiones != null)
                //{
                //    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaDivisiones.Contains(x.division)).ToList();

                //    lstCapturasRP2023 = lstCapturasRP2023.Join(
                //        listaCentrosCostoDivision,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}

                //if (objFiltroDTO.listaLineasNegocio != null)
                //{
                //    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                //    lstCapturasRP2023 = lstCapturasRP2023.Join(
                //        listaCentrosCostoDivisionLineaNegocio,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}
                #endregion
                #endregion

                #region fecha anterior del filtro
                List<CapturaDTO> lstCapturasRP2022 = _context.Select<CapturaDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT t1.idAgrupacion, t1.fechaEntrada
                                                            FROM tblS_MedioAmbienteCaptura AS t1 
                                                            INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
                		                                    INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
                		                                    INNER JOIN tblS_MedioAmbienteClasificacion AS t4 ON t3.clasificacion = t4.id
                			                                    WHERE t1.esActivo = @esActivo AND t4.id = @tipoResiduo AND (t1.fechaEntrada BETWEEN @fechaInicio AND @fechaFinal)
                                                        GROUP BY t1.idAgrupacion, t1.fechaEntrada",
                    parametros = new { esActivo = true, tipoResiduo = 3, fechaInicio = fechaInicio2022, fechaFinal = fechaFinal2022 }
                }).ToList();

                //lstCapturasRP2022 = lstCapturasRP2022.Where(x => (objFiltroDTO.idAgrupacion > 0 ? x.idAgrupacion == objFiltroDTO.idAgrupacion : true) && x.fechaEntrada.Date >= mesInicio.Date && x.fechaEntrada.Date <= mesFinal.Date).ToList();

                #region Filtrar por division y lineas de negocios
                //if (objFiltroDTO.listaDivisiones != null)
                //{
                //    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaDivisiones.Contains(x.division)).ToList();

                //    lstCapturasRP2022 = lstCapturasRP2022.Join(
                //        listaCentrosCostoDivision,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}

                //if (objFiltroDTO.listaLineasNegocio != null)
                //{
                //    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                //    lstCapturasRP2022 = lstCapturasRP2022.Join(
                //        listaCentrosCostoDivisionLineaNegocio,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}
                #endregion
                #endregion

                #endregion

                #region GENERACIÓN DE RESIDUOS DE MANEJO ESPECIAL

                #region Codigo Actual del filtro
                List<CapturaDTO> lstCapturasRME2023 = _context.Select<CapturaDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT t1.idAgrupacion, t1.fechaEntrada
                                            FROM tblS_MedioAmbienteCaptura AS t1 
                                            INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
		                                    INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
		                                    INNER JOIN tblS_MedioAmbienteClasificacion AS t4 ON t3.clasificacion = t4.id
			                                    WHERE t1.esActivo = @esActivo AND t4.id = @tipoResiduo AND (t1.fechaEntrada BETWEEN @fechaInicio AND @fechaFinal)
                                        GROUP BY t1.idAgrupacion, t1.fechaEntrada",
                    parametros = new { esActivo = true, tipoResiduo = 4, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                }).ToList();

                //lstCapturasRME2023 = lstCapturasRME2023.Where(x => (objFiltroDTO.idAgrupacion > 0 ? x.idAgrupacion == objFiltroDTO.idAgrupacion : true) && x.fechaEntrada.Date >= mesInicio.Date && x.fechaEntrada.Date <= mesFinal.Date).ToList();

                #region Filtrar por division y lineas de negocios
                //if (objFiltroDTO.listaDivisiones != null)
                //{
                //    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaDivisiones.Contains(x.division)).ToList();

                //    lstCapturasRME2023 = lstCapturasRME2023.Join(
                //        listaCentrosCostoDivision,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}

                //if (objFiltroDTO.listaLineasNegocio != null)
                //{
                //    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                //    lstCapturasRME2023 = lstCapturasRME2023.Join(
                //        listaCentrosCostoDivisionLineaNegocio,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}
                #endregion
                #endregion               
             
                #region año anterior del filtro
                List<CapturaDTO> lstCapturasRME2022 = _context.Select<CapturaDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT t1.idAgrupacion, t1.fechaEntrada
                                                            FROM tblS_MedioAmbienteCaptura AS t1 
                                                            INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
                		                                    INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
                		                                    INNER JOIN tblS_MedioAmbienteClasificacion AS t4 ON t3.clasificacion = t4.id
                			                                    WHERE t1.esActivo = @esActivo AND t4.id = @tipoResiduo AND (t1.fechaEntrada BETWEEN @fechaInicio AND @fechaFinal)
                                                        GROUP BY t1.idAgrupacion, t1.fechaEntrada",
                    parametros = new { esActivo = true, tipoResiduo = 4, fechaInicio = fechaInicio2022, fechaFinal = fechaFinal2022 }
                }).ToList();

                //lstCapturasRME2022 = lstCapturasRME2022.Where(x => (objFiltroDTO.idAgrupacion > 0 ? x.idAgrupacion == objFiltroDTO.idAgrupacion : true) && x.fechaEntrada.Date >= mesInicio.Date && x.fechaEntrada.Date <= mesFinal.Date).ToList();

                #region Filtrar por division y lineas de negocios
                //if (objFiltroDTO.listaDivisiones != null)
                //{
                //    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaDivisiones.Contains(x.division)).ToList();

                //    lstCapturasRME2022 = lstCapturasRME2022.Join(
                //        listaCentrosCostoDivision,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}

                //if (objFiltroDTO.listaLineasNegocio != null)
                //{
                //    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                //    lstCapturasRME2022 = lstCapturasRME2022.Join(
                //        listaCentrosCostoDivisionLineaNegocio,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}
                #endregion
                #endregion
               
                #endregion

                #region GENERACIÓN DE RESIDUOS SOLIDOS URBANOS

                #region codigo actual filtro

                List<CapturaDTO> lstCapturasRSU2023 = _context.Select<CapturaDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT t1.idAgrupacion, t1.fechaEntrada
                                            FROM tblS_MedioAmbienteCaptura AS t1 
                                            INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
		                                    INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
		                                    INNER JOIN tblS_MedioAmbienteClasificacion AS t4 ON t3.clasificacion = t4.id
			                                    WHERE t1.esActivo = @esActivo AND t4.id = @tipoResiduo AND (t1.fechaEntrada BETWEEN @fechaInicio AND @fechaFinal)
                                        GROUP BY t1.idAgrupacion, t1.fechaEntrada",
                    parametros = new { esActivo = true, tipoResiduo = 5, fechaInicio = fechaInicio, fechaFinal = fechaFinal }
                }).ToList();

                //lstCapturasRSU2023 = lstCapturasRSU2023.Where(x => (objFiltroDTO.idAgrupacion > 0 ? x.idAgrupacion == objFiltroDTO.idAgrupacion : true) && x.fechaEntrada.Date >= mesInicio.Date && x.fechaEntrada.Date <= mesFinal.Date).ToList();

                #region Filtrar por division y lineas de negocios
                //if (objFiltroDTO.listaDivisiones != null)
                //{
                //    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaDivisiones.Contains(x.division)).ToList();

                //    lstCapturasRSU2023 = lstCapturasRSU2023.Join(
                //        listaCentrosCostoDivision,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}

                //if (objFiltroDTO.listaLineasNegocio != null)
                //{
                //    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                //    lstCapturasRSU2023 = lstCapturasRSU2023.Join(
                //        listaCentrosCostoDivisionLineaNegocio,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}
                #endregion

                #endregion

                #region Año anterior del filtro
                List<CapturaDTO> lstCapturasRSU2022 = _context.Select<CapturaDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT t1.idAgrupacion, t1.fechaEntrada
                                                                            FROM tblS_MedioAmbienteCaptura AS t1 
                                                                            INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
                                		                                    INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
                                		                                    INNER JOIN tblS_MedioAmbienteClasificacion AS t4 ON t3.clasificacion = t4.id
                                			                                    WHERE t1.esActivo = @esActivo AND t4.id = @tipoResiduo AND (t1.fechaEntrada BETWEEN @fechaInicio AND @fechaFinal)
                                                                        GROUP BY t1.idAgrupacion, t1.fechaEntrada",
                    parametros = new { esActivo = true, tipoResiduo = 5, fechaInicio = fechaInicio2022, fechaFinal = fechaFinal2022 }
                }).ToList();

                //lstCapturasRSU2022 = lstCapturasRSU2022.Where(x => (objFiltroDTO.idAgrupacion > 0 ? x.idAgrupacion == objFiltroDTO.idAgrupacion : true) && x.fechaEntrada.Date >= mesInicio.Date && x.fechaEntrada.Date <= mesFinal.Date).ToList();

                #region Filtrar por division y lineas de negocios
                //if (objFiltroDTO.listaDivisiones != null)
                //{
                //    var listaCentrosCostoDivision = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaDivisiones.Contains(x.division)).ToList();

                //    lstCapturasRSU2022 = lstCapturasRSU2022.Join(
                //        listaCentrosCostoDivision,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}

                //if (objFiltroDTO.listaLineasNegocio != null)
                //{
                //    var listaCentrosCostoDivisionLineaNegocio = _context.tblS_Req_CentroCostoDivision.Where(x => x.estatus && objFiltroDTO.listaLineasNegocio.Contains(x.lineaNegocio_id)).ToList(); //Se asume la división ya que una linea de negocio nomás puede pertenecer a una división.

                //    lstCapturasRSU2022 = lstCapturasRSU2022.Join(
                //        listaCentrosCostoDivisionLineaNegocio,
                //        r => new { r.idEmpresa, r.idAgrupacion },
                //        cd => new { cd.idEmpresa, idAgrupacion = (int)cd.idAgrupacion },
                //        (r, cd) => new { r, cd }
                //    ).Select(x => x.r).ToList();
                //}
                #endregion
                #endregion

                #endregion

                #endregion

                #region GRAFICA BARRAS DTO
                GraficaBarrasIndiceDTO objGraficaBarrasDTO = new GraficaBarrasIndiceDTO();

                #region SE OBTIENE LOS MESES A MOSTRAR
                string[] arrMonths = new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
                int numMesInicial = Convert.ToInt32(mmInicio) - 1;
                int numMesFinal = Convert.ToInt32(mmFinal);
                int totalMesesMostrar = numMesFinal - numMesInicial;
                for (int i = 0; i < totalMesesMostrar; i++)
                {
                    objGraficaBarrasDTO.lstCategorias.Add(arrMonths[numMesInicial]);
                    numMesInicial++;
                }
                #endregion

                #region SE INDICA EN QUE MESES FUERON REALIZADOS LOS REGISTROS
                decimal generacionTotalActual = 0, generacionTotal2022, indiceActual = 0, indice2022 = 0;
                int numMes = 0;
                int cantRBI = 0, cantRP = 0, cantRME = 0, cantRSU = 0;
                numMes = Convert.ToInt32(mmInicio);
                for (int i = 0; i < totalMesesMostrar; i++)
                {
                    switch (numMes)
                    {
                        case 1:
                            #region ENERO
                            #region SE OBTIENE INDICE 2021
                            objGraficaBarrasDTO.lstDataRBI.Add(lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 1).Count());
                            objGraficaBarrasDTO.lstDataRP.Add(lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 1).Count());
                            objGraficaBarrasDTO.lstDataRME.Add(lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 1).Count());
                            objGraficaBarrasDTO.lstDataRSU.Add(lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 1).Count());

                            generacionTotalActual = lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 1 && w.fechaEntrada.Year == DateTime.Now.Year).Count() + lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 1).Count() +
                                                      lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 1).Count() + lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 1).Count();

                            if ((decimal)generacionTotalActual > 0 && (decimal)HHFechaFiltro > 0)
                            {
                                indiceActual = (200000 * (decimal)generacionTotalActual) / (decimal)HHFechaFiltro;
                                objGraficaBarrasDTO.lstIndice2021.Add(indiceActual);
                                objGraficaBarrasDTO.lstAcumulado2021.Add(indiceActual);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2023.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(0);
                            }
                            #endregion

                            #region SE OBTIENE INDICE 2022
                            cantRBI = lstCapturasRBI2022.Where(w => w.fechaEntrada.Month == 1).Count();
                            cantRP = lstCapturasRP2022.Where(w => w.fechaEntrada.Month == 1).Count();
                            cantRME = lstCapturasRME2022.Where(w => w.fechaEntrada.Month == 1).Count();
                            cantRSU = lstCapturasRSU2022.Where(w => w.fechaEntrada.Month == 1).Count();

                            generacionTotal2022 = cantRBI + cantRP + cantRME + cantRSU;

                            if ((decimal)generacionTotal2022 > 0 && (decimal)HH2022 > 0)
                            {
                                indice2022 = (200000 * (decimal)generacionTotal2022) / (decimal)HH2022;
                                objGraficaBarrasDTO.lstIndice2022.Add(indice2022);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(indice2022);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2022.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(0);
                            }
                            #endregion
                            break;
                            #endregion
                        case 2:
                            #region FEBRERO
                            objGraficaBarrasDTO.lstDataRBI.Add(lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 2).Count());
                            objGraficaBarrasDTO.lstDataRP.Add(lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 2).Count());
                            objGraficaBarrasDTO.lstDataRME.Add(lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 2).Count());
                            objGraficaBarrasDTO.lstDataRSU.Add(lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 2).Count());

                            #region SE OBTIENE INDICE 2023
                            generacionTotalActual = lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 2 && w.fechaEntrada.Year == DateTime.Now.Year).Count() + lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 2).Count() +
                                              lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 2).Count() + lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 2).Count();

                            if ((decimal)generacionTotalActual > 0 && (decimal)HHFechaFiltro > 0)
                            {
                                indiceActual = (200000 * (decimal)generacionTotalActual) / (decimal)HHFechaFiltro;
                                objGraficaBarrasDTO.lstIndice2023.Add(indiceActual);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(objGraficaBarrasDTO.lstIndice2023[0] + indiceActual);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2023.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(indiceActual);
                            }
                            #endregion

                            #region SE OBTIENE INDICE 2022
                            cantRBI = lstCapturasRBI2022.Where(w => w.fechaEntrada.Month == 2).Count();
                            cantRP = lstCapturasRP2022.Where(w => w.fechaEntrada.Month == 2).Count();
                            cantRME = lstCapturasRME2022.Where(w => w.fechaEntrada.Month == 2).Count();
                            cantRSU = lstCapturasRSU2022.Where(w => w.fechaEntrada.Month == 2).Count();

                            generacionTotal2022 = cantRBI + cantRP + cantRME + cantRSU;

                            if ((decimal)generacionTotal2022 > 0 && (decimal)HH2022 > 0)
                            {
                                indice2022 = (200000 * (decimal)generacionTotal2022) / (decimal)HH2022;
                                objGraficaBarrasDTO.lstIndice2022.Add(indice2022);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[0] + indice2022);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2022.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[0]);
                            }
                            #endregion
                            break;
                            #endregion
                        case 3:
                            #region MARZO
                            objGraficaBarrasDTO.lstDataRBI.Add(lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 3).Count());
                            objGraficaBarrasDTO.lstDataRP.Add(lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 3).Count());
                            objGraficaBarrasDTO.lstDataRME.Add(lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 3).Count());
                            objGraficaBarrasDTO.lstDataRSU.Add(lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 3).Count());

                            #region SE OBTIENE INDICE 2023
                            generacionTotalActual = lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 03 && w.fechaEntrada.Year == DateTime.Now.Year).Count() + lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 3).Count() +
                                              lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 3).Count() + lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 3).Count();

                            if ((decimal)generacionTotalActual > 0 && (decimal)HHFechaFiltro > 0)
                            {
                                indiceActual = (200000 * (decimal)generacionTotalActual) / (decimal)HHFechaFiltro;
                                objGraficaBarrasDTO.lstIndice2023.Add(indiceActual);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(objGraficaBarrasDTO.lstIndice2023[1] + indiceActual);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2023.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(indiceActual);
                            }
                            #endregion

                            #region SE OBTIENE INDICE 2022
                            cantRBI = lstCapturasRBI2022.Where(w => w.fechaEntrada.Month == 3).Count();
                            cantRP = lstCapturasRP2022.Where(w => w.fechaEntrada.Month == 3).Count();
                            cantRME = lstCapturasRME2022.Where(w => w.fechaEntrada.Month == 3).Count();
                            cantRSU = lstCapturasRSU2022.Where(w => w.fechaEntrada.Month == 3).Count();

                            generacionTotal2022 = cantRBI + cantRP + cantRME + cantRSU;

                            if ((decimal)generacionTotal2022 > 0 && (decimal)HH2022 > 0)
                            {
                                indice2022 = (200000 * (decimal)generacionTotal2022) / (decimal)HH2022;
                                objGraficaBarrasDTO.lstIndice2022.Add(indice2022);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[1] + indice2022);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2022.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[1]);
                            }
                            #endregion
                            break;
                            #endregion
                        case 4:
                            #region ABRIL
                            objGraficaBarrasDTO.lstDataRBI.Add(lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 4).Count());
                            objGraficaBarrasDTO.lstDataRP.Add(lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 4).Count());
                            objGraficaBarrasDTO.lstDataRME.Add(lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 4).Count());
                            objGraficaBarrasDTO.lstDataRSU.Add(lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 4).Count());

                            #region SE OBTIENE INDICE 2023
                            generacionTotalActual = lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 4 && w.fechaEntrada.Year == DateTime.Now.Year).Count() + lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 4).Count() +
                                              lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 4).Count() + lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 4).Count();

                            if ((decimal)generacionTotalActual > 0 && (decimal)HHFechaFiltro > 0)
                            {
                                indiceActual = (200000 * (decimal)generacionTotalActual) / (decimal)HHFechaFiltro;
                                objGraficaBarrasDTO.lstIndice2023.Add(indiceActual);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(objGraficaBarrasDTO.lstIndice2023[2] + indiceActual);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2023.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(indiceActual);
                            }
                            #endregion

                            #region SE OBTIENE INDICE 2022
                            cantRBI = lstCapturasRBI2022.Where(w => w.fechaEntrada.Month == 4).Count();
                            cantRP = lstCapturasRP2022.Where(w => w.fechaEntrada.Month == 4).Count();
                            cantRME = lstCapturasRME2022.Where(w => w.fechaEntrada.Month == 4).Count();
                            cantRSU = lstCapturasRSU2022.Where(w => w.fechaEntrada.Month == 4).Count();

                            generacionTotal2022 = cantRBI + cantRP + cantRME + cantRSU;

                            if ((decimal)generacionTotal2022 > 0 && (decimal)HH2022 > 0)
                            {
                                indice2022 = (200000 * (decimal)generacionTotal2022) / (decimal)HH2022;
                                objGraficaBarrasDTO.lstIndice2022.Add(indice2022);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[2] + indice2022);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2022.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[2]);
                            }
                            #endregion
                            break;
                            #endregion
                        case 5:
                            #region MAYO
                            objGraficaBarrasDTO.lstDataRBI.Add(lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 5).Count());
                            objGraficaBarrasDTO.lstDataRP.Add(lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 5).Count());
                            objGraficaBarrasDTO.lstDataRME.Add(lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 5).Count());
                            objGraficaBarrasDTO.lstDataRSU.Add(lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 5).Count());

                            #region SE OBTIENE INDICE 2023
                            generacionTotalActual = lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 05 && w.fechaEntrada.Year == DateTime.Now.Year).Count() + lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 5).Count() +
                                              lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 5).Count() + lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 5).Count();

                            if ((decimal)generacionTotalActual > 0 && (decimal)HHFechaFiltro > 0)
                            {
                                indiceActual = (200000 * (decimal)generacionTotalActual) / (decimal)HHFechaFiltro;
                                objGraficaBarrasDTO.lstIndice2023.Add(indiceActual);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(objGraficaBarrasDTO.lstIndice2023[3] + indiceActual);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2023.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(indiceActual);
                            }
                            #endregion

                            #region SE OBTIENE INDICE 2022
                            cantRBI = lstCapturasRBI2022.Where(w => w.fechaEntrada.Month == 5).Count();
                            cantRP = lstCapturasRP2022.Where(w => w.fechaEntrada.Month == 5).Count();
                            cantRME = lstCapturasRME2022.Where(w => w.fechaEntrada.Month == 5).Count();
                            cantRSU = lstCapturasRSU2022.Where(w => w.fechaEntrada.Month == 5).Count();

                            generacionTotal2022 = cantRBI + cantRP + cantRME + cantRSU;

                            if ((decimal)generacionTotal2022 > 0 && (decimal)HH2022 > 0)
                            {
                                indice2022 = (200000 * (decimal)generacionTotal2022) / (decimal)HH2022;
                                objGraficaBarrasDTO.lstIndice2022.Add(indice2022);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[3] + indice2022);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2022.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[3]);
                            }
                            #endregion
                            break;
                            #endregion
                        case 6:
                            #region JUNIO
                            objGraficaBarrasDTO.lstDataRBI.Add(lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 6).Count());
                            objGraficaBarrasDTO.lstDataRP.Add(lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 6).Count());
                            objGraficaBarrasDTO.lstDataRME.Add(lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 6).Count());
                            objGraficaBarrasDTO.lstDataRSU.Add(lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 6).Count());

                            #region SE OBTIENE INDICE 2023
                            generacionTotalActual = lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 6 && w.fechaEntrada.Year == DateTime.Now.Year).Count() + lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 6).Count() +
                                              lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 6).Count() + lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 6).Count();

                            if ((decimal)generacionTotalActual > 0 && (decimal)HHFechaFiltro > 0)
                            {
                                indiceActual = (200000 * (decimal)generacionTotalActual) / (decimal)HHFechaFiltro;
                                objGraficaBarrasDTO.lstIndice2023.Add(indiceActual);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(objGraficaBarrasDTO.lstIndice2023[4] + indiceActual);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2023.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(indiceActual);
                            }
                            #endregion

                            #region SE OBTIENE INDICE 2022
                            cantRBI = lstCapturasRBI2022.Where(w => w.fechaEntrada.Month == 6).Count();
                            cantRP = lstCapturasRP2022.Where(w => w.fechaEntrada.Month == 6).Count();
                            cantRME = lstCapturasRME2022.Where(w => w.fechaEntrada.Month == 6).Count();
                            cantRSU = lstCapturasRSU2022.Where(w => w.fechaEntrada.Month == 6).Count();

                            generacionTotal2022 = cantRBI + cantRP + cantRME + cantRSU;

                            if ((decimal)generacionTotal2022 > 0 && (decimal)HH2022 > 0)
                            {
                                indice2022 = (200000 * (decimal)generacionTotal2022) / (decimal)HH2022;
                                objGraficaBarrasDTO.lstIndice2022.Add(indice2022);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[4] + indice2022);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2022.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[4]);
                            }
                            #endregion
                            break;
                            #endregion
                        case 7:
                            #region JULIO
                            objGraficaBarrasDTO.lstDataRBI.Add(lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 7).Count());
                            objGraficaBarrasDTO.lstDataRP.Add(lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 7).Count());
                            objGraficaBarrasDTO.lstDataRME.Add(lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 7).Count());
                            objGraficaBarrasDTO.lstDataRSU.Add(lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 7).Count());

                            #region SE OBTIENE INDICE 2023
                            generacionTotalActual = lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 7 && w.fechaEntrada.Year == DateTime.Now.Year).Count() + lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 7).Count() +
                                              lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 7).Count() + lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 7).Count();

                            if ((decimal)generacionTotalActual > 0 && (decimal)HHFechaFiltro > 0)
                            {
                                indiceActual = (200000 * (decimal)generacionTotalActual) / (decimal)HHFechaFiltro;
                                objGraficaBarrasDTO.lstIndice2023.Add(indiceActual);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(objGraficaBarrasDTO.lstIndice2023[5] + indiceActual);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2023.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(indiceActual);
                            }
                            #endregion

                            #region SE OBTIENE INDICE 2022
                            cantRBI = lstCapturasRBI2022.Where(w => w.fechaEntrada.Month == 7).Count();
                            cantRP = lstCapturasRP2022.Where(w => w.fechaEntrada.Month == 7).Count();
                            cantRME = lstCapturasRME2022.Where(w => w.fechaEntrada.Month == 7).Count();
                            cantRSU = lstCapturasRSU2022.Where(w => w.fechaEntrada.Month == 7).Count();

                            generacionTotal2022 = cantRBI + cantRP + cantRME + cantRSU;

                            if ((decimal)generacionTotal2022 > 0 && (decimal)HH2022 > 0)
                            {
                                indice2022 = (200000 * (decimal)generacionTotal2022) / (decimal)HH2022;
                                objGraficaBarrasDTO.lstIndice2022.Add(indice2022);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[5] + indice2022);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2022.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[5]);
                            }
                            #endregion
                            break;
                            #endregion
                        case 8:
                            #region AGOSTO
                            objGraficaBarrasDTO.lstDataRBI.Add(lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 8).Count());
                            objGraficaBarrasDTO.lstDataRP.Add(lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 8).Count());
                            objGraficaBarrasDTO.lstDataRME.Add(lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 8).Count());
                            objGraficaBarrasDTO.lstDataRSU.Add(lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 8).Count());

                            #region SE OBTIENE INDICE 2023
                            generacionTotalActual = lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 08 && w.fechaEntrada.Year == DateTime.Now.Year).Count() + lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 8).Count() +
                                              lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 8).Count() + lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 8).Count();

                            if ((decimal)generacionTotalActual > 0 && (decimal)HHFechaFiltro > 0)
                            {
                                indiceActual = (200000 * (decimal)generacionTotalActual) / (decimal)HHFechaFiltro;
                                objGraficaBarrasDTO.lstIndice2023.Add(indiceActual);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(objGraficaBarrasDTO.lstIndice2023[6] + indiceActual);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2023.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(indiceActual);
                            }
                            #endregion

                            #region SE OBTIENE INDICE 2022
                            cantRBI = lstCapturasRBI2022.Where(w => w.fechaEntrada.Month == 8).Count();
                            cantRP = lstCapturasRP2022.Where(w => w.fechaEntrada.Month == 8).Count();
                            cantRME = lstCapturasRME2022.Where(w => w.fechaEntrada.Month == 8).Count();
                            cantRSU = lstCapturasRSU2022.Where(w => w.fechaEntrada.Month == 8).Count();

                            generacionTotal2022 = cantRBI + cantRP + cantRME + cantRSU;

                            if ((decimal)generacionTotal2022 > 0 && (decimal)HH2022 > 0)
                            {
                                indice2022 = (200000 * (decimal)generacionTotal2022) / (decimal)HH2022;
                                objGraficaBarrasDTO.lstIndice2022.Add(indice2022);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[6] + indice2022);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2022.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[6]);
                            }
                            #endregion
                            break;
                            #endregion
                        case 9:
                            #region SEPTIEMBRE
                            objGraficaBarrasDTO.lstDataRBI.Add(lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 9).Count());
                            objGraficaBarrasDTO.lstDataRP.Add(lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 9).Count());
                            objGraficaBarrasDTO.lstDataRME.Add(lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 9).Count());
                            objGraficaBarrasDTO.lstDataRSU.Add(lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 9).Count());

                            #region SE OBTIENE INDICE 2023
                            generacionTotalActual = lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 9 && w.fechaEntrada.Year == DateTime.Now.Year).Count() + lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 9).Count() +
                                              lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 9).Count() + lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 9).Count();

                            if ((decimal)generacionTotalActual > 0 && (decimal)HHFechaFiltro > 0)
                            {
                                indiceActual = (200000 * (decimal)generacionTotalActual) / (decimal)HHFechaFiltro;
                                objGraficaBarrasDTO.lstIndice2023.Add(indiceActual);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(objGraficaBarrasDTO.lstIndice2023[7] + indiceActual);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2023.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(indiceActual);
                            }
                            #endregion

                            #region SE OBTIENE INDICE 2022
                            cantRBI = lstCapturasRBI2022.Where(w => w.fechaEntrada.Month == 9).Count();
                            cantRP = lstCapturasRP2022.Where(w => w.fechaEntrada.Month == 9).Count();
                            cantRME = lstCapturasRME2022.Where(w => w.fechaEntrada.Month == 9).Count();
                            cantRSU = lstCapturasRSU2022.Where(w => w.fechaEntrada.Month == 9).Count();

                            generacionTotal2022 = cantRBI + cantRP + cantRME + cantRSU;

                            if ((decimal)generacionTotal2022 > 0 && (decimal)HH2022 > 0)
                            {
                                indice2022 = (200000 * (decimal)generacionTotal2022) / (decimal)HH2022;
                                objGraficaBarrasDTO.lstIndice2022.Add(indice2022);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[7] + indice2022);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2022.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[7]);
                            }
                            #endregion
                            break;
                            #endregion
                        case 10:
                            #region OCTUBRE
                            objGraficaBarrasDTO.lstDataRBI.Add(lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 10).Count());
                            objGraficaBarrasDTO.lstDataRP.Add(lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 10).Count());
                            objGraficaBarrasDTO.lstDataRME.Add(lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 10).Count());
                            objGraficaBarrasDTO.lstDataRSU.Add(lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 10).Count());

                            #region SE OBTIENE INDICE 2023
                            generacionTotalActual = lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 10 && w.fechaEntrada.Year == DateTime.Now.Year).Count() + lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 10).Count() +
                                              lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 10).Count() + lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 10).Count();

                            if ((decimal)generacionTotalActual > 0 && (decimal)HHFechaFiltro > 0)
                            {
                                indiceActual = (200000 * (decimal)generacionTotalActual) / (decimal)HHFechaFiltro;
                                objGraficaBarrasDTO.lstIndice2023.Add(indiceActual);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(objGraficaBarrasDTO.lstIndice2023[8] + indiceActual);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2023.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(indiceActual);
                            }
                            #endregion

                            #region SE OBTIENE INDICE 2022
                            cantRBI = lstCapturasRBI2022.Where(w => w.fechaEntrada.Month == 10).Count();
                            cantRP = lstCapturasRP2022.Where(w => w.fechaEntrada.Month == 10).Count();
                            cantRME = lstCapturasRME2022.Where(w => w.fechaEntrada.Month == 10).Count();
                            cantRSU = lstCapturasRSU2022.Where(w => w.fechaEntrada.Month == 10).Count();

                            generacionTotal2022 = cantRBI + cantRP + cantRME + cantRSU;

                            if ((decimal)generacionTotal2022 > 0 && (decimal)HH2022 > 0)
                            {
                                indice2022 = (200000 * (decimal)generacionTotal2022) / (decimal)HH2022;
                                objGraficaBarrasDTO.lstIndice2022.Add(indice2022);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[8] + indice2022);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2022.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[8]);
                            }
                            #endregion

                            break;
                            #endregion
                        case 11:
                            #region NOVIEMBRE
                            objGraficaBarrasDTO.lstDataRBI.Add(lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 11).Count());
                            objGraficaBarrasDTO.lstDataRP.Add(lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 11).Count());
                            objGraficaBarrasDTO.lstDataRME.Add(lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 11).Count());
                            objGraficaBarrasDTO.lstDataRSU.Add(lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 11).Count());

                            #region SE OBTIENE INDICE 2023
                            generacionTotalActual = lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 11 && w.fechaEntrada.Year == DateTime.Now.Year).Count() + lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 11).Count() +
                                              lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 11).Count() + lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 11).Count();

                            if ((decimal)generacionTotalActual > 0 && (decimal)HHFechaFiltro > 0)
                            {
                                indiceActual = (200000 * (decimal)generacionTotalActual) / (decimal)HHFechaFiltro;
                                objGraficaBarrasDTO.lstIndice2023.Add(indiceActual);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(objGraficaBarrasDTO.lstIndice2023[9] + indiceActual);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2023.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(indiceActual);
                            }
                            #endregion

                            #region SE OBTIENE INDICE 2022
                            cantRBI = lstCapturasRBI2022.Where(w => w.fechaEntrada.Month == 11).Count();
                            cantRP = lstCapturasRP2022.Where(w => w.fechaEntrada.Month == 11).Count();
                            cantRME = lstCapturasRME2022.Where(w => w.fechaEntrada.Month == 11).Count();
                            cantRSU = lstCapturasRSU2022.Where(w => w.fechaEntrada.Month == 11).Count();

                            generacionTotal2022 = cantRBI + cantRP + cantRME + cantRSU;

                            if ((decimal)generacionTotal2022 > 0 && (decimal)HH2022 > 0)
                            {
                                indice2022 = (200000 * (decimal)generacionTotal2022) / (decimal)HH2022;
                                objGraficaBarrasDTO.lstIndice2022.Add(indice2022);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[9] + indice2022);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2022.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[9]);
                            }
                            #endregion
                            break;
                            #endregion
                        case 12:
                            #region DICIEMBRE
                            objGraficaBarrasDTO.lstDataRBI.Add(lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 12).Count());
                            objGraficaBarrasDTO.lstDataRP.Add(lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 12).Count());
                            objGraficaBarrasDTO.lstDataRME.Add(lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 12).Count());
                            objGraficaBarrasDTO.lstDataRSU.Add(lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 12).Count());

                            #region SE OBTIENE INDICE 2023
                            generacionTotalActual = lstCapturasRBIFechaFiltro.Where(w => w.fechaEntrada.Month == 12 && w.fechaEntrada.Year == DateTime.Now.Year).Count() + lstCapturasRP2023.Where(w => w.fechaEntrada.Month == 12).Count() +
                                              lstCapturasRME2023.Where(w => w.fechaEntrada.Month == 12).Count() + lstCapturasRSU2023.Where(w => w.fechaEntrada.Month == 12).Count();

                            if ((decimal)generacionTotalActual > 0 && (decimal)HHFechaFiltro > 0)
                            {
                                indiceActual = (200000 * (decimal)generacionTotalActual) / (decimal)HHFechaFiltro;
                                objGraficaBarrasDTO.lstIndice2023.Add(indiceActual);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(objGraficaBarrasDTO.lstIndice2023[10] + indiceActual);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2023.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2023.Add(indiceActual);
                            }
                            #endregion

                            #region SE OBTIENE INDICE 2022
                            cantRBI = lstCapturasRBI2022.Where(w => w.fechaEntrada.Month == 12).Count();
                            cantRP = lstCapturasRP2022.Where(w => w.fechaEntrada.Month == 12).Count();
                            cantRME = lstCapturasRME2022.Where(w => w.fechaEntrada.Month == 12).Count();
                            cantRSU = lstCapturasRSU2022.Where(w => w.fechaEntrada.Month == 12).Count();

                            generacionTotal2022 = cantRBI + cantRP + cantRME + cantRSU;

                            if ((decimal)generacionTotal2022 > 0 && (decimal)HH2022 > 0)
                            {
                                indice2022 = (200000 * (decimal)generacionTotal2022) / (decimal)HH2022;
                                objGraficaBarrasDTO.lstIndice2022.Add(indice2022);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[10] + indice2022);
                            }
                            else
                            {
                                objGraficaBarrasDTO.lstIndice2022.Add(0);
                                objGraficaBarrasDTO.lstAcumulado2022.Add(objGraficaBarrasDTO.lstIndice2022[10]);
                            }
                            #endregion
                            break;
                            #endregion
                    }
                    numMes++;
                }
                #endregion

                resultado.Add("objGraficaBarrasDTO", objGraficaBarrasDTO); //OMAR
                resultado.Add(SUCCESS, true);
                #endregion

                #region GRAFICA CONSUMO DE ENERGÍA ELECTRICA
                Dictionary<string, decimal> objGraficaConsumoEnergia = new Dictionary<string, decimal>();
                objGraficaConsumoEnergia = GetGraficaConsumoEnergia(totalMesesMostrar,fechaInicio,fechaFinal);
                resultado.Add("graficaConsumoEnergiaElectricaData", objGraficaConsumoEnergia["data"]);
                resultado.Add("graficaConsumoEnergiaElectricaMeta", objGraficaConsumoEnergia["meta"]);
                #endregion

                #region GRAFICA GENERACIÓN DE ENERGÍA ELECTRICA
                Dictionary<string, decimal> objGraficaGeneracionEnergia = new Dictionary<string, decimal>();
                objGraficaGeneracionEnergia = GetGraficaGeneracionEnergia(totalMesesMostrar, fechaInicio, fechaFinal);
                resultado.Add("graficaGeneracionEnergiaElectricaData", objGraficaGeneracionEnergia["data"]);
                resultado.Add("graficaGeneracionEnergiaElectricaMeta", objGraficaGeneracionEnergia["meta"]);
                #endregion

                #region GRAFICA CONSUMO DE COMBUSTIBLES
                Dictionary<string, decimal> objGraficaConsumoCombustible = new Dictionary<string, decimal>();
                objGraficaConsumoCombustible = GetGraficaConsumoCombustibles(totalMesesMostrar, fechaInicio, fechaFinal);
                resultado.Add("graficaConsumoCombustibleData", objGraficaConsumoCombustible["data"]);
                resultado.Add("graficaConsumoCombustibleMeta", objGraficaConsumoCombustible["meta"]);
                #endregion

                #region GRAFICA GENERACION GEI
                Dictionary<string, decimal> objGraficaGeneracionGEI = new Dictionary<string, decimal>();
                objGraficaGeneracionGEI = GetGraficaGeneracionGEI(totalMesesMostrar, fechaInicio, fechaFinal);
                resultado.Add("graficaGeneracionGEIData", objGraficaGeneracionGEI["data"]);
                resultado.Add("graficaGeneracionGEIMeta", objGraficaGeneracionGEI["meta"]);
                #endregion

                #region GRAFICA CONSUMO DE AGUA
                Dictionary<string, decimal> objGraficaConsumoAgua = new Dictionary<string, decimal>();
                objGraficaConsumoAgua = GetGraficaConsumoAgua(totalMesesMostrar, fechaInicio, fechaFinal);
                resultado.Add("graficaConsumoAguaData", objGraficaConsumoAgua["data"]);
                resultado.Add("graficaConsumoAguaMeta", objGraficaConsumoAgua["meta"]);
                #endregion

                //string = 
                //TODO: OBTENER LAS HORA HOMBRE DEL 2020 PARA OBTENER SOLAMENTE DATOS DEL 2020 Y SOLAMENTE DEL 2021.
                //TODO: MOSTRAR LAS GRAFICAS DE BARRAS CON DOS BOTONES, OCULTAR Y MOSTRAR LAS CORRESPONDIENTES.
                //TODO: OBTENER SOLO LOS REGISTROS DE CAPTURA DEL 2020 Y DEL 2021 POR SEPARADO.

                #region SE CREA BITACORA
                SaveBitacora(16, (int)AccionEnum.CONSULTA, 0, JsonUtils.convertNetObjectToJson(0));
                #endregion

            }
            catch (Exception e)
            {
                LogError(16, 16, "MedioAmbienteController", "GraficaBarrasIndice", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }



        #region CONSUMO ENERGIA ELECTRICA
        private Dictionary<string, decimal> GetGraficaConsumoEnergia(int totalMesesMostrar,string fechaInicio,string fechaFinal)
        {
            Dictionary<string, decimal> resultadoConsumo = new Dictionary<string, decimal>();
            DateTime fechaInicialAnterior = new DateTime();
            DateTime fechaFinalAnterior = new DateTime();
            fechaInicialAnterior = Convert.ToDateTime(fechaInicio).AddYears(-1);
            fechaFinalAnterior = Convert.ToDateTime(fechaInicio).AddYears(-1);
            #region SE OBTIENE LOS REGISTROS CON LA CLASIFICACION "CONSUMO DE ENERGIA"
            decimal sumaRangoFecha = _context.Select<decimal>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT sum(t2.cantAspectoAmbiental) as cantAspectoAmbiental
	                            FROM tblS_MedioAmbienteCaptura AS t1
	                            INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
	                            INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
		                            WHERE t3.id = @id AND t1.esActivo = @esActivo AND t2.esActivo = @esActivo AND t3.esActivo = @esActivo AND (t1.fechaEntrada BETWEEN @fechaInicio AND @fechaFinal)",
                parametros = new { id = 29, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal } //TODO
            }).FirstOrDefault();

            decimal sumatoria2022 = _context.Select<decimal>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT sum(t2.cantAspectoAmbiental) as cantAspectoAmbiental
	                            FROM tblS_MedioAmbienteCaptura AS t1
	                            INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
	                            INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
		                            WHERE t3.id=29 AND t1.esActivo = 1 AND t2.esActivo = 1 AND t3.esActivo = 1 AND t1.fechaEntrada BETWEEN '2022-01-01T00:00:00' AND '2022-05-31T00:00:00'"
                //parametros = new { fechaInicialAnterior = fechaInicialAnterior, fechaFinalAnterior = fechaFinalAnterior } //TODO
            }).FirstOrDefault();

            #endregion

            #region SE OBTIENE LA META DE CONSUMO DE ENERGIA ELECTRICA
            decimal meta = _context.Select<decimal>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT meta FROM tblS_MedioAmbienteMetasAA WHERE id = @id AND registroActivo = @registroActivo",
                parametros = new { id = 1, registroActivo = true }
            }).FirstOrDefault();
            #endregion

            #region RESULTADO GRAFICA
            decimal nuevaMeta = 0;
            decimal porcentajeEnergiaEle = 0;
            //#if DEBUG
            //            cantConsumo = 20;
            //#endif
            if ( meta > 0)
            {
                if (totalMesesMostrar != 12)
                    nuevaMeta = ((decimal)totalMesesMostrar * meta) / 12;
                if (sumatoria2022 == 0 || sumaRangoFecha == 0)
                {
                    porcentajeEnergiaEle = 0;
                }else{
                    porcentajeEnergiaEle = Math.Round((sumaRangoFecha / (((sumaRangoFecha * 0.95m) / totalMesesMostrar) * totalMesesMostrar) * 100)); 
                }

            }

            resultadoConsumo.Add("data", (decimal)porcentajeEnergiaEle);
            resultadoConsumo.Add("meta", (decimal)nuevaMeta);
            #endregion

            return resultadoConsumo;
        }
        #endregion

        #region GENERACIÓN ENERGIA ELECTRICA
        private Dictionary<string, decimal> GetGraficaGeneracionEnergia(int totalMesesMostrar,string fechaInicio,string fechaFinal)
        {
            Dictionary<string, decimal> resultadoConsumo = new Dictionary<string, decimal>();

            DateTime fechaInicialAnterior = new DateTime();
            DateTime fechaFinalAnterior = new DateTime();
            fechaInicialAnterior = Convert.ToDateTime(fechaInicio).AddYears(-1);
            fechaFinalAnterior = Convert.ToDateTime(fechaInicio).AddYears(-1);


            #region SE OBTIENE LOS REGISTROS CON LA CLASIFICACION "CONSUMO DE ENERGIA"
            decimal sumaRangoFecha = _context.Select<decimal>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT sum(t2.cantAspectoAmbiental) as cantAspectoAmbiental
	                            FROM tblS_MedioAmbienteCaptura AS t1
	                            INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
	                            INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
		                            WHERE t3.id = @id AND t1.esActivo = @esActivo AND t2.esActivo = @esActivo AND t3.esActivo = @esActivo AND (t1.fechaEntrada BETWEEN @fechaInicio AND @fechaFinal)",
                parametros = new { id = 30, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal } //TODO
            }).FirstOrDefault();

            decimal sumatoria2022 = _context.Select<decimal>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT sum(t2.cantAspectoAmbiental) as cantAspectoAmbiental
	                            FROM tblS_MedioAmbienteCaptura AS t1
	                            INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
	                            INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
		                            WHERE t3.id=30 AND t1.esActivo = 1 AND t2.esActivo = 1 AND t3.esActivo = 1 AND t1.fechaEntrada BETWEEN '2022-01-01T00:00:00' AND '2022-05-31T00:00:00'"
                //parametros = new { fechaInicialAnterior = fechaInicialAnterior, fechaFinalAnterior = fechaFinalAnterior } //TODO
            }).FirstOrDefault();
            #endregion

            #region SE OBTIENE LA META DE CONSUMO DE ENERGIA ELECTRICA
            decimal meta = _context.Select<decimal>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT meta FROM tblS_MedioAmbienteMetasAA WHERE id = @id AND registroActivo = @registroActivo",
                parametros = new { id = 1, registroActivo = true }
            }).FirstOrDefault();
            #endregion

            #region RESULTADO GRAFICA
            decimal nuevaMeta = 0;
            decimal porcentajeGeneracionEnergiaEle = 0;
            //#if DEBUG
            //            cantConsumo = 25;
            //#endif
            if ( meta > 0)
            {
                if (totalMesesMostrar != 12)
                    nuevaMeta = ((decimal)totalMesesMostrar * meta) / 12;
                if (sumatoria2022 == 0 || sumaRangoFecha == 0)
                {
                    porcentajeGeneracionEnergiaEle = 0;
                }
                else
                {
                    porcentajeGeneracionEnergiaEle = Math.Round((sumaRangoFecha / (((sumaRangoFecha * 0.95m) / totalMesesMostrar) * totalMesesMostrar) * 100)); 
                }
               
            }

            resultadoConsumo.Add("data", (decimal)porcentajeGeneracionEnergiaEle);
            resultadoConsumo.Add("meta", (decimal)nuevaMeta);
            #endregion

            return resultadoConsumo;
        }
        #endregion

        #region CONSUMO DE COMBUSTIBLES
        private Dictionary<string, decimal> GetGraficaConsumoCombustibles(int totalMesesMostrar,string fechaInicio,string fechaFinal)
        {
            Dictionary<string, decimal> resultadoConsumo = new Dictionary<string, decimal>();
            DateTime fechaInicialAnterior = new DateTime();
            DateTime fechaFinalAnterior = new DateTime();
            fechaInicialAnterior = Convert.ToDateTime(fechaInicio).AddYears(-1);
            fechaFinalAnterior = Convert.ToDateTime(fechaInicio).AddYears(-1);
            #region SE OBTIENE LOS REGISTROS CON LA CLASIFICACION "CONSUMO DE ENERGIA"

            decimal sumaRangoFecha = _context.Select<decimal>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT sum(t2.cantAspectoAmbiental) as cantAspectoAmbiental
	                            FROM tblS_MedioAmbienteCaptura AS t1
	                            INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
	                            INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
		                            WHERE t3.id = @id AND t1.esActivo = @esActivo AND t2.esActivo = @esActivo AND t3.esActivo = @esActivo AND (t1.fechaEntrada BETWEEN @fechaInicio AND @fechaFinal)",
                parametros = new { id = 31, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal } //TODO
            }).FirstOrDefault();

            decimal sumatoria2022 = _context.Select<decimal>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT sum(t2.cantAspectoAmbiental) as cantAspectoAmbiental
	                            FROM tblS_MedioAmbienteCaptura AS t1
	                            INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
	                            INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
		                            WHERE t3.id=31 AND t1.esActivo = 1 AND t2.esActivo = 1 AND t3.esActivo = 1 AND t1.fechaEntrada BETWEEN '2022-01-01T00:00:00' AND '2022-05-31T00:00:00'"
                //parametros = new { fechaInicialAnterior = fechaInicialAnterior, fechaFinalAnterior = fechaFinalAnterior } //TODO
            }).FirstOrDefault();

            #endregion

            #region SE OBTIENE LA META DE CONSUMO DE ENERGIA ELECTRICA
            decimal meta = _context.Select<decimal>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT meta FROM tblS_MedioAmbienteMetasAA WHERE id = @id AND registroActivo = @registroActivo",
                parametros = new { id = 1, registroActivo = true }
            }).FirstOrDefault();
            #endregion

            #region RESULTADO GRAFICA
            decimal nuevaMeta = 0;
            decimal porcentajeConsumoCombustible = 0;
            //#if DEBUG
            //            cantConsumo = 30;
            //#endif
            if ( meta > 0)
            {
                if (totalMesesMostrar != 12)
                    nuevaMeta = ((decimal)totalMesesMostrar * meta) / 12;
                if (sumatoria2022 == 0 || sumaRangoFecha == 0)
                {
                    porcentajeConsumoCombustible = 0;
                }
                else
                {
                    porcentajeConsumoCombustible = Math.Round((sumaRangoFecha / (((sumaRangoFecha * 0.95m) / totalMesesMostrar) * totalMesesMostrar) * 100)); 
                }
                
            }

            resultadoConsumo.Add("data", (decimal)porcentajeConsumoCombustible);
            resultadoConsumo.Add("meta", (decimal)nuevaMeta);
            #endregion

            return resultadoConsumo;
        }
        #endregion

        #region GENERACION GEI
        private Dictionary<string, decimal> GetGraficaGeneracionGEI(int totalMesesMostrar, string fechaInicio, string fechaFinal)
        {
            Dictionary<string, decimal> resultadoConsumo = new Dictionary<string, decimal>();
            DateTime fechaInicialAnterior = new DateTime();
            DateTime fechaFinalAnterior = new DateTime();
            fechaInicialAnterior = Convert.ToDateTime(fechaInicio).AddYears(-1);
            fechaFinalAnterior = Convert.ToDateTime(fechaInicio).AddYears(-1);
            #region SE OBTIENE LOS REGISTROS CON LA CLASIFICACION "CONSUMO DE ENERGIA"

            decimal sumaRangoFecha = _context.Select<decimal>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT sum(t2.cantAspectoAmbiental) as cantAspectoAmbiental
	                            FROM tblS_MedioAmbienteCaptura AS t1
	                            INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
	                            INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
		                            WHERE t3.id = @id AND t1.esActivo = @esActivo AND t2.esActivo = @esActivo AND t3.esActivo = @esActivo AND (t1.fechaEntrada BETWEEN @fechaInicio AND @fechaFinal)",
                parametros = new { id = 32, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal } //TODO
            }).FirstOrDefault();

            decimal sumatoria2022 = _context.Select<decimal>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT sum(t2.cantAspectoAmbiental) as cantAspectoAmbiental
	                            FROM tblS_MedioAmbienteCaptura AS t1
	                            INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
	                            INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
		                            WHERE t3.id=32 AND t1.esActivo = 1 AND t2.esActivo = 1 AND t3.esActivo = 1 AND t1.fechaEntrada BETWEEN '2022-01-01T00:00:00' AND '2022-05-31T00:00:00'"
                //parametros = new { fechaInicialAnterior = fechaInicialAnterior, fechaFinalAnterior = fechaFinalAnterior } //TODO
            }).FirstOrDefault();

            #endregion

            #region SE OBTIENE LA META DE CONSUMO DE ENERGIA ELECTRICA
            decimal meta = _context.Select<decimal>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT meta FROM tblS_MedioAmbienteMetasAA WHERE id = @id AND registroActivo = @registroActivo",
                parametros = new { id = 1, registroActivo = true }
            }).FirstOrDefault();
            #endregion

            #region RESULTADO GRAFICA
            decimal nuevaMeta = 0;
            decimal porcentajeGEI = 0;
            //#if DEBUG
            //            cantConsumo = 35;
            //#endif
            if ( meta > 0)
            {
                if (totalMesesMostrar != 12)
                    nuevaMeta = ((decimal)totalMesesMostrar * meta) / 12;
                if (sumatoria2022 == 0 || sumaRangoFecha==0)
                {
                    porcentajeGEI = 0;
                }
                else
                {
                    porcentajeGEI = Math.Round((sumaRangoFecha / (((sumaRangoFecha * 0.95m) / totalMesesMostrar) * totalMesesMostrar) * 100)); 
                }
                
            }

            resultadoConsumo.Add("data", (decimal)porcentajeGEI);
            resultadoConsumo.Add("meta", (decimal)nuevaMeta);
            #endregion

            return resultadoConsumo;
        }
        #endregion

        #region GENERACION CONSUMO AGUA
        private Dictionary<string, decimal> GetGraficaConsumoAgua(int totalMesesMostrar,string fechaInicio,string fechaFinal)
        {
            Dictionary<string, decimal> resultadoConsumo = new Dictionary<string, decimal>();
            DateTime fechaInicialAnterior = new DateTime();
            DateTime fechaFinalAnterior = new DateTime();
            fechaInicialAnterior = Convert.ToDateTime(fechaInicio).AddYears(-1);
            fechaFinalAnterior = Convert.ToDateTime(fechaInicio).AddYears(-1);
            #region SE OBTIENE LOS REGISTROS CON LA CLASIFICACION "CONSUMO DE ENERGIA"


            decimal sumaRangoFecha = _context.Select<decimal>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT sum(t2.cantAspectoAmbiental) as cantAspectoAmbiental
	                            FROM tblS_MedioAmbienteCaptura AS t1
	                            INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
	                            INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
		                            WHERE t3.id = @id AND t1.esActivo = @esActivo AND t2.esActivo = @esActivo AND t3.esActivo = @esActivo AND (t1.fechaEntrada BETWEEN @fechaInicio AND @fechaFinal)",
                parametros = new { id = 33, esActivo = true, fechaInicio = fechaInicio, fechaFinal = fechaFinal } //TODO
            }).FirstOrDefault();

            decimal sumatoria2022 = _context.Select<decimal>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = string.Format(@"SELECT sum(t2.cantAspectoAmbiental) as cantAspectoAmbiental
	                            FROM tblS_MedioAmbienteCaptura AS t1
	                            INNER JOIN tblS_MedioAmbienteCapturaDet AS t2 ON t1.id = t2.idCaptura
	                            INNER JOIN tblS_MedioAmbienteAspectoAmbiental AS t3 ON t2.idAspectoAmbiental = t3.id
		                            WHERE t3.id=33 AND t1.esActivo = 1 AND t2.esActivo = 1 AND t3.esActivo = 1 AND t1.fechaEntrada BETWEEN '2022-01-01T00:00:00' AND '2022-05-31T00:00:00'")
                //parametros = new { esActivo = true } //TODO
            }).FirstOrDefault();
            #endregion

            #region SE OBTIENE LA META DE CONSUMO DE ENERGIA ELECTRICA
            decimal meta = _context.Select<decimal>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT meta FROM tblS_MedioAmbienteMetasAA WHERE id = @id AND registroActivo = @registroActivo",
                parametros = new { id = 1, registroActivo = true }
            }).FirstOrDefault();
            #endregion

            #region RESULTADO GRAFICA
            decimal nuevaMeta = 0;
            decimal porcentajeConsumoAgua = 0;
            //#if DEBUG
            //            cantConsumo = 40;
            //#endif
            if ( meta > 0)
            {
                if (totalMesesMostrar != 12)
                    nuevaMeta = ((decimal)totalMesesMostrar * meta) / 12;
                if (sumatoria2022 == 0 || sumaRangoFecha == 0)
                {
                    porcentajeConsumoAgua = 0;
                }
                else
                {
                    porcentajeConsumoAgua = Math.Round((sumaRangoFecha / (((sumaRangoFecha * 0.95m) / totalMesesMostrar) * totalMesesMostrar) * 100)); 
                }
                
            }

            resultadoConsumo.Add("data", (decimal)porcentajeConsumoAgua);
            resultadoConsumo.Add("meta", (decimal)nuevaMeta);
            #endregion

            return resultadoConsumo;
        }
        #endregion

                

        #region FILL COMBOS
        public Dictionary<string, object> FillCboAgrupaciones()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL COMBO CLASIFICACIONES TRANSPORTISTAS
                List<ComboDTO> lstClasificaciones = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT id AS Value, nomAgrupacion AS Text FROM tblS_IncidentesAgrupacionCC WHERE esActivo = @esActivo ORDER BY nomAgrupacion",
                    parametros = new { esActivo = true }
                });
                resultado.Add(ITEMS, lstClasificaciones);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(16, 16, "MedioAmbienteController", "FillCboClasificacionesTransportistas", e, AccionEnum.CONSULTA, 0, 0);
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
                #region SE OBTIENE LISTADO DE USUARIOS
                List<ComboDTO> lstUsuariosCbo = new List<ComboDTO>();

                //var strQuery = new OdbcConsultaDTO();
                //strQuery.consulta = @"SELECT clave_empleado AS Value, (nombre + ' ' + ape_paterno + ' ' + ape_materno) AS Text FROM sn_empleados WHERE estatus_empleado = 'A'";
                //List<ComboDTO> lstUsuariosCP = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.CplanRh, strQuery).ToList();

                //var strQuery2 = new OdbcConsultaDTO();
                //strQuery2.consulta = @"SELECT clave_empleado as Value, (nombre +' ' + ape_paterno + ' ' + ape_materno) AS Text FROM sn_empleados";
                //List<ComboDTO> lstUsuariosArr = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.ArrenRh, strQuery2).ToList();

                var lstUsuariosCP = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT clave_empleado AS Value, (nombre + ' ' + ape_paterno + ' ' + ape_materno) AS Text FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A'",
                });

                var lstUsuariosArr = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT clave_empleado AS Value, (nombre + ' ' + ape_paterno + ' ' + ape_materno) AS Text FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A'",
                });

                foreach (var item in lstUsuariosCP)
                {
                    lstUsuariosCbo.Add(item);
                }

                foreach (var item in lstUsuariosArr)
                {
                    var existe = lstUsuariosCbo.Where(w => w.Value == item.Value).FirstOrDefault();
                    if (existe == null)
                        lstUsuariosCbo.Add(item);
                }

                resultado.Add(ITEMS, lstUsuariosCbo);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception ex)
            {
                LogError(0, 0, "MedioAmbienteController", "FillCboUsuarios", ex, AccionEnum.CONSULTA, 0, 0);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboAspectosAmbientales()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL COMBO ASPECTOS AMBIENTALES
                List<ComboDTO> lstClasificaciones = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT id AS Value, descripcion AS text FROM tblS_MedioAmbienteAspectoAmbiental WHERE esActivo = @esActivo ORDER BY descripcion",
                    parametros = new { esActivo = true }
                });
                resultado.Add(ITEMS, lstClasificaciones);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(16, 16, "MedioAmbienteController", "FillCboAspectosAmbientales", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboTransportistas()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL COMBO TRANSPORTISTAS
                List<ComboDTO> lstTransportistas = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT id AS Value, razonSocial AS text FROM tblS_MedioAmbienteTransportistas WHERE esActivo = @esActivo ORDER BY razonSocial",
                    parametros = new { esActivo = true }
                });
                resultado.Add(ITEMS, lstTransportistas);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(16, 16, "MedioAmbienteController", "FillCboTransportistas", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        #endregion

        #region GENERALES
        public Tuple<Stream, string> DescargarArchivo(int _idCaptura, int _tipoArchivo)
        {
            try
            {
                tblS_MedioAmbienteArchivos objArchivo = _context.Select<tblS_MedioAmbienteArchivos>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT id, nombreArchivo, rutaArchivo FROM tblS_MedioAmbienteArchivos WHERE idCaptura = @idCaptura AND tipoArchivo = @tipoArchivo AND registroActivo = @registroActivo",
                    parametros = new { idCaptura = _idCaptura, tipoArchivo = _tipoArchivo, registroActivo = true }
                }).FirstOrDefault();

#if DEBUG
                string rutaArchivoServidor = objArchivo.rutaArchivo;
                string rutaArchivoLocal = string.Empty;
                string[] splitArchivo = rutaArchivoServidor.Split('\\');
                foreach (var item in splitArchivo)
                {
                    rutaArchivoLocal = item;
                }
                string ruta = "C:\\Proyecto\\SIGOPLAN\\SEGURIDAD_MEDIO_AMBIENTE\\" + _idCaptura + "\\" + rutaArchivoLocal;
                var fileStream = GlobalUtils.GetFileAsStream(ruta);
                string name = Path.GetFileName(objArchivo.nombreArchivo);
#else
                var fileStream = GlobalUtils.GetFileAsStream(objArchivo.rutaArchivo);
                string name = Path.GetFileName(objArchivo.nombreArchivo);
#endif

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                LogError(0, 0, "ReporteFallaController", "DescargarArchivo", e, AccionEnum.DESCARGAR, _idCaptura, 0);
                return null;
            }
        }
        #endregion

        #region PRIVATE GENERAL
        private bool RegistrarArchivo(CapturaDTO _objCEDTO, HttpPostedFileBase _objFile, int _tipoArchivo)
        {
            try
            {
                #region SE REGISTRA ARCHIVO
                List<Tuple<HttpPostedFileBase, string>> lstArchivos = new List<Tuple<HttpPostedFileBase, string>>();
                var CarpetaNueva = Path.Combine(RutaBase, _objCEDTO.id.ToString());
                ExisteCarpeta(CarpetaNueva, true);

                string nombreArchivo = GetFormatoNombreArchivo("Evidencia-", _objFile.FileName);
                string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);
                lstArchivos.Add(Tuple.Create(_objFile, rutaArchivo));

                tblS_MedioAmbienteArchivos objArchivo = new tblS_MedioAmbienteArchivos();
                objArchivo.idCaptura = _objCEDTO.id;
                objArchivo.tipoArchivo = _tipoArchivo;
                objArchivo.nombreArchivo = nombreArchivo;
                objArchivo.rutaArchivo = rutaArchivo;
                objArchivo.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                objArchivo.fechaCreacion = DateTime.Now;
                objArchivo.registroActivo = true;
                _context.tblS_MedioAmbienteArchivos.Add(objArchivo);
                _context.SaveChanges();
                #endregion

                #region SE CREA BITACORA
                SaveBitacora(16, (int)AccionEnum.AGREGAR, _objCEDTO.id, JsonUtils.convertNetObjectToJson(objArchivo));
                #endregion

                if (GlobalUtils.SaveHTTPPostedFile(_objFile, rutaArchivo) == false)
                    throw new Exception("Ocurrió un error al registrar la evidencia en el servidor.");

                return true;
            }
            catch (Exception e)
            {
                LogError(16, 16, "MedioAmbienteController", "RegistrarArchivo", e, AccionEnum.AGREGAR, _objCEDTO.id, 0);
                return false;
            }
        }

        private static bool ExisteCarpeta(string path, bool crear = false)
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
            catch (Exception e)
            {
                existe = false;
            }
            return existe;
        }

        private string GetFormatoNombreArchivo(string nombreBase, string fileName)
        {
            return String.Format("{0} {1}{2}", nombreBase, fileName.Split('.')[0], Path.GetExtension(fileName));
        }
        #endregion
    }
}