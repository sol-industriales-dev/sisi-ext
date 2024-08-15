using Core.DTO.Contabilidad.ControlPresupuestal;
using Core.DTO.Contabilidad.Presupuesto;
using Core.DTO.Principal.Generales;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Presupuesto
{
    public interface IControlPresupuestalDAO
    {
        Dictionary<string, object> FillCboModeloEquipo(int? idGrupo);
        Dictionary<string, object> FillCboModeloEquipoMultiple(List<int> listaGrupos);
        Dictionary<string, object> getComboEconomicos(string AreaCuenta, int? modelo);
        Dictionary<string, object> getComboEconomicosMultiple(string AreaCuenta, List<int> listaModelos);
        Dictionary<string, object> cargarControlPresupuestal(FiltrosControlPresupuestalDTO filtros);
        Dictionary<string, object> cargarControlPresupuestal_Solo_Grafica(FiltrosControlPresupuestalDTO filtros);
        Dictionary<string, object> cargarDetalleAgrupado(FiltrosControlPresupuestalDTO filtros, string economico, int concepto);
        Dictionary<string, object> CargarDetallePresupuestal(FiltrosControlPresupuestalDTO filtros, string economico, int concepto);
        Dictionary<string, object> cargarDetalleMovimientos(FiltrosControlPresupuestalDTO filtros, string economico, int concepto, int cta, int scta, int sscta);
        Dictionary<string, object> cargarDashboard(FiltrosDashboardDTO filtros);
        Dictionary<string, object> getComboDivision();
        Dictionary<string, object> getComboConcepto();
        List<ComboDTO> obtenerGruposMaquinaria(int idTipo);
        dynamic getCboCC(List<int> divisionesIDs);
    }
}
