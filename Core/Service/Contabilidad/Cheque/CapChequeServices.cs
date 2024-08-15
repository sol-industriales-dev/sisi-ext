using Core.DAO.Contabilidad.Cheque;
using Core.DTO.Administracion.Cheque;
using Core.DTO.Contabilidad.Cheque;
using Core.Entity.Administrativo.Contabilidad.Cheque;
using Core.Entity.Administrativo.Contabilidad.Cheques;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.Cheque
{
    public class CapChequeServices : ICapChequeDAO
    {
        #region Variables y constructor
        private ICapChequeDAO capChequeDAO { get; set; }

        public CapChequeServices(ICapChequeDAO iCapChequeDAO)
        {
            capChequeDAO = iCapChequeDAO;
        }
        #endregion

        public Dictionary<string, object> GuardarCheque(tblC_sb_cheques objSave, int ocID, List<tblC_sc_movpol> listaMovpol, int tipoCheque)
        {
            return capChequeDAO.GuardarCheque(objSave, ocID, listaMovpol, tipoCheque);
        }
        public Dictionary<string, object> ObtenerDatosPoliza(tblC_sb_cheques objSave, int ocID)
        {
            return capChequeDAO.ObtenerDatosPoliza(objSave, ocID);
        }

        public Dictionary<string, object> GetListaCheques(filtroBusquedaDTO objFiltros)
        {
            return capChequeDAO.GetListaCheques(objFiltros);
        }
        public Dictionary<string, object> SetUltimoCheque(int cuenta)
        {
            return capChequeDAO.SetUltimoCheque(cuenta);
        }
        public Dictionary<string, object> GetBeneficiario(string term)
        {
            return capChequeDAO.GetBeneficiario(term);
        }
        //m = Mes
        //a = Año
        public Dictionary<string, object> GetMovPol(int poliza, int m, int a)
        {
            return capChequeDAO.GetMovPol(poliza, m, a);
        }
        public Dictionary<string, object> ComboSubTipoMovimiento()
        {
            return capChequeDAO.ComboSubTipoMovimiento();
        }
        public Dictionary<string, object> ComboTipoMovimientos()
        {
            return capChequeDAO.ComboTipoMovimientos();
        }
        public Dictionary<string, object> GetListaCuentasInit()
        {
            return capChequeDAO.GetListaCuentasInit();
        }
        public dynamic GetListaProveedores(string term, bool porDesc)
        {
            return capChequeDAO.GetListaProveedores(term, porDesc);
        }
        public Dictionary<string, object> GetInfoBanco(int banco)
        {
            return capChequeDAO.GetInfoBanco(banco);
        }
        public Dictionary<string, object> GetInfoCheque(int cuenta)
        {
            return capChequeDAO.GetInfoCheque(cuenta);
        }
        public Dictionary<string, object> GetTipoMovimientos()
        {
            return capChequeDAO.GetTipoMovimientos();
        }
        public Dictionary<string, object> GetProveedores()
        {
            return capChequeDAO.GetProveedores();
        }

        public Dictionary<string, object> GetCuentasBanco()
        {
            return capChequeDAO.GetCuentasBanco();
        }
        public Dictionary<string, object> GetPolizas(int iPoliza)
        {
            return capChequeDAO.GetPolizas(iPoliza);
        }

        public Dictionary<string, object> GetPolizasCheque(int iPoliza, string fecha)
        {
            return capChequeDAO.GetPolizasCheque(iPoliza, fecha);
        }

        public dynamic GetListaCuentas(string term, bool porDesc)
        {
            return capChequeDAO.GetListaCuentas(term, porDesc);
        }
        public Dictionary<string, object> GetOrdenCompraAnticipo()
        {
            return capChequeDAO.GetOrdenCompraAnticipo();
        }
        public Dictionary<string, object> CboEconomico()
        {
            return capChequeDAO.CboEconomico();
        }
        public Dictionary<string, object> GetCheques(filtroCheques filtros)
        {
            return capChequeDAO.GetCheques(filtros);
        }
        public Dictionary<string, object> SaveOrUpdatePoliza(List<tblC_sc_movpol> data)
        {
            return capChequeDAO.SaveOrUpdatePoliza(data);
        }
        public Dictionary<string, object> ValidaPoliza(List<tblC_sc_movpol> listaMovpol)
        {
            return capChequeDAO.ValidaPoliza(listaMovpol);
        }

        public PrintInfoChequeDTO GetInfoCheque(int iPoliza, int mes, int year)
        {
            return capChequeDAO.GetInfoCheque(iPoliza, mes, year);

        }

        public Dictionary<string, object> BuscarCuenta(int cuenta, int subCuenta, int ssubCuenta)
        {
            return capChequeDAO.BuscarCuenta(cuenta, subCuenta, ssubCuenta);
        }

        public dynamic GetDescripcionesCta(string term)
        {
            return capChequeDAO.GetDescripcionesCta(term);
        }
        public Dictionary<string, object> BuscarOC(string cc, int numero)
        {
            return capChequeDAO.BuscarOC(cc, numero);
        }

        public Dictionary<string, object> DeleteCheque(int chequeID)
        {
            return capChequeDAO.DeleteCheque(chequeID);
        }

        public Dictionary<string, object> GetOCSeleccionado(string cc, int numero)
        {
            return capChequeDAO.GetOCSeleccionado(cc, numero);
        }

        public Dictionary<string, object> OpenEditCheques(int idCheque)
        {
            return capChequeDAO.OpenEditCheques(idCheque);
        }

    }
}
