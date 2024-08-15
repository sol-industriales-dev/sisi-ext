using Core.DAO.Administracion.Seguridad.SaludOcupacional;
using Core.Entity.Administrativo.Seguridad.SaludOcupacional;
using Core.DTO.Administracion.Seguridad.SaludOcupacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;

namespace Core.Service.Administracion.Seguridad.SaludOcupacional
{
    public class SaludOcupacionalService : ISaludOcupacionalDAO
    {

        public ISaludOcupacionalDAO saludOcupacionalDAO;
        public ISaludOcupacionalDAO SaludOcupacionalDAO
        {
            get { return saludOcupacionalDAO; }
            set { saludOcupacionalDAO = value; }
        }
        public SaludOcupacionalService(ISaludOcupacionalDAO saludOcupacionalDAO)
        {
            this.SaludOcupacionalDAO = saludOcupacionalDAO;
        }

        #region Atención Médica
        public Dictionary<string, object> CargarInformacionEmpleado(int claveEmpleado)
        {
            return saludOcupacionalDAO.CargarInformacionEmpleado(claveEmpleado);
        }

        public Dictionary<string, object> GuardarNuevaAtencionMedica(tblS_SO_AtencionMedica atencionMedica, tblS_SO_AtencionMedica_Revision revision, HttpPostedFileBase archivoST7, HttpPostedFileBase archivoST2)
        {
            return saludOcupacionalDAO.GuardarNuevaAtencionMedica(atencionMedica, revision, archivoST7, archivoST2);
        }

        public Dictionary<string, object> GuardarNuevaRevision(tblS_SO_AtencionMedica_Revision revision, HttpPostedFileBase archivoST7, HttpPostedFileBase archivoST2)
        {
            return saludOcupacionalDAO.GuardarNuevaRevision(revision, archivoST7, archivoST2);
        }

        public Dictionary<string, object> GuardarArchivosST7ST2(int atencionMedica_id, HttpPostedFileBase archivoST7, HttpPostedFileBase archivoST2)
        {
            return saludOcupacionalDAO.GuardarArchivosST7ST2(atencionMedica_id, archivoST7, archivoST2);
        }

        public Dictionary<string, object> CargarAtencionesMedicas(int claveEmpleado)
        {
            return saludOcupacionalDAO.CargarAtencionesMedicas(claveEmpleado);
        }
        public Dictionary<string, object> EliminarAtencionMedica(int idAtencion)
        {
            return saludOcupacionalDAO.EliminarAtencionMedica(idAtencion);
        }
        

        public Dictionary<string, object> CargarAtencionMedicaDetalle(int atencionMedica_id)
        {
            return saludOcupacionalDAO.CargarAtencionMedicaDetalle(atencionMedica_id);
        }

        public Tuple<Stream, string> DescargarArchivoST7(int atencionMedica_id)
        {
            return saludOcupacionalDAO.DescargarArchivoST7(atencionMedica_id);
        }

        public Tuple<Stream, string> DescargarArchivoST2(int atencionMedica_id)
        {
            return saludOcupacionalDAO.DescargarArchivoST2(atencionMedica_id);
        }

        public AtencionMedicaReporteDTO GetAtencionMedicaReporte(int atencionMedica_id)
        {
            return saludOcupacionalDAO.GetAtencionMedicaReporte(atencionMedica_id);
        }

        public Dictionary<string, object> CargarHistorialEmpleado(int claveEmpleado)
        {
            return saludOcupacionalDAO.CargarHistorialEmpleado(claveEmpleado);
        }
        #endregion

        #region Medicos

        public Dictionary<string, object> GetMedicos(medicoDTO _objFilterDTO)
        {
            return saludOcupacionalDAO.GetMedicos(_objFilterDTO);
        }

        public Dictionary<string, object> CrearEditarMedicos(medicoDTO _objFilterDTO)
        {
            return saludOcupacionalDAO.CrearEditarMedicos(_objFilterDTO);
        }

        public Dictionary<string, object> EliminarMedico(int _idMedico)
        {
            return saludOcupacionalDAO.EliminarMedico(_idMedico);
        }

        #endregion

        #region HISTORIAL CLINICO
        public Dictionary<string, object> CEHistorialClinico(HistorialClinicoDTO objHistorialClinicoDTO, List<HttpPostedFileBase> lstArchivosDatosPersonales, List<HttpPostedFileBase> lstArchivosEspirometria, List<HttpPostedFileBase> lstArchivosAudiometria, List<HttpPostedFileBase> lstArchivosElectrocardiograma,
                                               List<HttpPostedFileBase> lstArchivosRadiografias, List<HttpPostedFileBase> lstArchivosLaboratorio, List<HttpPostedFileBase> lstArchivosDocumentosAdjuntos)
        {
            return saludOcupacionalDAO.CEHistorialClinico(objHistorialClinicoDTO, lstArchivosDatosPersonales, lstArchivosEspirometria, lstArchivosAudiometria, lstArchivosElectrocardiograma, lstArchivosRadiografias, lstArchivosLaboratorio, 
                                                          lstArchivosDocumentosAdjuntos);
        }

        public Dictionary<string, object> GetUltimoFolioHistorialClinico()
        {
            return saludOcupacionalDAO.GetUltimoFolioHistorialClinico();
        }

        public Dictionary<string, object> GetDatosActualizarHistorialClinico(int idHistorialClinico)
        {
            return saludOcupacionalDAO.GetDatosActualizarHistorialClinico(idHistorialClinico);
        }

        public Dictionary<string, object> GetHistorialesClinicos()
        {
            return saludOcupacionalDAO.GetHistorialesClinicos();
        }

        public Dictionary<string, object> EliminarHistorialClinico(int idHistorialClinico)
        {
            return saludOcupacionalDAO.EliminarHistorialClinico(idHistorialClinico);
        }

        #region OBSERVACIÓN MEDICO CP
        public Dictionary<string, object> GetObservacionMedicoInternoCP(int idHC)
        {
            return saludOcupacionalDAO.GetObservacionMedicoInternoCP(idHC);
        }

        public Dictionary<string, object> EditarObservacionMedicoInternoCP(List<HttpPostedFileBase> lstArchivos, HistorialClinicoDTO objDTO)
        {
            return saludOcupacionalDAO.EditarObservacionMedicoInternoCP(lstArchivos, objDTO);
        }
        #endregion 

        #region CARGAR DOCUMENTO FIRMADO MEDICO EXTERNO
        public Dictionary<string, object> CargarDocumentoFirmadoMedicoExterno(List<HttpPostedFileBase> lstArchivos, HistorialClinicoDTO objDTO)
        {
            return saludOcupacionalDAO.CargarDocumentoFirmadoMedicoExterno(lstArchivos, objDTO);
        }

        public Dictionary<string, object> GetRutaArchivo(int idHC, int tipoArchivo)
        {
            return saludOcupacionalDAO.GetRutaArchivo(idHC, tipoArchivo);
        }

        public Tuple<Stream, string> DescargarArchivo(int idHC, int tipoArchivo)
        {
            return saludOcupacionalDAO.DescargarArchivo(idHC, tipoArchivo);
        }

        public Dictionary<string, object> VerificarExisteDocumento(int idHC, int tipoArchivo)
        {
            return saludOcupacionalDAO.VerificarExisteDocumento(idHC, tipoArchivo);
        }
        #endregion
        #endregion

        #region FUNCIONES GENERALES
        public Dictionary<string, object> FillCboEscolaridades()
        {
            return saludOcupacionalDAO.FillCboEscolaridades();
        }

        public Dictionary<string, object> FillCboEmpresas()
        {
            return saludOcupacionalDAO.FillCboEmpresas();
        }

        public Dictionary<string, object> FillCboCC()
        {
            return saludOcupacionalDAO.FillCboCC();
        }

        public Dictionary<string, object> FillCboEstadoCivil()
        {
            return saludOcupacionalDAO.FillCboEstadoCivil();
        }

        public Dictionary<string, object> FillCboTipoSanguineo()
        {
            return saludOcupacionalDAO.FillCboTipoSanguineo();
        }

        public Dictionary<string, object> GetEdadPaciente(DateTime fechaNac)
        {
            return saludOcupacionalDAO.GetEdadPaciente(fechaNac);
        }

        public Dictionary<string, object> FillCboMarcasCovid19()
        {
            return saludOcupacionalDAO.FillCboMarcasCovid19();
        }

        public Dictionary<string, object> FillCboUsuariosMedicos()
        {
            return saludOcupacionalDAO.FillCboUsuariosMedicos();
        }
        #endregion

        #region CrystalReports

        public Dictionary<string, object> GetReportDataHistorialClinico(int idHistorialClinico)
        {
            return saludOcupacionalDAO.GetReportDataHistorialClinico(idHistorialClinico);
        }
        public Dictionary<string, object> GetReportDataCertificado()
        {
            return saludOcupacionalDAO.GetReportDataCertificado();
        }

        #endregion

    }
}
