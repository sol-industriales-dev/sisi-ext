using Core.DTO.Administracion.Cheque;
using Core.DTO.Contabilidad.Cheque;
using Core.Entity.Administrativo.Contabilidad.Cheque;
using Core.Entity.Administrativo.Contabilidad.Cheques;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Cheque
{
    public interface ICapChequeDAO
    {

        #region Consultas en SQL SERVER

        #endregion
        #region Consultas en ENKONTROL
        dynamic GetListaProveedores(string term, bool porDesc);
        Dictionary<string, object> GetListaCuentasInit();
        Dictionary<string, object> ComboTipoMovimientos();
        Dictionary<string, object> ComboSubTipoMovimiento();
        Dictionary<string, object> GetMovPol(int poliza, int m, int a);
        Dictionary<string, object> GetBeneficiario(string term);
        Dictionary<string, object> GetInfoBanco(int banco);
        #endregion
        Dictionary<string, object> GuardarCheque(tblC_sb_cheques objSave, int ocID, List<tblC_sc_movpol> listaMovpol, int tipoCheque);
        Dictionary<string, object> ObtenerDatosPoliza(tblC_sb_cheques objSave, int ocID);
        Dictionary<string, object> GetListaCheques(filtroBusquedaDTO objFiltros);
        Dictionary<string, object> SetUltimoCheque(int cuenta);
        Dictionary<string, object> GetInfoCheque(int cuenta);
        Dictionary<string, object> GetTipoMovimientos();
        Dictionary<string, object> GetCuentasBanco();
        Dictionary<string, object> GetProveedores();
        Dictionary<string, object> GetPolizas(int iPoliza);
        Dictionary<string, object> GetPolizasCheque(int iPoliza, string fecha);
        dynamic GetListaCuentas(string term, bool porDesc);
        Dictionary<string, object> GetOrdenCompraAnticipo();
        Dictionary<string, object> CboEconomico();
        Dictionary<string, object> GetCheques(filtroCheques filtros);
        Dictionary<string, object> SaveOrUpdatePoliza(List<tblC_sc_movpol> data);
        Dictionary<string, object> ValidaPoliza(List<tblC_sc_movpol> listaMovpol);
        PrintInfoChequeDTO GetInfoCheque(int iPoliza, int mes, int year);
        Dictionary<string, object> BuscarCuenta(int cuenta, int subCuenta, int ssubCuenta);
        dynamic GetDescripcionesCta(string term);
        Dictionary<string, object> DeleteCheque(int chequeID);
        Dictionary<string, object> BuscarOC(string cc, int numero);
        Dictionary<string, object> GetOCSeleccionado(string cc, int numero);

        Dictionary<string, object> OpenEditCheques(int idCheque);
    }
}