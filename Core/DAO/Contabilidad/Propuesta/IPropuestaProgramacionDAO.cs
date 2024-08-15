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

namespace Core.DAO.Contabilidad.Propuesta
{
    public interface IPropuestaProgramacionDAO
    {
        Dictionary<string, object> guardarGastosProv(List<tblC_sp_gastos_prov> lst, bool manual);
        bool guardarGastosProv_ActivoFijo(List<tblC_sp_gastos_prov> lst, bool manual);
        List<tblC_sp_gastos_prov> getLstGastosProv(BusqConcentradoDTO busq);
        List<tblC_sp_gastos_prov> getLstGastosProv(BusqPropEkDTO busq);
        List<tblC_sp_gastos_prov> getLstFacturasProv(BusqPropEkDTO busq);
        List<tblC_sp_gastos_prov_activofijo> getLstFacturasProv_activofijo(BusqPropEkDTO busq);
        List<tblC_sp_gastos_prov> getLstFacturasSaldosMenores(BusqPropEkDTO busq);
        bool GuardarMontosProgrPagos(List<MontoPropPagoDTO> lst, DateTime pago);
        int SaldosMenores_GenerarPolizas(DateTime pago, string moneda, decimal total, bool tipo);
        string SaldosMenores_GenerarPolizas_det(List<MontoPropPagoDTO> lst, DateTime pago, int polizaID, bool tipo);
        List<string> getLimitNoProveedores();
        List<sp_genera_movprovDTO> getLstGenMovProv(BusqGenMovProvDTO busq);
        List<ProgrPagoDTO> GetListProgrPagos(string min, string max, List<string> cc, DateTime fecha);
        List<Core.DTO.Principal.Generales.ComboDTO> getComboProveedores();
        List<tblC_sp_gastos_prov> getReportes(DateTime fechaInicio, DateTime fechaFin, bool autorizada);

        #region Facturas duplicadas
        List<Core.DTO.Enkontrol.Tablas.Poliza.sp_movprovDTO> getFacturasDuplicadas();
        #endregion

        #region Peru
        List<FacturasProvDTO> getFacturasPendientesPeru(List<FacturasProvDTO> facturasPeru);
        List<FacturasProvDTO> getLstFacturasProvPeru(BusqPropEkDTO busq);
        #endregion
        
        #region Autorización facturas
        List<ProgrPagoDTO> GetFacturasPendientes(string min, string max, List<string> cc, int tipo);
        string GetRutaRequerimiento(string cc, int numero);
        Dictionary<string, object> ValidarFactura(List<FiltroValidacionDTO> lstFiltro);
        Dictionary<string, object> AutorizarFactura(List<FiltroValidacionDTO> lstFiltro);
        Dictionary<string, object> GetDescripcionTM(int tm);

        #endregion
    }
}
