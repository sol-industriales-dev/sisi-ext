using Core.DAO.Contabilidad.Propuesta;
using Core.DTO.Contabilidad.Propuesta;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Administracion;
using Core.DTO.COMPRAS;
using Core.DTO.Contabilidad.Propuesta.Validacion;

namespace Core.Service.Contabilidad.Propuesta
{
    public class PropuestaProgramacionServices : IPropuestaProgramacionDAO
    {
        #region Atributos
        public IPropuestaProgramacionDAO c_propDAO { get; set; }
        #endregion
        #region Propiedades
        public IPropuestaProgramacionDAO PropDAO 
        {
            get { return c_propDAO; }
            set { c_propDAO = value; } 
        }
        #endregion
        #region Constructores
        public PropuestaProgramacionServices(IPropuestaProgramacionDAO propDAO)
        {
            PropDAO = propDAO;
        }
        #endregion
        public Dictionary<string, object> guardarGastosProv(List<tblC_sp_gastos_prov> lst, bool manual)
        {
            return PropDAO.guardarGastosProv(lst,manual);
        }
        public bool guardarGastosProv_ActivoFijo(List<tblC_sp_gastos_prov> lst, bool manual)
        {
            return PropDAO.guardarGastosProv_ActivoFijo(lst, manual);
        }
        public List<tblC_sp_gastos_prov> getLstGastosProv(BusqConcentradoDTO busq)
        {
            return PropDAO.getLstGastosProv(busq);
        }
        public List<tblC_sp_gastos_prov> getLstGastosProv(BusqPropEkDTO busq)
        {
            return PropDAO.getLstGastosProv(busq);
        }
        public List<tblC_sp_gastos_prov> getLstFacturasProv(BusqPropEkDTO busq)
        {
            return PropDAO.getLstFacturasProv(busq);
        }
        public List<tblC_sp_gastos_prov_activofijo> getLstFacturasProv_activofijo(BusqPropEkDTO busq)
        {
            return PropDAO.getLstFacturasProv_activofijo(busq);
        }
        public List<tblC_sp_gastos_prov> getLstFacturasSaldosMenores(BusqPropEkDTO busq)
        { 
            return PropDAO.getLstFacturasSaldosMenores(busq);
        }
        public bool GuardarMontosProgrPagos(List<MontoPropPagoDTO> lst, DateTime pago)
        {
            return PropDAO.GuardarMontosProgrPagos(lst ,pago);
        }
        public int SaldosMenores_GenerarPolizas(DateTime pago, string moneda, decimal total, bool tipo)
        {
            return PropDAO.SaldosMenores_GenerarPolizas(pago,moneda,total,tipo);
        }
        public string SaldosMenores_GenerarPolizas_det(List<MontoPropPagoDTO> lst, DateTime pago, int polizaID, bool tipo)
        {
            return PropDAO.SaldosMenores_GenerarPolizas_det(lst, pago, polizaID,tipo);
        }
        public List<string> getLimitNoProveedores()
        {
            return PropDAO.getLimitNoProveedores();
        }
        public List<ProgrPagoDTO> GetListProgrPagos(string min, string max, List<string> cc, DateTime fecha)
        {
            return PropDAO.GetListProgrPagos(min, max, cc, fecha);
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> getComboProveedores()
        {
            return PropDAO.getComboProveedores();
        }
        public List<sp_genera_movprovDTO> getLstGenMovProv(BusqGenMovProvDTO busq)
        {
            return PropDAO.getLstGenMovProv(busq);
        }

        public List<tblC_sp_gastos_prov> getReportes(DateTime fechaInicio, DateTime fechaFin, bool autorizada)
        {
            return PropDAO.getReportes(fechaInicio,fechaFin,autorizada);
        }

        #region Facturas duplicadas
        public List<Core.DTO.Enkontrol.Tablas.Poliza.sp_movprovDTO> getFacturasDuplicadas()
        {
            return PropDAO.getFacturasDuplicadas();
        }
        #endregion

        #region Peru
        public List<FacturasProvDTO> getFacturasPendientesPeru(List<FacturasProvDTO> facturasPeru)
        {
            return PropDAO.getFacturasPendientesPeru(facturasPeru);
        }
        public List<FacturasProvDTO> getLstFacturasProvPeru(BusqPropEkDTO busq)
        {
            return PropDAO.getLstFacturasProvPeru(busq);
        }
        #endregion

        #region Autorización Facturas
        public List<ProgrPagoDTO> GetFacturasPendientes(string min, string max, List<string> cc, int tipo)
        {
            return PropDAO.GetFacturasPendientes(min, max, cc, tipo);
        }

        public string GetRutaRequerimiento(string cc, int numero)
        {
            return PropDAO.GetRutaRequerimiento(cc, numero);
        }

        public Dictionary<string, object> ValidarFactura(List<FiltroValidacionDTO> lstFiltro)
        {
            return PropDAO.ValidarFactura(lstFiltro);
        }

        public Dictionary<string, object> AutorizarFactura(List<FiltroValidacionDTO> lstFiltro)
        {
            return PropDAO.AutorizarFactura(lstFiltro);
        }

        public Dictionary<string, object> GetDescripcionTM(int tm)
        {
            return PropDAO.GetDescripcionTM(tm);
        }
        #endregion
    }
}
