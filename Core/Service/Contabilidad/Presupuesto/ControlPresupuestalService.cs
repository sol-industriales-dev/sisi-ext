using Core.DAO.Contabilidad.Presupuesto;
using Core.DTO.Contabilidad.ControlPresupuestal;
using Core.DTO.Contabilidad.Presupuesto;
using Core.DTO.Principal.Generales;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.Presupuesto
{
    public class ControlPresupuestalService : IControlPresupuestalDAO
    {
        public IControlPresupuestalDAO controlPresupuestalDAO { get; set; }
        public IControlPresupuestalDAO controlPresupuestal
        {
            get { return controlPresupuestalDAO; }
            set { controlPresupuestalDAO = value; }
        }
        public ControlPresupuestalService(IControlPresupuestalDAO ControlPresupuestal)
        {
            controlPresupuestal = ControlPresupuestal;
        }
        public Dictionary<string, object> FillCboModeloEquipo(int? idGrupo)
        {
            return controlPresupuestal.FillCboModeloEquipo(idGrupo);
        }
        public Dictionary<string, object> FillCboModeloEquipoMultiple(List<int> listaGrupos)
        {
            return controlPresupuestal.FillCboModeloEquipoMultiple(listaGrupos);
        }
        public Dictionary<string, object> getComboEconomicos(string AreaCuenta, int? modelo)
        {
            return controlPresupuestal.getComboEconomicos(AreaCuenta,modelo);
        }
        public Dictionary<string, object> getComboEconomicosMultiple(string AreaCuenta, List<int> listaModelos)
        {
            return controlPresupuestal.getComboEconomicosMultiple(AreaCuenta, listaModelos);
        }
        public Dictionary<string, object> CargarDetallePresupuestal(FiltrosControlPresupuestalDTO filtros, string economico, int concepto)
        {
            return controlPresupuestal.CargarDetallePresupuestal(filtros,economico,concepto);
        }
        public Dictionary<string, object> cargarControlPresupuestal(FiltrosControlPresupuestalDTO filtros)
        {
            return controlPresupuestal.cargarControlPresupuestal(filtros);
        }
        public Dictionary<string, object> cargarControlPresupuestal_Solo_Grafica(FiltrosControlPresupuestalDTO filtros)
        {
            return controlPresupuestal.cargarControlPresupuestal_Solo_Grafica(filtros);
        }
        public Dictionary<string, object> cargarDetalleAgrupado(FiltrosControlPresupuestalDTO filtros, string economico, int concepto)
        {
            return controlPresupuestal.cargarDetalleAgrupado(filtros, economico, concepto);
        }

        public Dictionary<string, object> cargarDetalleMovimientos(FiltrosControlPresupuestalDTO filtros, string economico, int concepto, int cta, int scta, int sscta)
        {
            return controlPresupuestal.cargarDetalleMovimientos(filtros, economico, concepto, cta, scta, sscta);
        }

        public Dictionary<string, object> cargarDashboard(FiltrosDashboardDTO filtros)
        {
            return controlPresupuestal.cargarDashboard(filtros);
        }

        public Dictionary<string, object> getComboDivision()
        {
            return controlPresupuestal.getComboDivision();
        }

        public Dictionary<string, object> getComboConcepto()
        {
            return controlPresupuestal.getComboConcepto();
        }
        public List<ComboDTO> obtenerGruposMaquinaria(int idTipo)
        {
            return controlPresupuestal.obtenerGruposMaquinaria(idTipo);
        }
        public dynamic getCboCC(List<int> divisionesIDs)
        {
            return controlPresupuestal.getCboCC(divisionesIDs);
        }
    }
}
