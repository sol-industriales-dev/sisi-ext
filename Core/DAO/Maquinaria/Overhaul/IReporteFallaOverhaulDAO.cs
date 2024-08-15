using Core.DTO.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.DTO.Principal.Generales;
using System.IO;
using Core.DTO.Maquinaria.Mantenimiento.Correctivo.ReporteFalla;

namespace Core.DAO.Maquinaria.Overhaul
{
    public interface IReporteFallaOverhaulDAO
    {
        bool Guardar(tblM_ReporteFalla obj);
        bool Guardar(tblM_ReporteFalla_Componente obj);
        bool Guardar(tblM_ReporteFalla_Reparacion obj);
        List<tblM_ReporteFalla> getReporteFallas(FiltrosReporteFallaDTO obj);
        void Eliminar(int id);
        bool EliminarReparacionDesdeComponente(tblM_ReporteFalla_Componente obj);
        bool EliminarComponenteDesdeReparacion(tblM_ReporteFalla_Reparacion obj);
        List<ReporteFallaDTO> cargarReportes(int estatus, /*string descripcionComponente,*/ int cc);
        tblM_ReporteFalla aprobarReporte(int idReporte);
        tblM_ReporteFalla getReporte(int idReporte);
        tblM_ReporteFalla_Componente getRptComoponente(int idReporte);
        tblM_ReporteFalla_Reparacion getRptReparacion(int idReporte);
        List<tblM_ReporteFalla_Archivo> getLstImagenes(int idReporte);
        int getIdComponenteFromIdReporteFalla(int idReporte);
        //int getConjuntoComponente(int idSubconjunto);
        List<tblM_trackComponentes> fillCboComponentes(int idModelo, int idSubconjunto);
        tblM_CatSubConjunto getSubconjuntoComponente(int idComponente);
        tblM_AsignacionEquipos getProcedenciaMaquina(int idMaquina);
        string getCCNameByAC(string areaCuenta);
        tblM_CatComponente getComponente(int idComponente);
        tblM_trackComponentes getTrackingActualComponente(int idComponente);
        string getCCNameByID(int idCC);
        List<ComboDTO> fillVistoBueno(string CentroCostos);
        List<ComboDTO> fillCboRevisa(string CentroCostos);
        tblM_ReporteFalla getReporteByID(int idReporte);

        Dictionary<string, object> GetArchivosReporteFalla(int _idReporteFalla);

        Tuple<Stream, string> DescargarArchivoReporteFalla(int _idArchivo);
    }
}
