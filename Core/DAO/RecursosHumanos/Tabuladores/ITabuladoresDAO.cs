using Core.DTO.RecursosHumanos.Tabuladores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.RecursosHumanos.Tabuladores
{
    public interface ITabuladoresDAO
    {
        #region LINEA DE NEGOCIOS
        Dictionary<string, object> GetLineaNegocios();

        Dictionary<string, object> CELineaNegocio(LineaNegocioDTO objFiltroDTO);

        Dictionary<string, object> GetDatosActualizarLineaNegocio(LineaNegocioDTO objFiltroDTO);

        Dictionary<string, object> EliminarLineaNegocio(LineaNegocioDTO objFiltroDTO);

        #region LINEA DE NEGOCIOS REL CC
        Dictionary<string, object> GetLineaNegociosRelCC(LineaNegocioDetDTO objFiltroDTO);

        Dictionary<string, object> CELineaNegocioRelCC(LineaNegocioDetDTO objFiltroDTO);

        Dictionary<string, object> EliminarLineaNegocioRelCC(LineaNegocioDetDTO objFiltroDTO);

        Dictionary<string, object> FillCboCCDisponibles(LineaNegocioDetDTO objFiltroDTO);
        #endregion
        #endregion

        #region ASIGNACIÓN TABULADORES
        Dictionary<string, object> InitCEAsignacionTabulador();

        Dictionary<string, object> GetAsignacionTabuladores(TabuladorDetDTO objFiltroDTO);

        Dictionary<string, object> CrearAsignacionTabulador(TabuladorDTO objTabuladorDTO, List<TabuladorDetDTO> lstTabuladoresDTO, List<GestionAutorizanteDTO> lstGestionAutorizantesDTO);

        Dictionary<string, object> EliminarTabuladoresPuesto(TabuladorDTO objFiltroDTO);

        Dictionary<string, object> GetTabuladoresExistentes(TabuladorDTO objParamDTO);

        #region TABULADORES DET
        Dictionary<string, object> GetListadoTabuladoresDet(TabuladorDetDTO objFiltroDTO);

        Dictionary<string, object> GetDatosActualizarTabuladorDet(TabuladorDetDTO objFiltroDTO);

        Dictionary<string, object> ActualizarTabuladorDet(TabuladorDetDTO objFiltroDTO);

        Dictionary<string, object> EliminarTabuladorDet(TabuladorDetDTO objParamDTO);
        #endregion
        #endregion

        #region PLANTILLA DE PERSONAL
        Dictionary<string, object> GetTabuladoresAutorizados(TabuladorDTO objParamDTO);

        Dictionary<string, object> FillCboFiltroCC_PlantillaPersonal(TabuladorDTO objParamDTO);

        Dictionary<string, object> CrearSolicitudPlantilla(TabuladorDTO objParamDTO);

        Dictionary<string, object> NotificarPlantilla(string ccPlantilla, bool esAuthCompleta);

        Dictionary<string, object> FillCboFiltroPuestos_PlantillaPersonal(TabuladorDTO objParamDTO);

        Dictionary<string, object> FillCboFiltroLineaNegocios_PlantillaPersonal(TabuladorDTO objParamDTO);
        #endregion

        #region MODIFICACIÓN
        Dictionary<string, object> FillCboTipoModificaciones();

        Dictionary<string, object> GetTabuladoresModificacion(TabuladorDTO objParamDTO);

        Dictionary<string, object> FillCboFiltroPuestos_Modificacion(TabuladorDTO objParamDTO);

        Dictionary<string, object> CrearModificacion(GestionModificacionTabuladorDTO objParamDTO, List<GestionModificacionTabuladorDetDTO> lstParamDTO, List<GestionAutorizanteDTO> lstGestionAutorizantesDTO);

        Dictionary<string, object> GetTabuladoresEmpleadosActivos(TabuladorDTO objParamDTO);

        Dictionary<string, object> GetTabuladoresPuestos(TabuladorDTO objParamDTO);
        #endregion

        #region GESTIÓN TABULADORES
        Dictionary<string, object> GetGestionTabuladores(TabuladorDTO objParamDTO);

        Dictionary<string, object> AutorizarRechazarTabulador(TabuladorDTO objParamDTO);

        Dictionary<string, object> GetLstAutorizantesTabulador(GestionAutorizanteDTO objParamDTO);

        Dictionary<string, object> GuardarComentarioRechazoTabulador(int idTabulador, string comentario);

        Dictionary<string, object> GetDetalleRelTabulador(TabuladorDetDTO objParamDTO);
        #endregion

        #region GESTIÓN PLANTILLAS PERSONAL
        Dictionary<string, object> GetGestionPlantillasPersonal(PlantillaPersonalDTO objParamDTO);

        Dictionary<string, object> AutorizarRechazarPlantillaPersonal(PlantillaPersonalDTO objParamDTO);

        Dictionary<string, object> GetLstAutorizantesPlantilla(int idPlantilla);

        Dictionary<string, object> GuardarComentarioRechazoPlantilla(int idPlantilla, string comentario);

        Dictionary<string, object> GetPlantillaDetalle(int plantilla_id);

        Dictionary<string, object> GetDetallePlantillaTabuladores(PlantillaPersonalDTO objParamDTO);
        #endregion

        #region GESTIÓN MODIFICACIÓN
        Dictionary<string, object> GetGestionModificacion(GestionModificacionTabuladorDTO objParamDTO);

        Dictionary<string, object> AutorizarRechazarGestionModificacion(GestionModificacionTabuladorDTO objParamDTO);

        Dictionary<string, object> GetLstAutorizantesModificacion(int idModificacion);

        Dictionary<string, object> GuardarComentarioRechazoModificacion(int idModificacion, string comentario);

        Dictionary<string, object> GetModificacionDetalle(GestionModificacionTabuladorDetDTO objParamDTO);

        Dictionary<string, object> GetTabuladoresModificacionReportePDF(TabuladorDTO objParamDTO);
        #endregion

        #region REPORTES
        Dictionary<string, object> GetTabuladoresReporte(TabuladorDTO objParamDTO);
        List<string> GetCCLineaNegocios(List<int> lineasNegocio);
        Dictionary<string, object> GetTabuladoresReportePdf(TabuladorDTO objParamDTO);
        Dictionary<string, object> SendReporteCorreo(TabuladorDTO objParamDTO);
        Dictionary<string, object> FillCboFiltroPuestos_Reportes(TabuladorDTO objParamDTO);

        #endregion

        #region GESTION REPORTES
        Dictionary<string, object> GetGestionReportes(ReportesTabuladoresDTO objParamDTO);
        Dictionary<string, object> AutorizarRechazarReporte(ReportesTabuladoresDTO objParamDTO);
        Dictionary<string, object> GetLstAutorizantesReporte(int idReporte);
        Dictionary<string, object> GuardarComentarioRechazoReporte(int idReporte, string comentario);
        Dictionary<string, object> GetTabuladoresReporteByCC(TabuladorDTO objParamDTO);
        TabuladorDTO GetParametrosReporte(int idReporte);
        Dictionary<string, object> AutorizarReportesMasivo(List<int> lstIdReportes);
        #endregion

        #region ACCESO MENU
        Dictionary<string, object> GetAccesosMenu();

        bool VerificarAcceso(int FK_Menu);
        #endregion

        #region GENERALES
        Dictionary<string, object> FillCboPuestos();

        Dictionary<string, object> GetInformacionPuesto(PuestoDTO objFiltroDTO);

        Dictionary<string, object> FillCboCategorias();

        Dictionary<string, object> FillCboEsquemaPagos();

        Dictionary<string, object> FillCboLineaNegocios(TabuladorDTO objParamDTO);

        Dictionary<string, object> FillCboAreasDepartamentos();

        Dictionary<string, object> FillCboTipoNomina();

        Dictionary<string, object> FillCboSindicatos();

        Dictionary<string, object> FillCboNivelMando();

        Dictionary<string, object> FillCboUsuarios();

        Dictionary<string, object> FillCboCC(List<int> lstFK_LineaNegocio);

        Dictionary<string, object> FillCboGestionEstatus();

        List<string> GetDescLineaNegocio(List<int> lstLineasNegocio);

        List<string> GetDescCCs(List<string> lstCCs);

        List<RepAutorizantesTABDTO> GetAutorizantesReporte(List<GestionAutorizanteDTO> lstGestionAutorizantesDTO);

        Dictionary<string, object> GetFechaActual();
        #endregion
    }
}
