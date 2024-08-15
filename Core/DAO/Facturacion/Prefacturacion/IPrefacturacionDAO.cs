using Core.DTO.Facturacion;
using Core.DTO.Facturacion.Prefactura.Insumos;
using Core.Entity.Administrativo.Contabilidad.Facturas;
using Core.Entity.Facturacion.Prefacturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Facturacion.Prefacturacion
{
    public interface IPrefacturacionDAO
    {
        tblF_CapPrefactura savePrefactura(tblF_CapPrefactura obj);
        List<tblF_RepPrefactura> getPrefactura(DateTime inicio, DateTime fin, string cc);
        List<OrdenCompraDTO> getlstOrdenCompra(DateTime inicio, DateTime fin, string cc);
        InsumosDTO getObjInsumo(DateTime fecha, string cc, int insumo);
        List<tblF_CapPrefactura> getPrefactura(int id);
        List<tipoInsumoDTO> getlstTipoInsumo();
        List<tblF_CatImporte> CboConceptoImporte();
        List<tblF_CapImporte> getTotales(int id);
        List<ComboDTO> FillComboUsocfdi();

        #region FACTURAS EK
        Dictionary<string, object> FillComboMetodoPagoSat();
        Dictionary<string,object> FillComboRegimenFiscal();
        Dictionary<string, object> FillComboTM();
        Dictionary<string, object> FillComboTipoFlete();
        Dictionary<string, object> FillComboCondEntrega();
        Dictionary<string, object> FillComboTipoPedido();
        Dictionary<string, object> GetFormaPagoSat(string claveSat);
        Dictionary<string, object> FillComboTipoFactura();
        Dictionary<string, object> FillComboSerie();
        Dictionary<string, object> FIllComboInsumos();

        #endregion

        #region CAT INSUMOS
        Dictionary<string, object> GetInsumosEK(string idInsumoSAT);
        Dictionary<string, object> GetInsumosSAT(tblF_EK_InsumosSAT objFiltro);
        Dictionary<string, object> CrearEditarInsumos(InsumosSATDAO objInsumo, List<string> lstRel);
        Dictionary<string, object> EliminarInsumo(int idInsumo);
        Dictionary<string, object> EliminarRelInsumo(string idInsumoSAT, string idInsumoEK);
        List<InsumosSATDAO> GetAutoCompleteInsumosDesc(string term);
        List<InsumosSATDAO> GetAutoCompleteInsumos(string term);

        #endregion
    }
}
