using Core.DTO.Administracion.Seguridad.SaludOcupacional;
using Core.Entity.Administrativo.Seguridad.SaludOcupacional;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Administracion.Seguridad.SaludOcupacional
{
    public interface ISaludOcupacionalDAO
    {
        #region ATENCIÓN MÉDICA
        Dictionary<string, object> CargarInformacionEmpleado(int claveEmpleado);
        Dictionary<string, object> GuardarNuevaAtencionMedica(tblS_SO_AtencionMedica atencionMedica, tblS_SO_AtencionMedica_Revision revision, HttpPostedFileBase archivoST7, HttpPostedFileBase archivoST2);
        Dictionary<string, object> GuardarNuevaRevision(tblS_SO_AtencionMedica_Revision revision, HttpPostedFileBase archivoST7, HttpPostedFileBase archivoST2);
        Dictionary<string, object> GuardarArchivosST7ST2(int atencionMedica_id, HttpPostedFileBase archivoST7, HttpPostedFileBase archivoST2);
        Dictionary<string, object> CargarAtencionesMedicas(int claveEmpleado);
        Dictionary<string, object> CargarAtencionMedicaDetalle(int atencionMedica_id);
        Tuple<Stream, string> DescargarArchivoST7(int atencionMedica_id);
        Tuple<Stream, string> DescargarArchivoST2(int atencionMedica_id);
        AtencionMedicaReporteDTO GetAtencionMedicaReporte(int atencionMedica_id);
        Dictionary<string, object> CargarHistorialEmpleado(int claveEmpleado);
        Dictionary<string, object> EliminarAtencionMedica(int idAtencion);
        
        #endregion

        #region MEDICOS
        Dictionary<string, object> GetMedicos(medicoDTO _objFiltroDto);

        Dictionary<string, object> CrearEditarMedicos(medicoDTO _objMedicoDTO);

        Dictionary<string, object> EliminarMedico(int _idMedico);
        #endregion

        #region HISTORIAL CLINICO
        Dictionary<string, object> CEHistorialClinico(HistorialClinicoDTO objHistorialClinicoDTO, List<HttpPostedFileBase> lstArchivosDatosPersonales, List<HttpPostedFileBase> lstArchivosEspirometria, List<HttpPostedFileBase> lstArchivosAudiometria,
                                                      List<HttpPostedFileBase> lstArchivosElectrocardiograma, List<HttpPostedFileBase> lstArchivosRadiografias, List<HttpPostedFileBase> lstArchivosLaboratorio, 
                                                      List<HttpPostedFileBase> lstArchivosDocumentosAdjuntos);

        Dictionary<string, object> GetUltimoFolioHistorialClinico();

        Dictionary<string, object> GetDatosActualizarHistorialClinico(int idHistorialClinico);

        Dictionary<string, object> GetHistorialesClinicos();

        Dictionary<string, object> EliminarHistorialClinico(int idHistorialClinico);

        #region OBSERVACIÓN MEDICO CP
        Dictionary<string, object> GetObservacionMedicoInternoCP(int idHC);

        Dictionary<string, object> EditarObservacionMedicoInternoCP(List<HttpPostedFileBase> lstArchivos, HistorialClinicoDTO objDTO);
        #endregion 

        #region CARGAR DOCUMENTO FIRMADO MEDICO EXTERNO
        Dictionary<string, object> CargarDocumentoFirmadoMedicoExterno(List<HttpPostedFileBase> lstArchivos, HistorialClinicoDTO objDTO);

        Dictionary<string, object> GetRutaArchivo(int idHC, int tipoArchivo);

        Tuple<Stream, string> DescargarArchivo(int idHC, int tipoArchiv);

        Dictionary<string, object> VerificarExisteDocumento(int idHC, int tipoArchivo);
        #endregion
        #endregion

        #region FUNCIONES GENERALES
        Dictionary<string, object> FillCboEscolaridades();

        Dictionary<string, object> FillCboEmpresas();

        Dictionary<string, object> FillCboCC();

        Dictionary<string, object> FillCboEstadoCivil();

        Dictionary<string, object> FillCboTipoSanguineo();

        Dictionary<string, object> GetEdadPaciente(DateTime fechaNac);

        Dictionary<string, object> FillCboMarcasCovid19();

        Dictionary<string, object> FillCboUsuariosMedicos();
        #endregion

        #region REPORTES
        Dictionary<string, object> GetReportDataHistorialClinico(int idHistorialClinico);
        Dictionary<string, object> GetReportDataCertificado();
        #endregion
    }
}
