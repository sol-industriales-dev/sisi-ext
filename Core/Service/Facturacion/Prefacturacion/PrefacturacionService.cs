using Core.DAO.Facturacion.Prefacturacion;
using Core.DTO.Facturacion;
using Core.DTO.Facturacion.Prefactura.Insumos;
using Core.Entity.Administrativo.Contabilidad.Facturas;
using Core.Entity.Facturacion.Prefacturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Facturacion.Prefacturacion
{
    public class PrefacturacionService : IPrefacturacionDAO
    {
        private IPrefacturacionDAO f_IPrefacturacionDAO;

        public IPrefacturacionDAO Prefactura
        {
            get { return f_IPrefacturacionDAO; }
            set { f_IPrefacturacionDAO = value; }
        }

        public PrefacturacionService(IPrefacturacionDAO prefactura)
        {
            this.f_IPrefacturacionDAO = prefactura;
        }

        public tblF_CapPrefactura savePrefactura(tblF_CapPrefactura obj)
        {
            return Prefactura.savePrefactura(obj);
        }

        public List<tblF_RepPrefactura> getPrefactura(DateTime inicio, DateTime fin, string cc)
        {
            return Prefactura.getPrefactura(inicio, fin, cc);
        }

        public List<tblF_CapPrefactura> getPrefactura(int id)
        {
            return Prefactura.getPrefactura(id);
        }

        public List<OrdenCompraDTO> getlstOrdenCompra(DateTime inicio, DateTime fin, string cc)
        {
            return Prefactura.getlstOrdenCompra(inicio, fin, cc);
        }

        public InsumosDTO getObjInsumo(DateTime fecha, string cc, int insumo)
        {
            return Prefactura.getObjInsumo(fecha, cc, insumo);
        }

        public List<tipoInsumoDTO> getlstTipoInsumo()
        {
            return Prefactura.getlstTipoInsumo();
        }

        public List<tblF_CatImporte> CboConceptoImporte()
        {
            return Prefactura.CboConceptoImporte();
        }

        public List<tblF_CapImporte> getTotales(int id)
        {
            return Prefactura.getTotales(id);
        }
        public List<ComboDTO> FillComboUsocfdi()
        {
            return Prefactura.FillComboUsocfdi();
        }

        #region FACTURAS EK

        public Dictionary<string,object> FillComboMetodoPagoSat()
        {
            return Prefactura.FillComboMetodoPagoSat();
        }

        public Dictionary<string,object> FillComboRegimenFiscal()
        {
            return Prefactura.FillComboRegimenFiscal();
        }

        public Dictionary<string, object> FillComboTM()
        {
            return Prefactura.FillComboTM();
        }

        public Dictionary<string, object> FillComboTipoFlete()
        {
            return Prefactura.FillComboTipoFlete();
        }

        public Dictionary<string, object> FillComboCondEntrega()
        {
            return Prefactura.FillComboCondEntrega();
        }

        public Dictionary<string, object> FillComboTipoPedido()
        {
            return Prefactura.FillComboTipoPedido();
        }

        public Dictionary<string,object> GetFormaPagoSat(string claveSat)
        {
            return Prefactura.GetFormaPagoSat(claveSat);
        }

        public Dictionary<string, object> FillComboTipoFactura()
        {
            return Prefactura.FillComboTipoFactura();
        }

        public Dictionary<string, object> FillComboSerie()
        {
            return Prefactura.FillComboSerie();
        }

        public Dictionary<string,object> FIllComboInsumos()
        {
            return Prefactura.FIllComboInsumos();
        }
        #endregion

        #region CAT INSUMOS
        public Dictionary<string, object> GetInsumosEK(string idInsumoSAT)
        {
            return Prefactura.GetInsumosEK(idInsumoSAT);
        }
        public Dictionary<string, object> GetInsumosSAT(tblF_EK_InsumosSAT objFiltro)
        {
            return Prefactura.GetInsumosSAT(objFiltro);
        }
        public Dictionary<string, object> CrearEditarInsumos(InsumosSATDAO objInsumo, List<string> lstRel)
        {
            return Prefactura.CrearEditarInsumos(objInsumo, lstRel);
        }
        public Dictionary<string, object> EliminarInsumo(int idInsumo)
        {
            return Prefactura.EliminarInsumo(idInsumo);
        }
        public Dictionary<string, object> EliminarRelInsumo(string idInsumoSAT, string idInsumoEK)
        {
            return Prefactura.EliminarRelInsumo(idInsumoSAT, idInsumoEK);
        }
        public List<InsumosSATDAO> GetAutoCompleteInsumosDesc(string term)
        {
            return Prefactura.GetAutoCompleteInsumosDesc(term);
        }
        public List<InsumosSATDAO> GetAutoCompleteInsumos(string term)
        {
            return Prefactura.GetAutoCompleteInsumos(term);
        }
        #endregion
    }
}
