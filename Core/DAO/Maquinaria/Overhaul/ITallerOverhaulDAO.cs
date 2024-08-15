using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;
using Core.DTO.Maquinaria.Overhaul;
using Core.DTO.Principal.Generales;
using Core.Enum.Principal;

namespace Core.DAO.Maquinaria.Overhaul
{
    public interface ITallerOverhaulDAO
    {
        List<tblM_CapPlaneacionOverhaul> CargarGridTallerEstatus(int idCalendario, int estatus, int tipo);
        string getCCByMaquinaID(int id);
        string getEconomicoByID(int id);
        List<ActividadOverhaulDTO> CargarGridModalTallerEstatus(int index);
        string getActividadByID(int id);
        bool IniciarActividadOverhaul(int actividadID, int eventoID);
        bool FinalizarActividadOverhaul(int actividadID, int eventoID);
        bool AgregarOverhaulTaller(int maquinaID, List<int> actividadesID);
        List<ComboDTO> FillCboEconomico_Componente(int modeloID);
        List<tblM_CatActividadOverhaul> CargarDatosDiagramaGantt(int idModelo, string descripcion, bool estatus);
        tblM_CatActividadOverhaul CargarActividad(int actividadID, List<tblM_CatActividadOverhaul> _lstCatActividadOverhaulDapperDTO = null);
        bool GuardarActividad(int actividadID, string descripcion, int modeloID, bool estatus, bool reporteEjecutivo, decimal horasDuracion);
        int getModeloIDByMaquinaID(int id);
        bool GuardarDiagramaGantt(int idEvento, string actividades, decimal sumaHoras, List<decimal> horasTrabajadas);
        List<ActividadOverhaulDTO> CargarActGuardadasDiagramaGantt(int idEvento);
        List<decimal> CargarDiasTrabajadosDG(int idEvento);
        bool IniciarOverhaul(int idEvento, DateTime fechaInicio, int tipoOverhaul);
        List<ActividadOverhaulDTO> CargarDatosActividadesTaller(int idEvento);
        string getDescripcionActividadOverhaul(int idActividad, List<tblM_CatActividadOverhaul> _lstCatActividadOverhaulDapperDTO = null);
        bool IniciarActividad(int idEvento, int idActividad, int id);
        bool FinalizarActividad(int idEvento, int idActividad, int id);
        bool GuardarComentarioActividad(int actividadID, int eventoID, string comentario, int tipo, int numDia);
        tblM_ComentarioActividadOverhaul CargarComentarioActividad(int actividadID, int eventoID, int tipo, int numDia);
        string CargarStringComentarioActividad(int actividadID, int eventoID, int tipo, DateTime fecha, int numDia, List<tblM_ComentarioActividadOverhaul> _lstComentarioActividadOverhaulDapperDTO = null);
        tblM_CapPlaneacionOverhaul getEventoOHByID(int id, List<tblM_CapPlaneacionOverhaul> _lstCapPlaneacionOverhaulDapperDTO = null);
        bool GuardarArchivoActividad(tblM_ComentarioActividadOverhaul archivo);
        List<tblM_ComentarioActividadOverhaul> CargarArchivosActividad(int idEvento, int idActividad, int tipo, int numDia, List<tblM_ComentarioActividadOverhaul> _lstComentarioActividadOverhaulDapperDTO = null);
        tblM_ComentarioActividadOverhaul getComentarioByID(int idComentario);
        bool EliminarArchivo(int idArchivo);
        int getArchivosCount(int idActividad);
        bool TerminarOverhaul(int idEvento, DateTime fechaFin, int tipoOverhaul, int estatus);
        string getCCByCalendarioID(int idCalendario);
        List<tblM_CatMaquina> getEconomicosByObraID(string obra);
        List<ComponentePlaneacionDTO> CargarGridOHFallaTaller(int idMaquina, bool planeacion);
        bool GuardarOHFallaTaller(int idMaquina, List<ComponentePlaneacionDTO> componentes, DateTime fecha, int calendarioID, int tipo);
        bool UpdateOHFallaTaller(int id, List<ComponentePlaneacionDTO> componentes, int calendarioID);
        tblM_CapPlaneacionOverhaul VerificarOHFallaTaller(int idMaquina, DateTime fecha, int calendarioID);
        bool GuardarOHParoTaller(int idMaquina, List<ComponentePlaneacionDTO> componentes, DateTime fecha, int calendarioID, string indexCal);
        bool CheckTallerRemocion(int componenteID, int maquinaID);
        bool TerminarOverhaul(int index);
        List<tblM_CatMaquina> getEconomicosByCalendarioID(int idCalendario);
        List<ActividadOverhaulDTO> CargarDatosDiagramaGantt(int idModelo, int eventoID);

        #region CONSULTAS CON DAPPER
        List<tblM_CapPlaneacionOverhaul> _lstCapPlaneacionOverhaulDapperDTO(MainContextEnum idEmpresa);

        List<tblM_ComentarioActividadOverhaul> _lstComentarioActividadOverhaulDapperDTO(MainContextEnum idEmpresa);

        List<tblM_CatActividadOverhaul> _lstCatActividadOverhaulDapperDTO(MainContextEnum idEmpresa);
        #endregion
    }
}
