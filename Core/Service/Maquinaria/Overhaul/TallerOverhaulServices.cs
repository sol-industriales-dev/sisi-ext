using Core.DAO.Maquinaria.Overhaul;
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

namespace Core.Service.Maquinaria.Overhaul
{
    public class TallerOverhaulServices : ITallerOverhaulDAO
    {
        #region Atributos
        private ITallerOverhaulDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private ITallerOverhaulDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public TallerOverhaulServices(ITallerOverhaulDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores

        public List<tblM_CapPlaneacionOverhaul> CargarGridTallerEstatus(int idCalendario, int estatus, int tipo) 
        {
            return interfazDAO.CargarGridTallerEstatus(idCalendario, estatus, tipo);
        }

        public string getCCByMaquinaID(int id)
        {
            return interfazDAO.getCCByMaquinaID(id);
        }

        public string getEconomicoByID(int id)
        {
            return interfazDAO.getEconomicoByID(id);
        }

        public List<ActividadOverhaulDTO> CargarGridModalTallerEstatus(int index)
        {
            return interfazDAO.CargarGridModalTallerEstatus(index);
        }

        public string getActividadByID(int id)
        {
            return interfazDAO.getActividadByID(id);
        }

        public bool IniciarActividadOverhaul(int actividadID, int eventoID)
        {
            return interfazDAO.IniciarActividadOverhaul(actividadID, eventoID);
        }

        public bool FinalizarActividadOverhaul(int actividadID, int eventoID)
        {
            return interfazDAO.FinalizarActividadOverhaul(actividadID, eventoID);
        }

        public bool AgregarOverhaulTaller(int maquinaID, List<int> actividadesID)
        {
            return interfazDAO.AgregarOverhaulTaller(maquinaID, actividadesID);
        }

        public List<ComboDTO> FillCboEconomico_Componente(int modeloID)
        {
            return interfazDAO.FillCboEconomico_Componente(modeloID);
        }

        public List<tblM_CatActividadOverhaul> CargarDatosDiagramaGantt(int idModelo, string descripcion, bool estatus)
        {
            return interfazDAO.CargarDatosDiagramaGantt(idModelo, descripcion, estatus);
        }
        public tblM_CatActividadOverhaul CargarActividad(int actividadID, List<tblM_CatActividadOverhaul> _lstCatActividadOverhaulDapperDTO = null)
        {
            return interfazDAO.CargarActividad(actividadID, _lstCatActividadOverhaulDapperDTO);
        }
        public bool GuardarActividad(int actividadID, string descripcion, int modeloID, bool estatus, bool reporteEjecutivo, decimal horasDuracion)
        {
            return interfazDAO.GuardarActividad(actividadID, descripcion, modeloID, estatus, reporteEjecutivo, horasDuracion);
        }
        public int getModeloIDByMaquinaID(int id)
        {
            return interfazDAO.getModeloIDByMaquinaID(id);
        }
        public bool GuardarDiagramaGantt(int idEvento, string actividades, decimal sumaHoras, List<decimal> horasTrabajadas)
        {
            return interfazDAO.GuardarDiagramaGantt(idEvento, actividades, sumaHoras, horasTrabajadas);
        }
        public List<ActividadOverhaulDTO> CargarActGuardadasDiagramaGantt(int idEvento)
        {
            return interfazDAO.CargarActGuardadasDiagramaGantt(idEvento);
        }
        public List<decimal> CargarDiasTrabajadosDG(int idEvento)
        {
            return interfazDAO.CargarDiasTrabajadosDG(idEvento);
        }
        public bool IniciarOverhaul(int idEvento, DateTime fechaInicio, int tipoOverhaul)
        {
            return interfazDAO.IniciarOverhaul(idEvento, fechaInicio, tipoOverhaul);
        }
        public List<ActividadOverhaulDTO> CargarDatosActividadesTaller(int idEvento)
        {
            return interfazDAO.CargarDatosActividadesTaller(idEvento);
        }
        public string getDescripcionActividadOverhaul(int idActividad, List<tblM_CatActividadOverhaul> _lstCatActividadOverhaulDapperDTO = null)
        {
            return interfazDAO.getDescripcionActividadOverhaul(idActividad, _lstCatActividadOverhaulDapperDTO);
        }
        public bool IniciarActividad(int idEvento, int idActividad, int id)
        {
            return interfazDAO.IniciarActividad(idEvento, idActividad, id);
        }
        public bool FinalizarActividad(int idEvento, int idActividad, int id)
        {
            return interfazDAO.FinalizarActividad(idEvento, idActividad, id);
        }
        public bool GuardarComentarioActividad(int actividadID, int eventoID, string comentario, int tipo, int numDia)
        {
            return interfazDAO.GuardarComentarioActividad(actividadID, eventoID, comentario, tipo, numDia);
        }
        public tblM_ComentarioActividadOverhaul CargarComentarioActividad(int actividadID, int eventoID, int tipo, int numDia)
        {
            return interfazDAO.CargarComentarioActividad(actividadID, eventoID, tipo, numDia);
        }
        public tblM_CapPlaneacionOverhaul getEventoOHByID(int id, List<tblM_CapPlaneacionOverhaul> _lstCapPlaneacionOverhaulDapperDTO = null)
        {
            return interfazDAO.getEventoOHByID(id, _lstCapPlaneacionOverhaulDapperDTO);
        }
        public bool GuardarArchivoActividad(tblM_ComentarioActividadOverhaul archivo)
        {
            return interfazDAO.GuardarArchivoActividad(archivo);
        }
        public List<tblM_ComentarioActividadOverhaul> CargarArchivosActividad(int idEvento, int idActividad, int tipo, int numDia, List<tblM_ComentarioActividadOverhaul> _lstComentarioActividadOverhaulDapperDTO = null)
        {
            return interfazDAO.CargarArchivosActividad(idEvento, idActividad, tipo, numDia, _lstComentarioActividadOverhaulDapperDTO);
        }
        public tblM_ComentarioActividadOverhaul getComentarioByID(int idComentario)
        {
            return interfazDAO.getComentarioByID(idComentario);
        }
        public bool EliminarArchivo(int idArchivo)
        {
            return interfazDAO.EliminarArchivo(idArchivo);
        }
        public int getArchivosCount(int idActividad)
        {
            return interfazDAO.getArchivosCount(idActividad);
        }
        public bool TerminarOverhaul(int idEvento, DateTime fechaFin, int tipoOverhaul, int estatus)
        {
            return interfazDAO.TerminarOverhaul(idEvento, fechaFin, tipoOverhaul, estatus);
        }
        public string getCCByCalendarioID(int idCalendario)
        {
            return interfazDAO.getCCByCalendarioID(idCalendario);
        }
        public List<tblM_CatMaquina> getEconomicosByObraID(string obra)
        {
            return interfazDAO.getEconomicosByObraID(obra);
        }
        public List<ComponentePlaneacionDTO> CargarGridOHFallaTaller(int idMaquina, bool planeacion)
        {
            return interfazDAO.CargarGridOHFallaTaller(idMaquina, planeacion);
        }
        public bool GuardarOHFallaTaller(int idMaquina, List<ComponentePlaneacionDTO> componentes, DateTime fecha, int calendarioID, int tipo)
        {
            return interfazDAO.GuardarOHFallaTaller(idMaquina, componentes, fecha, calendarioID, tipo);
        }
        public bool UpdateOHFallaTaller(int id, List<ComponentePlaneacionDTO> componentes, int calendarioID)
        {
            return interfazDAO.UpdateOHFallaTaller(id, componentes, calendarioID);
        }
        public tblM_CapPlaneacionOverhaul VerificarOHFallaTaller(int idMaquina, DateTime fecha, int calendarioID)
        {
            return interfazDAO.VerificarOHFallaTaller(idMaquina, fecha, calendarioID);
        }
        public bool GuardarOHParoTaller(int idMaquina, List<ComponentePlaneacionDTO> componentes, DateTime fecha, int calendarioID, string indexCal)
        {
            return interfazDAO.GuardarOHParoTaller(idMaquina, componentes, fecha, calendarioID, indexCal);
        }
        public string CargarStringComentarioActividad(int actividadID, int eventoID, int tipo, DateTime fecha, int numDia, List<tblM_ComentarioActividadOverhaul> _lstComentarioActividadOverhaulDapperDTO = null)
        {
            return interfazDAO.CargarStringComentarioActividad(actividadID, eventoID, tipo, fecha, numDia, _lstComentarioActividadOverhaulDapperDTO);
        }
        public bool CheckTallerRemocion(int componenteID, int maquinaID)
        {
            return interfazDAO.CheckTallerRemocion(componenteID, maquinaID);
        }
        public bool TerminarOverhaul(int index)
        {
            return interfazDAO.TerminarOverhaul(index);
        }
        public List<tblM_CatMaquina> getEconomicosByCalendarioID(int idCalendario)
        {
            return interfazDAO.getEconomicosByCalendarioID(idCalendario);
        }
        public List<ActividadOverhaulDTO> CargarDatosDiagramaGantt(int idModelo, int eventoID)
        {
            return interfazDAO.CargarDatosDiagramaGantt(idModelo, eventoID);
        }

        #region CONSULTAS CON DAPPER
        public List<tblM_CapPlaneacionOverhaul> _lstCapPlaneacionOverhaulDapperDTO(MainContextEnum idEmpresa)
        {
            return interfazDAO._lstCapPlaneacionOverhaulDapperDTO(idEmpresa);
        }

        public List<tblM_ComentarioActividadOverhaul> _lstComentarioActividadOverhaulDapperDTO(MainContextEnum idEmpresa)
        {
            return interfazDAO._lstComentarioActividadOverhaulDapperDTO(idEmpresa);
        }

        public List<tblM_CatActividadOverhaul> _lstCatActividadOverhaulDapperDTO(MainContextEnum idEmpresa)
        {
            return interfazDAO._lstCatActividadOverhaulDapperDTO(idEmpresa);
        }
        #endregion
    }
}
