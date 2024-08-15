using Core.DAO.RecursosHumanos.Tabuladores;
using Core.DTO.RecursosHumanos.Tabuladores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.RecursosHumanos.Tabuladores
{
    public class TabuladoresService : ITabuladoresDAO
    {
        #region INIT
        public ITabuladoresDAO r_tabuladores { get; set; }
        public ITabuladoresDAO Reclutamientos
        {
            get { return r_tabuladores; }
            set { r_tabuladores = value; }
        }
        public TabuladoresService(ITabuladoresDAO Reclutamientos)
        {
            this.Reclutamientos = Reclutamientos;
        }
        #endregion

        #region LINEA DE NEGOCIOS
        public Dictionary<string, object> GetLineaNegocios()
        {
            return r_tabuladores.GetLineaNegocios();
        }

        public Dictionary<string, object> CELineaNegocio(LineaNegocioDTO objFiltroDTO)
        {
            return r_tabuladores.CELineaNegocio(objFiltroDTO);
        }

        public Dictionary<string, object> GetDatosActualizarLineaNegocio(LineaNegocioDTO objFiltroDTO)
        {
            return r_tabuladores.GetDatosActualizarLineaNegocio(objFiltroDTO);
        }

        public Dictionary<string, object> EliminarLineaNegocio(LineaNegocioDTO objFiltroDTO)
        {
            return r_tabuladores.EliminarLineaNegocio(objFiltroDTO);
        }

        #region LINEA DE NEGOCIOS REL CC
        public Dictionary<string, object> GetLineaNegociosRelCC(LineaNegocioDetDTO objFiltroDTO)
        {
            return r_tabuladores.GetLineaNegociosRelCC(objFiltroDTO);
        }

        public Dictionary<string, object> CELineaNegocioRelCC(LineaNegocioDetDTO objFiltroDTO)
        {
            return r_tabuladores.CELineaNegocioRelCC(objFiltroDTO);
        }

        public Dictionary<string, object> EliminarLineaNegocioRelCC(LineaNegocioDetDTO objFiltroDTO)
        {
            return r_tabuladores.EliminarLineaNegocioRelCC(objFiltroDTO);
        }

        public Dictionary<string, object> FillCboCCDisponibles(LineaNegocioDetDTO objFiltroDTO)
        {
            return r_tabuladores.FillCboCCDisponibles(objFiltroDTO);
        }
        #endregion
        #endregion

        #region ASIGNACIÓN TABULADORES
        public Dictionary<string, object> InitCEAsignacionTabulador()
        {
            return r_tabuladores.InitCEAsignacionTabulador();
        }

        public Dictionary<string, object> GetAsignacionTabuladores(TabuladorDetDTO objFiltroDTO)
        {
            return r_tabuladores.GetAsignacionTabuladores(objFiltroDTO);
        }

        public Dictionary<string, object> CrearAsignacionTabulador(TabuladorDTO objTabuladorDTO, List<TabuladorDetDTO> lstTabuladoresDTO, List<GestionAutorizanteDTO> lstGestionAutorizantesDTO)
        {
            return r_tabuladores.CrearAsignacionTabulador(objTabuladorDTO, lstTabuladoresDTO, lstGestionAutorizantesDTO);
        }

        public Dictionary<string, object> EliminarTabuladoresPuesto(TabuladorDTO objFiltroDTO)
        {
            return r_tabuladores.EliminarTabuladoresPuesto(objFiltroDTO);
        }

        public Dictionary<string, object> GetTabuladoresExistentes(TabuladorDTO objParamDTO)
        {
            return r_tabuladores.GetTabuladoresExistentes(objParamDTO);
        }

        #region TABULADORES DET
        public Dictionary<string, object> GetListadoTabuladoresDet(TabuladorDetDTO objFiltroDTO)
        {
            return r_tabuladores.GetListadoTabuladoresDet(objFiltroDTO);
        }

        public Dictionary<string, object> GetDatosActualizarTabuladorDet(TabuladorDetDTO objFiltroDTO)
        {
            return r_tabuladores.GetDatosActualizarTabuladorDet(objFiltroDTO);
        }

        public Dictionary<string, object> ActualizarTabuladorDet(TabuladorDetDTO objFiltroDTO)
        {
            return r_tabuladores.ActualizarTabuladorDet(objFiltroDTO);
        }

        public Dictionary<string, object> EliminarTabuladorDet(TabuladorDetDTO objParamDTO)
        {
            return r_tabuladores.EliminarTabuladorDet(objParamDTO);
        }
        #endregion
        #endregion

        #region PLANTILLA DE PERSONAL
        public Dictionary<string, object> GetTabuladoresAutorizados(TabuladorDTO objParamDTO)
        {
            return r_tabuladores.GetTabuladoresAutorizados(objParamDTO);
        }

        public Dictionary<string, object> FillCboFiltroCC_PlantillaPersonal(TabuladorDTO objParamDTO)
        {
            return r_tabuladores.FillCboFiltroCC_PlantillaPersonal(objParamDTO);
        }

        public Dictionary<string, object> CrearSolicitudPlantilla(TabuladorDTO objParamDTO)
        {
            return r_tabuladores.CrearSolicitudPlantilla(objParamDTO);
        }

        public Dictionary<string, object> NotificarPlantilla(string ccPlantilla, bool esAuthCompleta)
        {
            return r_tabuladores.NotificarPlantilla(ccPlantilla, esAuthCompleta);
        }

        public Dictionary<string, object> FillCboFiltroPuestos_PlantillaPersonal(TabuladorDTO objParamDTO)
        {
            return r_tabuladores.FillCboFiltroPuestos_PlantillaPersonal(objParamDTO);
        }

        public Dictionary<string, object> FillCboFiltroLineaNegocios_PlantillaPersonal(TabuladorDTO objParamDTO)
        {
            return r_tabuladores.FillCboFiltroLineaNegocios_PlantillaPersonal(objParamDTO);
        }
        #endregion

        #region MODIFICACIÓN
        public Dictionary<string, object> FillCboTipoModificaciones()
        {
            return r_tabuladores.FillCboTipoModificaciones();
        }

        public Dictionary<string, object> GetTabuladoresModificacion(TabuladorDTO objParamDTO)
        {
            return r_tabuladores.GetTabuladoresModificacion(objParamDTO);
        }

        public Dictionary<string, object> FillCboFiltroPuestos_Modificacion(TabuladorDTO objParamDTO)
        {
            return r_tabuladores.FillCboFiltroPuestos_Modificacion(objParamDTO);
        }

        public Dictionary<string, object> CrearModificacion(GestionModificacionTabuladorDTO objParamDTO, List<GestionModificacionTabuladorDetDTO> lstParamDTO, List<GestionAutorizanteDTO> lstGestionAutorizantesDTO)
        {
            return r_tabuladores.CrearModificacion(objParamDTO, lstParamDTO, lstGestionAutorizantesDTO);
        }

        public Dictionary<string, object> GetTabuladoresEmpleadosActivos(TabuladorDTO objParamDTO)
        {
            return r_tabuladores.GetTabuladoresEmpleadosActivos(objParamDTO);
        }

        public Dictionary<string, object> GetTabuladoresPuestos(TabuladorDTO objParamDTO)
        {
            return r_tabuladores.GetTabuladoresPuestos(objParamDTO);
        }
        #endregion

        #region GESTIÓN TABULADORES
        public Dictionary<string, object> GetGestionTabuladores(TabuladorDTO objParamDTO)
        {
            return r_tabuladores.GetGestionTabuladores(objParamDTO);
        }

        public Dictionary<string, object> AutorizarRechazarTabulador(TabuladorDTO objParamDTO)
        {
            return r_tabuladores.AutorizarRechazarTabulador(objParamDTO);
        }

        public Dictionary<string, object> GetLstAutorizantesTabulador(GestionAutorizanteDTO objParamDTO)
        {
            return r_tabuladores.GetLstAutorizantesTabulador(objParamDTO);
        }

        public Dictionary<string, object> GuardarComentarioRechazoTabulador(int idTabulador, string comentario)
        {
            return r_tabuladores.GuardarComentarioRechazoTabulador(idTabulador, comentario);
        }

        public Dictionary<string, object> GetDetalleRelTabulador(TabuladorDetDTO objParamDTO)
        {
            return r_tabuladores.GetDetalleRelTabulador(objParamDTO);
        }
        #endregion

        #region GESTIÓN PLANTILLAS PERSONAL
        public Dictionary<string, object> GetGestionPlantillasPersonal(PlantillaPersonalDTO objParamDTO)
        {
            return r_tabuladores.GetGestionPlantillasPersonal(objParamDTO);
        }

        public Dictionary<string, object> AutorizarRechazarPlantillaPersonal(PlantillaPersonalDTO objParamDTO)
        {
            return r_tabuladores.AutorizarRechazarPlantillaPersonal(objParamDTO);
        }

        public Dictionary<string, object> GetLstAutorizantesPlantilla(int idPlantilla)
        {
            return r_tabuladores.GetLstAutorizantesPlantilla(idPlantilla);
        }

        public Dictionary<string, object> GuardarComentarioRechazoPlantilla(int idPlantilla, string comentario)
        {
            return r_tabuladores.GuardarComentarioRechazoPlantilla(idPlantilla, comentario);
        }

        public Dictionary<string, object> GetPlantillaDetalle(int plantilla_id)
        {
            return r_tabuladores.GetPlantillaDetalle(plantilla_id);
        }

        public Dictionary<string, object> GetDetallePlantillaTabuladores(PlantillaPersonalDTO objParamDTO)
        {
            return r_tabuladores.GetDetallePlantillaTabuladores(objParamDTO);
        }
        #endregion

        #region GESTIÓN MODIFICACIÓN
        public Dictionary<string, object> GetGestionModificacion(GestionModificacionTabuladorDTO objParamDTO)
        {
            return r_tabuladores.GetGestionModificacion(objParamDTO);
        }

        public Dictionary<string, object> AutorizarRechazarGestionModificacion(GestionModificacionTabuladorDTO objParamDTO)
        {
            return r_tabuladores.AutorizarRechazarGestionModificacion(objParamDTO);
        }

        public Dictionary<string, object> GetLstAutorizantesModificacion(int idModificacion)
        {
            return r_tabuladores.GetLstAutorizantesModificacion(idModificacion);
        }

        public Dictionary<string, object> GuardarComentarioRechazoModificacion(int idModificacion, string comentario)
        {
            return r_tabuladores.GuardarComentarioRechazoModificacion(idModificacion, comentario);
        }

        public Dictionary<string, object> GetModificacionDetalle(GestionModificacionTabuladorDetDTO objParamDTO)
        {
            return r_tabuladores.GetModificacionDetalle(objParamDTO);
        }

        public Dictionary<string, object> GetTabuladoresModificacionReportePDF(TabuladorDTO objParamDTO)
        {
            return r_tabuladores.GetTabuladoresModificacionReportePDF(objParamDTO);
        }
        #endregion

        #region REPORTES
        public Dictionary<string, object> GetTabuladoresReporte(TabuladorDTO objParamDTO)
        {
            return r_tabuladores.GetTabuladoresReporte(objParamDTO);
        }
        public List<string> GetCCLineaNegocios(List<int> lineasNegocio)
        {
            return r_tabuladores.GetCCLineaNegocios(lineasNegocio);
        }

        public Dictionary<string, object> GetTabuladoresReportePdf(TabuladorDTO objParamDTO)
        {
            return r_tabuladores.GetTabuladoresReportePdf(objParamDTO);
        }

        public Dictionary<string, object> SendReporteCorreo(TabuladorDTO objParamDTO)
        {
            return r_tabuladores.SendReporteCorreo(objParamDTO);
        }

        public Dictionary<string, object> FillCboFiltroPuestos_Reportes(TabuladorDTO objParamDTO)
        {
            return r_tabuladores.FillCboFiltroPuestos_Reportes(objParamDTO);
        }

        #endregion

        #region GESTION REPORTES
        public Dictionary<string, object> GetGestionReportes(ReportesTabuladoresDTO objParamDTO)
        {
            return r_tabuladores.GetGestionReportes(objParamDTO);
        }

        public Dictionary<string, object> AutorizarRechazarReporte(ReportesTabuladoresDTO objParamDTO)
        {
            return r_tabuladores.AutorizarRechazarReporte(objParamDTO);
        }

        public Dictionary<string, object> GetLstAutorizantesReporte(int idReporte)
        {
            return r_tabuladores.GetLstAutorizantesReporte(idReporte);
        }

        public Dictionary<string, object> GuardarComentarioRechazoReporte(int idReporte, string comentario)
        {
            return r_tabuladores.GuardarComentarioRechazoReporte(idReporte, comentario);
        }

        public Dictionary<string, object> GetTabuladoresReporteByCC(TabuladorDTO objParamDTO)
        {
            return r_tabuladores.GetTabuladoresReporteByCC(objParamDTO);
        }

        public TabuladorDTO GetParametrosReporte(int idReporte)
        {
            return r_tabuladores.GetParametrosReporte(idReporte);
        }

        public Dictionary<string, object> AutorizarReportesMasivo(List<int> lstIdReportes)
        {
            return r_tabuladores.AutorizarReportesMasivo(lstIdReportes);
        }
        #endregion

        #region ACCESO MENU
        public Dictionary<string, object> GetAccesosMenu()
        {
            return r_tabuladores.GetAccesosMenu();
        }

        public bool VerificarAcceso(int FK_Menu)
        {
            return r_tabuladores.VerificarAcceso(FK_Menu);
        }
        #endregion

        #region GENERALES
        public Dictionary<string, object> FillCboPuestos()
        {
            return r_tabuladores.FillCboPuestos();
        }

        public Dictionary<string, object> GetInformacionPuesto(PuestoDTO objFiltroDTO)
        {
            return r_tabuladores.GetInformacionPuesto(objFiltroDTO);
        }

        public Dictionary<string, object> FillCboCategorias()
        {
            return r_tabuladores.FillCboCategorias();
        }

        public Dictionary<string, object> FillCboEsquemaPagos()
        {
            return r_tabuladores.FillCboEsquemaPagos();
        }

        public Dictionary<string, object> FillCboLineaNegocios(TabuladorDTO objParamDTO)
        {
            return r_tabuladores.FillCboLineaNegocios(objParamDTO);
        }

        public Dictionary<string, object> FillCboAreasDepartamentos()
        {
            return r_tabuladores.FillCboAreasDepartamentos();
        }

        public Dictionary<string, object> FillCboTipoNomina()
        {
            return r_tabuladores.FillCboTipoNomina();
        }

        public Dictionary<string, object> FillCboSindicatos()
        {
            return r_tabuladores.FillCboSindicatos();
        }

        public Dictionary<string, object> FillCboNivelMando()
        {
            return r_tabuladores.FillCboNivelMando();
        }

        public Dictionary<string, object> FillCboUsuarios()
        {
            return r_tabuladores.FillCboUsuarios();
        }

        public Dictionary<string, object> FillCboCC(List<int> lstFK_LineaNegocio)
        {
            return r_tabuladores.FillCboCC(lstFK_LineaNegocio);
        }

        public Dictionary<string, object> FillCboGestionEstatus()
        {
            return r_tabuladores.FillCboGestionEstatus();
        }

        public List<string> GetDescLineaNegocio(List<int> lstLineasNegocio)
        {
            return r_tabuladores.GetDescLineaNegocio(lstLineasNegocio);
        }

        public List<string> GetDescCCs(List<string> lstCCs)
        {
            return r_tabuladores.GetDescCCs(lstCCs);
        }

        public List<RepAutorizantesTABDTO> GetAutorizantesReporte(List<GestionAutorizanteDTO> lstGestionAutorizantesDTO)
        {
            return r_tabuladores.GetAutorizantesReporte(lstGestionAutorizantesDTO);
        }
        
        public Dictionary<string, object> GetFechaActual()
        {
            return r_tabuladores.GetFechaActual();
        }
        #endregion
    }
}
