using Core.DAO.Administracion.Seguridad.MedioAmbiente;
using Core.DTO.Administracion.Seguridad.MedioAmbiente;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using Core.Entity.Administrativo.Seguridad.MedioAmbiente;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.Administracion.Seguridad.MedioAmbiente
{
    public class MedioAmbienteService : IMedioAmbienteDAO
    {
        #region INIT
        public IMedioAmbienteDAO medioAmbienteDAO { get; set; }

        public MedioAmbienteService(IMedioAmbienteDAO medioAmbienteDAO)
        {
            this.medioAmbienteDAO = medioAmbienteDAO;
        }
        #endregion

        #region ASPECTOS AMBIENTALES
        public Dictionary<string, object> getAspectosAmbientales(int tipoCaptura = 0)
        {
            return medioAmbienteDAO.getAspectosAmbientales(tipoCaptura);
        }

        public Dictionary<string, object> guardarNuevoAspectoAmbiental(tblS_MedioAmbienteAspectoAmbiental aspectoAmbiental)
        {
            return medioAmbienteDAO.guardarNuevoAspectoAmbiental(aspectoAmbiental);
        }

        public Dictionary<string, object> editarAspectoAmbiental(tblS_MedioAmbienteAspectoAmbiental aspectoAmbiental)
        {
            return medioAmbienteDAO.editarAspectoAmbiental(aspectoAmbiental);
        }

        public Dictionary<string, object> eliminarAspectoAmbiental(tblS_MedioAmbienteAspectoAmbiental aspectoAmbiental)
        {
            return medioAmbienteDAO.eliminarAspectoAmbiental(aspectoAmbiental);
        }

        public Dictionary<string, object> getClasificacionCombo()
        {
            return medioAmbienteDAO.getClasificacionCombo();
        }
        #endregion

        #region CAPTURAS
        #region ACOPIO
        public Dictionary<string, object> GetCapturas(CapturaDTO _objFiltroDTO)
        {
            return medioAmbienteDAO.GetCapturas(_objFiltroDTO);
        }

        public Dictionary<string, object> CrearEditarCaptura(CapturaDTO _objCEDTO, HttpPostedFileBase _objFile)
        {
            return medioAmbienteDAO.CrearEditarCaptura(_objCEDTO, _objFile);
        }

        public Dictionary<string, object> EliminarCaptura(int _idCaptura)
        {
            return medioAmbienteDAO.EliminarCaptura(_idCaptura);
        }

        public Dictionary<string, object> GetDatosActualizarCaptura(int _idCaptura)
        {
            return medioAmbienteDAO.GetDatosActualizarCaptura(_idCaptura);
        }

        public Dictionary<string, object> GetUltimoConsecutivoCodContenedor(int idAgrupacion, int idAspectoAmbiental, 
                                                                                List<tblS_IncidentesAgrupacionCC> lstIndicentesAgrupacionCC,
                                                                                List<tblS_MedioAmbienteAspectoAmbiental> lstMedioAmbienteAspectoAmbiental,
                                                                                List<tblS_MedioAmbienteCapturaDet> lstMedioAmbienteCapturaDet)
        {
            return medioAmbienteDAO.GetUltimoConsecutivoCodContenedor(idAgrupacion, idAspectoAmbiental, lstIndicentesAgrupacionCC, lstMedioAmbienteAspectoAmbiental, lstMedioAmbienteCapturaDet);
        }
        #endregion

        #region TRAYECTOS
        public Dictionary<string, object> CrearEditarTrayecto(CapturaDTO _objCEDTO, HttpPostedFileBase _objFile)
        {
            return medioAmbienteDAO.CrearEditarTrayecto(_objCEDTO, _objFile);
        }

        public Dictionary<string, object> EliminarTrayecto(int _idCaptura)
        {
            return medioAmbienteDAO.EliminarTrayecto(_idCaptura);
        }

        public Dictionary<string, object> GetDatosActualizarTrayecto(int _idCaptura)
        {
            return medioAmbienteDAO.GetDatosActualizarTrayecto(_idCaptura);
        }

        public Dictionary<string, object> GetAspectosAmbientalesToTrayectos(int idAgrupacion, string consecutivo)
        {
            return medioAmbienteDAO.GetAspectosAmbientalesToTrayectos(idAgrupacion, consecutivo);
        }

        public Dictionary<string, object> CrearAspectoAmbientalAcopioToTrayecto(AspectosAmbientalesToTrayectosDTO objParamDTO, HttpPostedFileBase objFile)
        {
            return medioAmbienteDAO.CrearAspectoAmbientalAcopioToTrayecto(objParamDTO, objFile);
        }
        #endregion

        #region DESTINO FINAL
        public Dictionary<string, object> CrearEditarDestinoFinal(CapturaDTO _objCEDTO, HttpPostedFileBase _objFile)
        {
            return medioAmbienteDAO.CrearEditarDestinoFinal(_objCEDTO, _objFile);
        }

        public Dictionary<string, object> EliminarDestinoFinal(int _idCaptura)
        {
            return medioAmbienteDAO.EliminarDestinoFinal(_idCaptura);
        }

        public Dictionary<string, object> GetDatosActualizarDestinoFinal(int _idCaptura)
        {
            return medioAmbienteDAO.GetDatosActualizarDestinoFinal(_idCaptura);
        }

        public Dictionary<string, object> GetAspectosAmbientalesToDestinoFinal(int idAgrupacion, string consecutivo)
        {
            return medioAmbienteDAO.GetAspectosAmbientalesToDestinoFinal(idAgrupacion, consecutivo);
        }

        public Dictionary<string, object> CrearDestinoFinal(AspectosAmbientalesToDestinoFinalDTO objParamDTO, HttpPostedFileBase objFile)
        {
            return medioAmbienteDAO.CrearDestinoFinal(objParamDTO, objFile);
        }
        #endregion

        #region ARCHIVOS REL CAPTURAS
        public Dictionary<string, object> GetArchivosRelCapturas(CapturaDTO objParamDTO)
        {
            return medioAmbienteDAO.GetArchivosRelCapturas(objParamDTO);
        }

        public Dictionary<string, object> VisualizarArchivo(int idArchivo)
        {
            return medioAmbienteDAO.VisualizarArchivo(idArchivo);
        }
        #endregion
        #endregion

        #region TRANSPORTISTAS
        public Dictionary<string, object> GetTransportistas(TransportistasDTO _objFiltroDTO)
        {
            return medioAmbienteDAO.GetTransportistas(_objFiltroDTO);
        }

        public Dictionary<string, object> CrearEditarTransportista(TransportistasDTO _objTransportistaDTO)
        {
            return medioAmbienteDAO.CrearEditarTransportista(_objTransportistaDTO);
        }

        public Dictionary<string, object> EliminarTransportista(int _idTransportista)
        {
            return medioAmbienteDAO.EliminarTransportista(_idTransportista);
        }

        public Dictionary<string, object> FillCboClasificacionesTransportistas()
        {
            return medioAmbienteDAO.FillCboClasificacionesTransportistas();
        }
        #endregion

        #region CLASIFICACIÓN TRANSPORTISTAS
        public Dictionary<string, object> GetClasificacionesTransportistas(ClasificacionTransportistaDTO _objFiltroDTO)
        {
            return medioAmbienteDAO.GetClasificacionesTransportistas(_objFiltroDTO);
        }

        public Dictionary<string, object> CrearEditarClasificacionTransportista(ClasificacionTransportistaDTO _objClasificacionTransportistaDTO)
        {
            return medioAmbienteDAO.CrearEditarClasificacionTransportista(_objClasificacionTransportistaDTO);
        }

        public Dictionary<string, object> EliminarClasificacionTransportista(int _idClasificacionTransportista)
        {
            return medioAmbienteDAO.EliminarClasificacionTransportista(_idClasificacionTransportista);
        }
        #endregion

        #region DASHBOARD
        public Dictionary<string, object> GetGraficas(FiltroDTO objFiltroDTO)
        {
            return medioAmbienteDAO.GetGraficas(objFiltroDTO);
        }
        #endregion

        #region GENERALES
        public Tuple<Stream, string> DescargarArchivo(int _idCaptura, int _tipoArchivo)
        {
            return medioAmbienteDAO.DescargarArchivo(_idCaptura, _tipoArchivo);
        }
        #endregion

        #region FILL COMBOS
        public Dictionary<string, object> FillCboAgrupaciones()
        {
            return medioAmbienteDAO.FillCboAgrupaciones();
        }

        public Dictionary<string, object> FillCboUsuarios()
        {
            return medioAmbienteDAO.FillCboUsuarios();
        }

        public Dictionary<string, object> FillCboAspectosAmbientales()
        {
            return medioAmbienteDAO.FillCboAspectosAmbientales();
        }

        public Dictionary<string, object> FillCboTransportistas()
        {
            return medioAmbienteDAO.FillCboTransportistas();
        }
        #endregion
    }
}
