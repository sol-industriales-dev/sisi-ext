using Core.DAO.Contabilidad;
using Core.Enum.Contabilidad.EstadoFinanciero;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad
{
    public class EstadosFinancierosService : IEstadosFinancierosDAO
    {
        public IEstadosFinancierosDAO estadosFinancierosDAO { get; set; }

        public EstadosFinancierosService(IEstadosFinancierosDAO estadosFinancierosDAO)
        {
            this.estadosFinancierosDAO = estadosFinancierosDAO;
        }

        public Dictionary<string, object> calcularBalanza(DateTime fechaAnioMes)
        {
            return estadosFinancierosDAO.calcularBalanza(fechaAnioMes);
        }

        public Dictionary<string, object> guardarBalanzaCorte(DateTime fechaAnioMes)
        {
            return estadosFinancierosDAO.guardarBalanzaCorte(fechaAnioMes);
        }

        public Dictionary<string, object> FillComboCC()
        {
            return estadosFinancierosDAO.FillComboCC();
        }

        public Dictionary<string, object> calcularEstadoResultados(List<EmpresaEnum> listaEmpresas, DateTime fechaAnioMes, List<string> listaCC)
        {
            return estadosFinancierosDAO.calcularEstadoResultados(listaEmpresas, fechaAnioMes, listaCC);
        }

        public Dictionary<string, object> GetEstadoResultadoDetalle(List<EmpresaEnum> listaEmpresas, DateTime fechaAnioMes, List<string> listaCC, int tipoBusqueda)
        {
            return estadosFinancierosDAO.GetEstadoResultadoDetalle(listaEmpresas, fechaAnioMes, listaCC, tipoBusqueda);
        }

        public Dictionary<string, object> EliminarBalanza(DateTime fechaCorte)
        {
            return estadosFinancierosDAO.EliminarBalanza(fechaCorte);
        }

        #region Balance
        public Dictionary<string, object> CalcularBalanceGeneral(List<EmpresaEnum> listaEmpresas, DateTime fechaAnioMes, List<string> listaCC, TipoBalanceEnum tipoBalance)
        {
            return estadosFinancierosDAO.CalcularBalanceGeneral(listaEmpresas, fechaAnioMes, listaCC, tipoBalance);
        }

        public Dictionary<string, object> GetBalanceDetalle(List<EmpresaEnum> listaEmpresas, DateTime fechaMesCorte, List<string> listaCC, TipoDetalleEnum tipoDetalle, int tipoTablaGeneral)
        {
            return estadosFinancierosDAO.GetBalanceDetalle(listaEmpresas, fechaMesCorte, listaCC, tipoDetalle, tipoTablaGeneral);
        }
        #endregion
    }
}
