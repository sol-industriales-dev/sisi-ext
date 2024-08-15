using Core.DAO.Contabilidad.Poliza;
using Core.DTO.Contabilidad.Poliza;
using Core.Entity.Administrativo.Contabilidad.Poliza;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.Poliza
{
    public class ConversionPolizaService : IConversionPolizaDAO
    {
        #region Atributos
        private IConversionPolizaDAO c_PolizaDAO;
        #endregion
        #region Propiedades
        public IConversionPolizaDAO PolizaDAO
        {
            get { return c_PolizaDAO; }
            set { c_PolizaDAO = value; }
        }
        #endregion
        #region Constructores
        public ConversionPolizaService(IConversionPolizaDAO polizaDAO)
        {
            this.PolizaDAO = polizaDAO;
        }
        #endregion

        public Dictionary<string, object> CargarPolizas(int poliza, int year, int mes)
        {
            return this.PolizaDAO.CargarPolizas(poliza, year, mes);
        }
        public Dictionary<string, object> BuscarCaptura(DateTime fechaConversion)
        {
            return this.PolizaDAO.BuscarCaptura(fechaConversion);
        }
        public Dictionary<string, object> ObtenerPolizaDetalle(int year, int mes, string tp, int poliza, int empresa)
        {
            return this.PolizaDAO.ObtenerPolizaDetalle(year, mes, tp, poliza, empresa);
        }

        public Dictionary<string, object> ObtenerPolizaDetallePeru(int year, int mes, string tp, int poliza, int empresa)
        {
            return this.PolizaDAO.ObtenerPolizaDetallePeru(year, mes, tp, poliza, empresa);
        }
        public Dictionary<string, object> CargarConversionPolizas(int year, int mes, string tp, int poliza, decimal monto)
        {
            return this.PolizaDAO.CargarConversionPolizas(year, mes, tp, poliza, monto);
        }
        public Dictionary<string, object> saveOrUpdatePoliza(List<tblC_SC_ConversionPoliza> listaPolizas, DateTime fechaConversion, bool aplicaFechaPoliza)
        {
            return this.PolizaDAO.saveOrUpdatePoliza(listaPolizas, fechaConversion, aplicaFechaPoliza);
        }
        public Dictionary<string, object> ObtenerPolizaEnkontrol(PolizasDTO poliza, EmpresaEnum empresa)
        {
            return this.PolizaDAO.ObtenerPolizaEnkontrol(poliza, empresa);
        }
    }
}
