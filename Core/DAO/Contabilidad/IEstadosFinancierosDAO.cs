using Core.Enum.Contabilidad.EstadoFinanciero;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad
{
    public interface IEstadosFinancierosDAO
    {
        Dictionary<string, object> calcularBalanza(DateTime fechaAnioMes);
        Dictionary<string, object> guardarBalanzaCorte(DateTime fechaAnioMes);
        Dictionary<string, object> FillComboCC();
        Dictionary<string, object> calcularEstadoResultados(List<EmpresaEnum> listaEmpresas, DateTime fechaAnioMes, List<string> listaCC);
        Dictionary<string, object> GetEstadoResultadoDetalle(List<EmpresaEnum> listaEmpresas, DateTime fechaAnioMes, List<string> listaCC, int tipoBusqueda);
        Dictionary<string, object> EliminarBalanza(DateTime fechaCorte);

        #region Balance
        Dictionary<string, object> CalcularBalanceGeneral(List<EmpresaEnum> listaEmpresas, DateTime fechaAnioMes, List<string> listaCC, TipoBalanceEnum tipoBalance);
        Dictionary<string, object> GetBalanceDetalle(List<EmpresaEnum> listaEmpresas, DateTime fechaMesCorte, List<string> listaCC, TipoDetalleEnum tipoDetalle, int tipoTablaGeneral);
        #endregion
    }
}
