using Core.DAO.Facturacion;
using Core.DTO.Contabilidad.Facturacion;
using Core.DTO.Contabilidad.FlujoEfectivo;
using Core.DTO.Contabilidad.Propuesta;
using Core.DTO.Facturacion;
using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using Core.Entity.Facturacion.Estimacion;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Facturacion
{
    public class FacturaciónService : IFacturaciónDAO
    {
        private IFacturaciónDAO f_FacturacionDAO;

        public IFacturaciónDAO Factura
        {
            get { return f_FacturacionDAO; }
            set { f_FacturacionDAO = value; }
        }
        #region Facturacción
        public FacturaciónService(IFacturaciónDAO factura)
        {
            this.f_FacturacionDAO = factura;
        }

        public List<InsumosDTO> lstInsumo(string insumo, string descripcion)
        {
            return Factura.lstInsumo(insumo, descripcion);
        }

        public InsumosDTO objInsumo(int consecutivo)
        {
            return Factura.objInsumo(consecutivo);
        }

        public InsumosDTO objRentencion(int insumo)
        {
            return Factura.objRentencion(insumo);
        }

        public object objPedido(int pedido, out int numcte)
        {
            return Factura.objPedido(pedido, out numcte);
        }
        public ClienteDTO objCliente(int cliente)
        {
            return Factura.objCliente(cliente);
        }
        public List<ClienteDTO> getLstCliente()
        {
            return Factura.getLstCliente();
        }
        public object objRemision(int pedido)
        {
            return Factura.objRemision(pedido);
        }
        public object objFactura(int pedido, out int cia_surcusal)
        {
            return Factura.objFactura(pedido, out cia_surcusal);
        }
        public CiaParametrosDTO objCdfParametros(int cia_sucursal)
        {
            return Factura.objCdfParametros(cia_sucursal);
        }
        public int UpdateFacura(BigFacturaDTO obj, List<PartidaDTO> lst, List<PartidaDTO> lstRentencion)
        {
            return Factura.UpdateFacura(obj, lst, lstRentencion);
        }
        public FacturaDTO getNew()
        {
            return Factura.getNew();
        }
        public List<object> GetlstInsumoFactura(int pedido)
        {
            return Factura.GetlstInsumoFactura(pedido);
        }
        public List<object> GetlstInsumoRentencion(int pedido)
        {
            return Factura.GetlstInsumoRentencion(pedido);
        }
        public List<ComboDTO> getListaCC()
        {
            return Factura.getListaCC();
        }
        public List<ComboDTO> getListaEmpleado()
        {
            return Factura.getListaEmpleado();
        }
        public List<ComboDTO> FillComboCiaSuc()
        {
            return Factura.FillComboCiaSuc();
        }
        public List<ComboDTO> FillComboSurcusal(int numcte)
        {
            return Factura.FillComboSurcusal(numcte);
        }
        public List<ComboDTO> FillComboCliente()
        {
            return Factura.FillComboCliente();
        }
        public List<ComboDTO> FillComboClienteNombre()
        {
            return Factura.FillComboClienteNombre();
        }
        public List<ComboDTO> ComboClienteNombre(EnkontrolEnum conector)
        {
            return Factura.ComboClienteNombre(conector);
        }
        public List<ComboDTO> FillComboClienteNombreMoneda(int moneda)
        {
            return Factura.FillComboClienteNombreMoneda(moneda);
        }
        public List<ComboDTO> FillComboRegFiscal()
        {
            return Factura.FillComboRegFiscal();
        }
        public List<ComboDTO> FillComboMetodoPago()
        {
            return Factura.FillComboMetodoPago();
        }
        public List<ComboDTO> FillComboClaveSat()
        {
            return Factura.FillComboClaveSat();
        }
        public List<ComboDTO> FillComboTm()
        {
            return Factura.FillComboTm();
        }
        public bool existPedido(int pedido)
        {
            return Factura.existPedido(pedido);
        }
        public List<ComboDTO> FillComboZonas()
        {
            return Factura.FillComboZonas();
        }
        #endregion
        #region Gestión
        public List<pedidoDTO> GetTblGestion(DateTime inicio, DateTime fin, string cliente)
        {
            return Factura.GetTblGestion(inicio, fin, cliente);
        }
        public int GetRemisionFromPedido(int pedido)
        {
            return Factura.GetRemisionFromPedido(pedido);
        }
        public int GetFacturaFromPedido(int pedido)
        {
            return Factura.GetFacturaFromPedido(pedido);
        }
        public string GetClienteNombre(int numcte)
        {
            return Factura.GetClienteNombre(numcte);
        }
        #endregion
        #region Acumulado
        public List<MovcltesDTO> GetMovimientoFacturasClientes(BusqConcentradoDTO busq)
        {
            return Factura.GetMovimientoFacturasClientes(busq);
        }
        #endregion
        #region Resumen Estimacion
        public bool guardarLstEstimacionResumen(List<tblF_EstimacionResumen> lst)
        {
            return f_FacturacionDAO.guardarLstEstimacionResumen(lst);
        }
        public bool eliminarEstimacion(List<int> lstId)
        {
            return f_FacturacionDAO.eliminarEstimacion(lstId);
        }
        public bool authResumenEstimacion(DateTime fecha)
        {
            return f_FacturacionDAO.authResumenEstimacion(fecha);
        }
        public tblF_AuthResumenEstimacion getAuthResumenEstimacion(DateTime fechaInicial, DateTime fechaFinal)
        {
            return f_FacturacionDAO.getAuthResumenEstimacion(fechaInicial, fechaFinal);
        }
        public List<tblF_EstimacionResumen> getlstEstimadoActivo(DateTime fechaInicial, DateTime fechaFinal)
        {
            return f_FacturacionDAO.getlstEstimadoActivo(fechaInicial, fechaFinal);
        }
        public List<tblF_EstimacionResumen> getlstEstimadoActivo(DateTime fechaInicial, DateTime fechaFinal, List<string> lstCc)
        {
            return f_FacturacionDAO.getlstEstimadoActivo(fechaInicial, fechaFinal, lstCc);
        }
        public List<tblF_EstimacionResumen> getAlltEstimadoo(DateTime fechaInicial, DateTime fechaFinal)
        {
            return f_FacturacionDAO.getAlltEstimadoo(fechaInicial, fechaFinal);
        }
        public List<tblF_EstimacionResumen> GetAnaliticoClientes(DateTime fecha_corte)
        {
            return f_FacturacionDAO.GetAnaliticoClientes(fecha_corte);
        }
        #endregion
        #region ProyeccionesDeCierro
        public List<tblC_FED_DetProyeccionCierre> getLstIngresosXCobrarCxCDTO(List<string> lstCC)
        {
            return f_FacturacionDAO.getLstIngresosXCobrarCxCDTO(lstCC);
        }
        public List<tblC_FED_DetProyeccionCierre> getLstRetencionesClientes(/*List<string> lstCC*/BusqProyeccionCierreDTO busq)
        {
            return f_FacturacionDAO.getLstRetencionesClientes(/*lstCC*/busq);
        }
        public List<tblC_FED_DetProyeccionCierre> getLstAmortizacionClientes(/*List<string> lstCC*/BusqProyeccionCierreDTO busq)
        {
            return f_FacturacionDAO.getLstAmortizacionClientes(/*lstCC*/ busq);
        }
        public List<tblC_FED_DetProyeccionCierre> getLstAnticipoClientes(BusqProyeccionCierreDTO busq)
        {
            return f_FacturacionDAO.getLstAnticipoClientes(busq);
        }
        public List<tblC_FED_DetProyeccionCierre> getLstAnticipoContratistas(BusqProyeccionCierreDTO busq)
        {
            return f_FacturacionDAO.getLstAnticipoContratistas(busq);
        }
        public List<tblC_FED_DetProyeccionCierre> getLstRetencionContratistas(BusqProyeccionCierreDTO busq)
        {
            return f_FacturacionDAO.getLstRetencionContratistas(busq);
        }
        #endregion
    }
}
