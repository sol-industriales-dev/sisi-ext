using Core.DTO.Contabilidad.Poliza;
using Core.DTO.Contabilidad.Propuesta;
using Core.DTO.Principal.Generales;
using Core.DTO.Principal.Usuarios;
using Core.DTO.Subcontratistas.Bloqueo;
using Core.Entity.Administrativo.Contabilidad;
using Core.Entity.Administrativo.Contabilidad.Poliza;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Proveedores;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Poliza
{
    public interface IPolizaDAO
    {
        bool Guardar(VMPolizaDTO o);
        bool Pagar(List<tblC_CadenaProductiva> lstPagadas);
        List<PolizasDTO> getPolizaEk(DateTime fecha, int poliza, string tp);
        List<PolizasDTO> getPolizaEk(DateTime perIni, DateTime perFin, int poliza, string tp);
        List<RepPolizaDTO> getPolizaEk(string Estatus, int iPol, int fPol, string iPer, string fPer, string iTp, string fTp);
        List<RepPolizaDTO> getPolizaEk(int year, int mes, int poliza, string tp);
        List<RepPolizaDTO> getPolizaEkPruebaArrendadora(string Estatus, int iPol, int fPol, string iPer, string fPer, string iTp, string fTp);
        List<RepMovPoliza2DTO> getMovPolizaEkPruebaArrendadora(string Estatus, int iPol, int fPol, string iPer, string fPer, string iTp, string fTp, string icc, string fcc);
        List<RepPolizaDTO> getPolizaPorEmpresa(EmpresaEnum empresa, int iPol, int fPol, string iPer, string fPer, string iTp, string fTp);
        List<RepMovPoliza2DTO> getMovPolizaPorEmpresa(EmpresaEnum empresa, int iPol, int fPol, string iPer, string fPer, string iTp, string fTp, string icc, string fcc);
        List<MovPolDiarioAcumDTO> getLstPolizaDiario(BusqConcentradoDTO busq);
        List<AnaliticoVencimiento6ColDTO> getLstCondSaldosCplan(BusqConcentradoDTO busq);
        List<AnaliticoVencimiento6ColDTO> getLstAnaliticoVencimiento6ColArrendadora(BusqConcentradoDTO busq);
        List<AnaliticoVencimiento6ColDTO> getLstAnaliticoVencimiento6Col(BusqAnaliticoDTO busq);
        List<MovProDTO> getLstCondSaldosBancos(BusqConcentradoDTO busq);
        List<tblC_SaldosCondensados> getLstAnaliticoVencimiento(DateTime fechaBusqueda, List<string> lstCC);
        List<MovpolDTO> getMovPolizaEk(DateTime fecha, int poliza, string tp);
        List<RepMovPoliza2DTO> getMovPolizaEk(string Estatus, int iPol, int fPol, string iPer, string fPer, string iTp, string fTp, string icc, string fcc);
        List<RepMovPoliza2DTO> getMovPolizaEk(int year, int mes, int poliza, string tp);
        List<ProveedorDTO> getProveedor();
        List<ProveedorDTO> CatProveedor(EnkontrolEnum conector);
        List<CatctaDTO> getCuenta();
        List<MovProDTO> getFactura(int numPro);
        List<MovpolDTO> getPolizaDesdeFactura(List<MovProDTO> lstConsulta);
        List<MovpolDTO> getCtaCadena(List<MovpolDTO> lstCadena, bool isIntereses, DateTime dllFecha);
        Dictionary<string, object> GetCtaCadenaNuevo(List<MovpolDTO> lstCadena, bool isIntereses, DateTime dllFecha);
        object getObjReferencia(string referencia, int numpro, string iSistema);
        OrdenCompraPagoDTO getOrdenCompra(int oc, string cc);
        string getTmDescipcion(int clave);
        decimal GetTipoCambioRegistro(string tp, string factura, string numProv, int tm, string cc, decimal monto);
        CatctaDTO getCuentata(int cta, int scta, int sscta);
        CtacomplDTO getCtacompl(int cta, int scta, int sscta);
        CtaIvaDTO getCtaIva(string iSistema);
        DifCambiariaDTO getCtaDiffCambiaria(string iSistema);
        CuentaDTO getInterfaceDescripcion(int cta, int scta, int sscta, string tp, int numprov);
        string getInterfaceSistema(int cta, int scta, int sscta);
        int getNumPoliza(string tp, DateTime fecha);
        bool guardarLstSaldosCondensados(List<tblC_SaldosCondensados> lst);
        List<tblC_SaldosCondensados> getAllCondSaldosActivos();
        List<tblC_CCProrrateo> getLstCCProrrateo();
        List<tblC_CCProrrateo> getLstCCProrrateo(string cc);
        bool guardarLstCCProrrateo(List<tblC_CCProrrateo> lst);
        List<tblC_RelCCPropuesta> getRelCCPropuesta();
        List<tblC_RelCCPropuesta> getRelCCPropuesta(string ccSecundario);
        bool guarderRelCCPropuesta(List<tblC_RelCCPropuesta> lst);
        #region ComboBox
        List<ComboDTO> getComboTipoPoliza();
        List<ComboDTO> getComboOc();
        List<ComboDTO> getComboCentroCostos();
        List<ComboDTO> getComboAreaCuenta();
        List<ComboDTO> getComboTipoMovimiento();
        List<ComboDTO> getComboTipoMovimiento(string iSistema);
        List<ComboDTO> lstObra();
        #endregion
        #region Asincronos
        Task<CatctaDTO> agetCuentata(int cta, int scta, int sscta);
        Task<string> agetInterfaceSistema(int cta, int scta, int sscta);
        Task<List<ComboDTO>> agetComboTipoMovimiento(Task<string> iSistema);
        Task<CuentaDTO> agetInterfaceDescripcion(int cta, int scta, int sscta, string tp, int numprov);
        Task<object> agetObjReferencia(string referencia, int numpro, string iSistema);
        Task<List<MovpolDTO>> agetMovPolizaEk(DateTime fecha, int poliza, string tp);
        #endregion

        bool aplicarBloqueo();

        List<SubcontratistaBloqueadoDTO> subcontratistasBloqueados();

        #region Peru
        List<ProveedorDTO> getProveedorPeru();
        #endregion
    }
}
