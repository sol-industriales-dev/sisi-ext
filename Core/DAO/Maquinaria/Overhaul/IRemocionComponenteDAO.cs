using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Maquinaria.Overhaul;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Catalogo;

using Core.Entity.Maquinaria.Overhaul;

using Core.Entity.RecursosHumanos.Catalogo;

namespace Core.DAO.Maquinaria.Overhaul
{
    public interface IRemocionComponenteDAO
    {
        void Guardar(tblM_ReporteRemocionComponente obj, int tipo);
        decimal CalcularHrsCicloComponente(int idComponente, DateTime fecha);
        List<horometrosComponentesDTO> GetHrsCicloActualComponentes(List<int> componentesIDs);
        decimal GetHrsCicloActualComponente(int componenteID, DateTime fechaActual, int id = 0, bool esHistorial = false);
        List<horometrosComponentesDTO> GetHrsAcumuladasComponentes(List<int> componentesIDs);
        decimal GetHrsAcumuladasComponente(int componenteID, DateTime fechaActual, bool esHistorial);
        bool GuardarReporteActualizacion(tblM_ReporteRemocionComponente obj);
        RemocionDTO cargarDatosRemocionComponente(int idComponente);
        List<ComboDTO> cargarCboComponenteInstalado(int idModelo, int idSubconjunto);
        List<ComboDTO> cargarCboPersonal(string cc);
        List<ComboDTO> getCatEmpleados();
        string getPuestoDescripcion(int idPuesto);
        List<ComboDTO> getMaquinasByModelo(int idModelo, int estatus);
        string getDescripcionCC(string cc);
        List<tblM_ReporteRemocionComponente> cargarReportes(int estatus, string descripcionComponente, string noEconomico, int motivoRemocion, DateTime? fechaInicio, DateTime? fechaFinal, List<string> cc, List<int> modelos, string noComponente);
        void Eliminar(int idComponente);
        tblM_ReporteRemocionComponente getReporteRemocionByID(int idReporte);        
        string getCC(string areaCuenta);
        bool verificarReporte(int idReporte);
        bool enviarReporte(int idReporte, int trackID);
        bool aprobarReporte(int idReporte);
        decimal GetHorasMaquina(string noEconomico);
        bool UpdateReporteDesecho(int idReporte);
        decimal GetHorasMaquinaPorFecha(string noEconomico, DateTime fecha);
        DateTime fechaInstalacion(int idComponente);
        bool EliminarReporteRemocionByID(int idReporte);
        decimal GetHorasMaquinaIDPorFecha(int idMaquina, DateTime fecha);
        List<ComboDTO> getEmpleadosRemocion(string term);

        bool EliminarArchivoTrackComponentes(int idArchivo, int idComponente);
    }
}

