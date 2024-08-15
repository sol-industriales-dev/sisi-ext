using Core.DAO.SeguimientoAcuerdos;
using Core.DTO.SeguimientoAcuerdos;
using Core.Entity.Principal.Usuarios;
using Core.Entity.SeguimientoAcuerdos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.SeguimientoAcuerdos
{
    public class SeguimientoAcuerdosService : ISeguimientoAcuerdosDAO
    {
        #region Atributos
        private ISeguimientoAcuerdosDAO m_seguimientoAcuerdosDAO;
        #endregion

        #region Propiedades
        public ISeguimientoAcuerdosDAO SeguimientoAcuerdosDAO
        {
            get { return m_seguimientoAcuerdosDAO; }
            set { m_seguimientoAcuerdosDAO = value; }
        }
        #endregion

        #region Constructores
        public SeguimientoAcuerdosService(ISeguimientoAcuerdosDAO seguimientoAcuerdosDAO)
        {
            this.SeguimientoAcuerdosDAO = seguimientoAcuerdosDAO;
        }
        #endregion
        public int guardarMinuta(tblSA_Minuta obj, bool nuevaVersion)
        {
            return SeguimientoAcuerdosDAO.guardarMinuta(obj, nuevaVersion);
        }
        public void guardarDescripcion(tblSA_Minuta obj, bool nuevaVersion)
        {
            SeguimientoAcuerdosDAO.guardarMinuta(obj, nuevaVersion);
        }
        public int guardarActividad(tblSA_Actividades obj)
        {
            return SeguimientoAcuerdosDAO.guardarActividad(obj);
        }
        public tblSA_ComentariosDTO guardarComentario(tblSA_Comentarios obj, HttpPostedFileBase file)
        {
            return SeguimientoAcuerdosDAO.guardarComentario(obj, file);
        }
        public int guardarParticipante(tblSA_Participante obj)
        {
            return SeguimientoAcuerdosDAO.guardarParticipante(obj);
        }
        public void eliminarParticipante(tblSA_Participante obj)
        {
            SeguimientoAcuerdosDAO.eliminarParticipante(obj);
        }
        public tblSA_MinutaDTO getMinuta(int id, int empresa)
        {
            return SeguimientoAcuerdosDAO.getMinuta(id, empresa);
        }
        public tblSA_MinutaDTO getMinutaForVersion(int id)
        {
            return SeguimientoAcuerdosDAO.getMinutaForVersion(id);
        }

        public List<tblSA_MinutaDTO> getAllMinutas(int user)
        {
            return SeguimientoAcuerdosDAO.getAllMinutas(user);
        }

        public List<tblSA_ActividadesDTO> getAllActividades(int user)
        {
            return SeguimientoAcuerdosDAO.getAllActividades(user);
        }
        public void avanceActividad(int id, int columna)
        {
            SeguimientoAcuerdosDAO.avanceActividad(id, columna);
        }
        public List<tblSA_ParticipanteDTO> getParticipantes(int minuta)
        {
            return SeguimientoAcuerdosDAO.getParticipantes(minuta);
        }
        public bool validarPromoverAvanceActividad(int actividadID, int usuarioID, bool desdeMinuta = false)
        {
            return SeguimientoAcuerdosDAO.validarPromoverAvanceActividad(actividadID, usuarioID, desdeMinuta);
        }
        public bool esResponsableACtividad(int id, int u)
        {
            return SeguimientoAcuerdosDAO.esResponsableACtividad(id, u);
        }
        public void promoverAvanceActividad(tblSA_PromoverActividad obj)
        {
            SeguimientoAcuerdosDAO.promoverAvanceActividad(obj);
        }
        public void promocionAvanceActividad(int actividadID, int accion, string observacion)
        {
            SeguimientoAcuerdosDAO.promocionAvanceActividad(actividadID, accion, observacion);
        }
        public List<PromoverDTO> getAllActividadesAPromover(int user)
        {
            return SeguimientoAcuerdosDAO.getAllActividadesAPromover(user);
        }
        public List<minutaRptDTO> getMinutaPrint(int minuta, int empresa)
        {
            return SeguimientoAcuerdosDAO.getMinutaPrint(minuta, empresa);
        }
        public List<listaAsistenciaMinutaRptDTO> getListaAsistenciaMinutaPrint(int minuta, int empresa)
        {
            return SeguimientoAcuerdosDAO.getListaAsistenciaMinutaPrint(minuta, empresa);
        }
        public int guardarResponsable(tblSA_Responsables obj)
        {
            return SeguimientoAcuerdosDAO.guardarResponsable(obj);
        }
        public void eliminarResponsable(tblSA_Responsables obj)
        {
            SeguimientoAcuerdosDAO.eliminarResponsable(obj);
        }
        public List<tblSA_ResponsablesDTO> getResponsablesPorActividad(int actividad)
        {
            return SeguimientoAcuerdosDAO.getResponsablesPorActividad(actividad);
        }
        public void guardarResponsables(List<tblSA_Responsables> obj)
        {
            SeguimientoAcuerdosDAO.guardarResponsables(obj);
        }
        public int guardarInteresado(tblSA_Interesados obj)
        {
            return SeguimientoAcuerdosDAO.guardarInteresado(obj);
        }
        public void eliminarInteresado(tblSA_Interesados obj)
        {
            SeguimientoAcuerdosDAO.eliminarInteresado(obj);
        }
        public List<tblSA_InteresadosDTO> getInteresadosPorActividad(int actividad)
        {
            return SeguimientoAcuerdosDAO.getInteresadosPorActividad(actividad);
        }
        public void guardarInteresados(List<tblSA_Interesados> obj)
        {
            SeguimientoAcuerdosDAO.guardarInteresados(obj);
        }
        public tblSA_Comentarios getComentarioByID(int id)
        {
            return SeguimientoAcuerdosDAO.getComentarioByID(id);
        }
        public List<string> enviarCorreos(int minutaID, List<int> usuarios, List<Byte[]> downloadPDF)
        {
            return SeguimientoAcuerdosDAO.enviarCorreos(minutaID, usuarios, downloadPDF);
        }
        public List<tblP_Usuario> FillComboParticipantes(int minutaID)
        {
            return SeguimientoAcuerdosDAO.FillComboParticipantes(minutaID);
        }
        public List<tblSA_Minuta> getMinutas(int user)
        {
            return SeguimientoAcuerdosDAO.getMinutas(user);
        }
        public List<tblP_Usuario> FillComboUsuarios(int minutaID)
        {
            return SeguimientoAcuerdosDAO.FillComboUsuarios(minutaID);
        }

        public void setDataCustom()
        {
            SeguimientoAcuerdosDAO.setDataCustom();
        }
        public int getActivitiesCount(int id)
        {
            return SeguimientoAcuerdosDAO.getActivitiesCount(id);
        }
        public List<ActividadesTipoDTO> getReporteActividades(List<int> departamentoID, DateTime fechaInicio, DateTime fechaFin, int estatus)
        {
            return SeguimientoAcuerdosDAO.getReporteActividades(departamentoID, fechaInicio, fechaFin, estatus);
        }
        public List<Minutas_ActividadesPendientesDTO> getReporteMinutasPendientes(List<int> departamentoID, DateTime fechaInicio, DateTime fechaFin)
        {
            return SeguimientoAcuerdosDAO.getReporteMinutasPendientes(departamentoID, fechaInicio, fechaFin);
        }
        public List<EstadisticoMinutasDTO> getReporteEstadisticoMinutas(List<int> departamentoID, DateTime fechaInicio, DateTime fechaFin)
        {
            return SeguimientoAcuerdosDAO.getReporteEstadisticoMinutas(departamentoID, fechaInicio, fechaFin);
        }
        public List<BitacoraSeguimientoAcuerdosDTO> getBitacoraMinutas(List<int> departamentoID, DateTime fechaInicio, DateTime fechaFin)
        {
            return SeguimientoAcuerdosDAO.getBitacoraMinutas(departamentoID, fechaInicio, fechaFin);
        }
        public List<tblP_Departamento> getDepartamentos()
        {
            return SeguimientoAcuerdosDAO.getDepartamentos();
        }
    }
}
