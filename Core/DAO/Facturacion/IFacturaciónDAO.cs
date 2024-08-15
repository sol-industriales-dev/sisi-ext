using Core.DTO.Facturacion;
using Core.DTO.Contabilidad.Propuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Contabilidad.Facturacion;
using Core.Entity.Facturacion.Estimacion;
using Core.DTO.Contabilidad.FlujoEfectivo;
using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using Core.Enum.Multiempresa;

namespace Core.DAO.Facturacion
{
    public interface IFacturaciónDAO
    {
        #region Facturacción
        List<ComboDTO> getListaCC();
        List<ComboDTO> FillComboCiaSuc();
        List<ComboDTO> FillComboSurcusal(int numcte);
        List<ComboDTO> FillComboCliente();
        List<ComboDTO> FillComboClienteNombre();
        List<ComboDTO> ComboClienteNombre(EnkontrolEnum conector);
        List<ComboDTO> FillComboClienteNombreMoneda(int moneda);
        List<ComboDTO> FillComboRegFiscal();
        List<ComboDTO> FillComboMetodoPago();
        List<ComboDTO> FillComboClaveSat();
        List<ComboDTO> FillComboTm();
        List<ComboDTO> getListaEmpleado();
        List<ComboDTO> FillComboZonas();
        List<InsumosDTO> lstInsumo(string insumo, string descripcion);
        List<object> GetlstInsumoFactura(int pedido);
        List<object> GetlstInsumoRentencion(int pedido);
        InsumosDTO objInsumo(int consecutivo);
        InsumosDTO objRentencion(int insumo);
        object objPedido(int pedido, out int numcte);
        object objRemision(int pedido);
        object objFactura(int pedido, out int cia_surcusal);
        ClienteDTO objCliente(int cliente);
        List<ClienteDTO> getLstCliente();
        CiaParametrosDTO objCdfParametros(int cia_sucursal);
        int UpdateFacura(BigFacturaDTO obj, List<PartidaDTO> lst, List<PartidaDTO> lstRentencion);
        FacturaDTO getNew();
        bool existPedido(int pedido);
        #endregion
        #region Gestión
        List<pedidoDTO> GetTblGestion(DateTime inicio, DateTime fin, string cliente);
        int GetRemisionFromPedido(int pedido);
        int GetFacturaFromPedido(int pedido);
        string GetClienteNombre(int numcte);
        #endregion
        #region Acumulado
        List<MovcltesDTO> GetMovimientoFacturasClientes(BusqConcentradoDTO busq);
        #endregion
        #region Resumen Estimacion
        bool guardarLstEstimacionResumen(List<tblF_EstimacionResumen> lst);
        bool eliminarEstimacion(List<int> lstId);
        bool authResumenEstimacion(DateTime fecha);
        tblF_AuthResumenEstimacion getAuthResumenEstimacion(DateTime fechaInicial, DateTime fechaFinal);
        List<tblF_EstimacionResumen> getlstEstimadoActivo(DateTime fechaInicial, DateTime fechaFinal);
        List<tblF_EstimacionResumen> getlstEstimadoActivo(DateTime fechaInicial, DateTime fechaFinal, List<string> lstCc);
        List<tblF_EstimacionResumen> getAlltEstimadoo(DateTime fechaInicial, DateTime fechaFinal);
        List<tblF_EstimacionResumen> GetAnaliticoClientes(DateTime fecha_corte);
        #endregion
        #region ProyeccionDeCierre
        List<tblC_FED_DetProyeccionCierre> getLstIngresosXCobrarCxCDTO(List<string> lstCC);
        List<tblC_FED_DetProyeccionCierre> getLstRetencionesClientes(/*List<string> lstCC*/BusqProyeccionCierreDTO busq);
        List<tblC_FED_DetProyeccionCierre> getLstAmortizacionClientes(/*List<string> lstCC*/BusqProyeccionCierreDTO busq);
        List<tblC_FED_DetProyeccionCierre> getLstAnticipoClientes(BusqProyeccionCierreDTO busq);        
        List<tblC_FED_DetProyeccionCierre> getLstAnticipoContratistas(BusqProyeccionCierreDTO busq);
        List<tblC_FED_DetProyeccionCierre> getLstRetencionContratistas(BusqProyeccionCierreDTO busq);
        #endregion
    }
}
