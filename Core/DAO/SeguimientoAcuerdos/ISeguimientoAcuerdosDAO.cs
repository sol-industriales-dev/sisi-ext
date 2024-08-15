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

namespace Core.DAO.SeguimientoAcuerdos
{
    public interface ISeguimientoAcuerdosDAO
    {
        int guardarMinuta(tblSA_Minuta obj, bool nuevaVersion);
        void guardarDescripcion(tblSA_Minuta obj, bool nuevaVersion);
        int guardarActividad(tblSA_Actividades obj);
        tblSA_ComentariosDTO guardarComentario(tblSA_Comentarios obj, HttpPostedFileBase file);
        int guardarParticipante(tblSA_Participante obj);
        void eliminarParticipante(tblSA_Participante obj);
        tblSA_MinutaDTO getMinuta(int id, int empresa);
        tblSA_MinutaDTO getMinutaForVersion(int id);
        
        List<tblSA_MinutaDTO> getAllMinutas(int user);
        List<tblSA_ActividadesDTO> getAllActividades(int user);
        void avanceActividad(int id, int columna);
        List<tblSA_ParticipanteDTO> getParticipantes(int minuta);
        bool validarPromoverAvanceActividad(int actividadID, int usuarioID, bool desdeMinuta = false);
        bool esResponsableACtividad(int id, int u);
        void promoverAvanceActividad(tblSA_PromoverActividad obj);
        void promocionAvanceActividad(int actividadID, int accion, string observacion);
        List<PromoverDTO> getAllActividadesAPromover(int user);
        List<minutaRptDTO> getMinutaPrint(int minuta, int empresa);
        List<listaAsistenciaMinutaRptDTO> getListaAsistenciaMinutaPrint(int minuta, int empresa);

        int guardarResponsable(tblSA_Responsables obj);
        void guardarResponsables(List<tblSA_Responsables> obj);
        void eliminarResponsable(tblSA_Responsables obj);
        List<tblSA_ResponsablesDTO> getResponsablesPorActividad(int actividad);

        int guardarInteresado(tblSA_Interesados obj);
        void guardarInteresados(List<tblSA_Interesados> obj);
        void eliminarInteresado(tblSA_Interesados obj);
        List<tblSA_InteresadosDTO> getInteresadosPorActividad(int actividad);
        tblSA_Comentarios getComentarioByID(int id);
        List<string> enviarCorreos(int minutaID, List<int> usuarios, List<Byte[]> downloadPDF);
        List<tblP_Usuario> FillComboParticipantes(int minutaID);
        List<tblSA_Minuta> getMinutas(int user);
        List<tblP_Usuario> FillComboUsuarios(int minutaID);
        void setDataCustom();
        int getActivitiesCount(int id);
        List<ActividadesTipoDTO> getReporteActividades(List<int> departamentoID, DateTime fechaInicio, DateTime fechaFin,int estatus);
        List<Minutas_ActividadesPendientesDTO> getReporteMinutasPendientes(List<int> departamentoID, DateTime fechaInicio, DateTime fechaFin);
        List<EstadisticoMinutasDTO> getReporteEstadisticoMinutas(List<int> departamentoID, DateTime fechaInicio, DateTime fechaFin);
        List<BitacoraSeguimientoAcuerdosDTO> getBitacoraMinutas(List<int> departamentoID, DateTime fechaInicio, DateTime fechaFin);
        List<tblP_Departamento> getDepartamentos();
    }
}
