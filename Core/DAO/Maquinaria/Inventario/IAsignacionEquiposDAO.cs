using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Inventario.Comparativos;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Enum.Principal.Bitacoras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface IAsignacionEquiposDAO
    {

        List<tblM_AsignacionEquipos> getAsignacionesByEconomicos(int economicoID);
        void SaveOrUpdate(List<tblM_AsignacionEquipos> obj);
        void SaveOrUpdate(tblM_AsignacionEquipos obj);

        tblM_AsignacionEquipos GetAsiganacionBySolicitud(int obj);
        List<tblM_CatMaquina> getMaquinasAsignadas(List<int> idEconomicos);
        List<int> getMaquinariaAsiganadasD(DateTime FechaInico);

        List<tblM_AsignacionEquipos> getAsignacionActivas(List<string> ccs);

        tblM_AsignacionEquipos GetAsiganacionById(int obj);

        List<tblM_AsignacionEquipos> getAsignacionesCompras();
        tblM_AsignacionEquipos getAsignacionesCompras(int id);

        tblM_AsignacionEquipos getEconomicoAsignado(int idEconomico);

        bool GetAsginadosRecibidos(int obj);

        List<tblM_AsignacionEquipos> getAsignacionesByCC(string centroCostos);
        List<tblM_AsignacionEquipos> getAsignacionesByID(int id);

        List<tblM_AsignacionEquipos> getAsignacionesByIDs(int id);
        void delete(tblM_AsignacionEquipos Entidad);
        List<tblM_AsignacionEquipos> GetListaAsignaciones();
        List<tblM_AsignacionEquipos> getAsignacionesbySDet(int id);
        List<tblM_AsignacionEquipos> GetAsignacionControles(int Filtro, string CentroCostos);

        List<HistoricoMaquinariaDTO> GetHistorialMaquina(string EconomicoID);
        List<HistoricoMaquinariaDTO> getHistorialEconomicos(string EconomicoID);
        List<CuadroAutorizacionDTO> CargarSolicitudes(int estado, string obra, DateTime fechaInicio, DateTime fechaFin);
        void logErrores(int sistema, int modulo, string controlador, string action, Exception exception, AccionEnum tipo, long registroId, object objeto);
    }        


}
