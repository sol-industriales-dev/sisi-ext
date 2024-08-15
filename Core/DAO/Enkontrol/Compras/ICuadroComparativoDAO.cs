using Core.DTO;
using Core.DTO.Enkontrol.OrdenCompra;
using Core.DTO.Enkontrol.OrdenCompra.CuadroComparativo;
using Core.DTO.Principal.Generales;
using Core.Entity.Enkontrol.Compras.OrdenCompra.CuadroComparativo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Enkontrol.Compras
{
    public interface ICuadroComparativoDAO
    {
        #region Dashboard
        Dictionary<string, object> ConsultaDashboard(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores);
        #endregion

        Respuesta darVoBoProvNoOptimo(List<CheckProvNoOptimoDTO> idNoOptimo);
        bool GuardarConfiabilidad(CuadroComparativoDTO cuadro);
        List<tblCom_CC_Calificacion> CalificarConfiabilidad(CuadroComparativoDTO cuadro);
        Dictionary<string, object> CalificarConfiabilidad(CuadroComparativoReporteDTO reporte, int numero);
        Dictionary<string, object> FillCboProveedores();
        Dictionary<string, object> FillCboCompradores();
        Dictionary<string, object> GetDetallesProveedoresOptimosVsNoOptimos(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores);
        Dictionary<string, object> GetDetallesTop10ProvNoOptimos(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores);
        Dictionary<string, object> GetDetallesTop10ProvOptimos(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores);
        Dictionary<string, object> GetDetallesCalificaciones(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores);
    }
}
