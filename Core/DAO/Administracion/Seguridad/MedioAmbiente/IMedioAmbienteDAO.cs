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

namespace Core.DAO.Administracion.Seguridad.MedioAmbiente
{
    public interface IMedioAmbienteDAO
    {
        #region ASPECTOS AMBIENTALES
        Dictionary<string, object> getAspectosAmbientales(int tipoCaptura = 0);

        Dictionary<string, object> guardarNuevoAspectoAmbiental(tblS_MedioAmbienteAspectoAmbiental aspectoAmbiental);

        Dictionary<string, object> editarAspectoAmbiental(tblS_MedioAmbienteAspectoAmbiental aspectoAmbiental);

        Dictionary<string, object> eliminarAspectoAmbiental(tblS_MedioAmbienteAspectoAmbiental aspectoAmbiental);

        Dictionary<string, object> getClasificacionCombo();
        #endregion

        #region CAPTURAS
        #region ACOPIO
        Dictionary<string, object> GetCapturas(CapturaDTO _objFiltroDTO);

        Dictionary<string, object> CrearEditarCaptura(CapturaDTO _objCEDTO, HttpPostedFileBase _objFile);

        Dictionary<string, object> EliminarCaptura(int _idCaptura);

        Dictionary<string, object> GetDatosActualizarCaptura(int _idCaptura);

        Dictionary<string, object> GetUltimoConsecutivoCodContenedor(int idAgrupacion, int idAspectoAmbiental, 
                                                                                List<tblS_IncidentesAgrupacionCC> lstIndicentesAgrupacionCC, 
                                                                                List<tblS_MedioAmbienteAspectoAmbiental> lstMedioAmbienteAspectoAmbiental,
                                                                                List<tblS_MedioAmbienteCapturaDet> lstMedioAmbienteCapturaDet);
        #endregion

        #region TRAYECTOS
        Dictionary<string, object> CrearEditarTrayecto(CapturaDTO _objCEDTO, HttpPostedFileBase _objFile);

        Dictionary<string, object> EliminarTrayecto(int _idCaptura);

        Dictionary<string, object> GetDatosActualizarTrayecto(int _idCaptura);

        Dictionary<string, object> GetAspectosAmbientalesToTrayectos(int idAgrupacion, string consecutivo);

        Dictionary<string, object> CrearAspectoAmbientalAcopioToTrayecto(AspectosAmbientalesToTrayectosDTO objParamDTO, HttpPostedFileBase objFile);
        #endregion

        #region DESTINO FINAL
        Dictionary<string, object> CrearEditarDestinoFinal(CapturaDTO _objCEDTO, HttpPostedFileBase _objFile);

        Dictionary<string, object> EliminarDestinoFinal(int _idCaptura);

        Dictionary<string, object> GetDatosActualizarDestinoFinal(int _idCaptura);

        Dictionary<string, object> GetAspectosAmbientalesToDestinoFinal(int idAgrupacion, string consecutivo);

        Dictionary<string, object> CrearDestinoFinal(AspectosAmbientalesToDestinoFinalDTO objParamDTO, HttpPostedFileBase objFile);
        #endregion

        #region ARCHIVOS REL CAPTURAS
        Dictionary<string, object> GetArchivosRelCapturas(CapturaDTO objParamDTO);

        Dictionary<string, object> VisualizarArchivo(int idArchivo);
        #endregion
        #endregion

        #region TRANSPORTISTAS
        Dictionary<string, object> GetTransportistas(TransportistasDTO _objFiltroDTO);

        Dictionary<string, object> CrearEditarTransportista(TransportistasDTO _objTransportistaDTO);

        Dictionary<string, object> EliminarTransportista(int _idTransportista);

        Dictionary<string, object> FillCboClasificacionesTransportistas();
        #endregion

        #region CLASIFICACIÓN TRANSPORTISTAS
        Dictionary<string, object> GetClasificacionesTransportistas(ClasificacionTransportistaDTO _objFiltroDTO);

        Dictionary<string, object> CrearEditarClasificacionTransportista(ClasificacionTransportistaDTO _objClasificacionTransportistaDTO);

        Dictionary<string, object> EliminarClasificacionTransportista(int _idClasificacionTransportista);
        #endregion

        #region DASHBOARD
        Dictionary<string, object> GetGraficas(FiltroDTO objFiltroDTO);
        #endregion

        #region GENERALES
        Tuple<Stream, string> DescargarArchivo(int _idCaptura, int _tipoArchivo);
        #endregion

        #region FILL COMBOS
        Dictionary<string, object> FillCboAgrupaciones();

        Dictionary<string, object> FillCboUsuarios();

        Dictionary<string, object> FillCboAspectosAmbientales();

        Dictionary<string, object> FillCboTransportistas();
        #endregion
    }
}