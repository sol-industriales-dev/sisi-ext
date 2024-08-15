using Core.DAO.Contabilidad.Poliza;
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

namespace Core.Service.Contabilidad.Poliza
{
    public class PolizaService : IPolizaDAO
    {
        #region Atributos
        private IPolizaDAO c_PolizaDAO;
        #endregion
        #region Propiedades
        public IPolizaDAO PolizaDAO
        {
            get { return c_PolizaDAO; }
            set { c_PolizaDAO = value; }
        }
        #endregion
        #region Constructores
        public PolizaService(IPolizaDAO polizaDAO)
        {
            this.PolizaDAO = polizaDAO;
        }
        #endregion
        public bool Guardar(VMPolizaDTO o)
        {
            return this.PolizaDAO.Guardar(o);
        }
        public bool Pagar(List<tblC_CadenaProductiva> lstPagadas)
        {
            return this.PolizaDAO.Pagar(lstPagadas);
        }
        public List<PolizasDTO> getPolizaEk(DateTime fecha, int poliza, string tp)
        {
            return this.PolizaDAO.getPolizaEk(fecha, poliza, tp);
        }
        public List<PolizasDTO> getPolizaEk(DateTime perIni, DateTime perFin, int poliza, string tp)
        {
            return this.PolizaDAO.getPolizaEk(perIni, perFin, poliza, tp);
        }
        public List<RepPolizaDTO> getPolizaEk(string Estatus, int iPol, int fPol, string iPer, string fPer, string iTp, string fTp)
        {
            return this.PolizaDAO.getPolizaEk(Estatus, iPol, fPol, iPer, fPer, iTp, fTp);
        }

        public List<RepPolizaDTO> getPolizaEk(int year, int mes, int poliza, string tp)
        {
            return this.PolizaDAO.getPolizaEk(year, mes, poliza, tp);
        }
        public List<RepPolizaDTO> getPolizaEkPruebaArrendadora(string Estatus, int iPol, int fPol, string iPer, string fPer, string iTp, string fTp)
        {
            return this.PolizaDAO.getPolizaEkPruebaArrendadora(Estatus, iPol, fPol, iPer, fPer, iTp, fTp);
        }
        public List<RepMovPoliza2DTO> getMovPolizaEkPruebaArrendadora(string Estatus, int iPol, int fPol, string iPer, string fPer, string iTp, string fTp, string icc, string fcc)
        {
            return this.PolizaDAO.getMovPolizaEkPruebaArrendadora(Estatus, iPol, fPol, iPer, fPer, iTp, fTp, icc, fcc);
        }
        public List<RepPolizaDTO> getPolizaPorEmpresa(EmpresaEnum empresa, int iPol, int fPol, string iPer, string fPer, string iTp, string fTp)
        {
            return this.PolizaDAO.getPolizaPorEmpresa(empresa, iPol, fPol, iPer, fPer, iTp, fTp);
        }
        public List<RepMovPoliza2DTO> getMovPolizaPorEmpresa(EmpresaEnum empresa, int iPol, int fPol, string iPer, string fPer, string iTp, string fTp, string icc, string fcc)
        {
            return this.PolizaDAO.getMovPolizaPorEmpresa(empresa, iPol, fPol, iPer, fPer, iTp, fTp, icc, fcc);
        }
        public List<MovPolDiarioAcumDTO> getLstPolizaDiario(BusqConcentradoDTO busq)
        {
            return PolizaDAO.getLstPolizaDiario(busq); 
        }
        /// <summary>
        /// Consulta los registros del reporte de condensado de saldos de arrendadora en construplan
        /// </summary>
        /// <param name="busq">Busqueda de propuesta</param>
        /// <returns>Saldos de facutras</returns>
        public List<AnaliticoVencimiento6ColDTO> getLstCondSaldosCplan(BusqConcentradoDTO busq)
        {
            return PolizaDAO.getLstCondSaldosCplan(busq);
        }
        /// <summary>
        /// Consulta los registros del reporte de condensado de saldos de todos los proveedores de la arrendadora
        /// </summary>
        /// <param name="busq">Busqueda de propuesta</param>
        /// <returns>Saldos de facutras</returns>
        public List<AnaliticoVencimiento6ColDTO> getLstAnaliticoVencimiento6ColArrendadora(BusqConcentradoDTO busq)
        {
            return PolizaDAO.getLstAnaliticoVencimiento6ColArrendadora(busq);
        }
        /// <summary>
        /// Consulta los registros del reporte de condensado de saldos de todos los proveedores
        /// </summary>
        /// <param name="busq">Busqueda de propuesta</param>
        /// <returns>Saldos de facutras</returns>
        public List<AnaliticoVencimiento6ColDTO> getLstAnaliticoVencimiento6Col(BusqAnaliticoDTO busq)
        {
            return PolizaDAO.getLstAnaliticoVencimiento6Col(busq);
        }
        /// <summary>
        /// Consulta los registros del reporte de condensado de saldos de los bancos
        /// </summary>
        /// <param name="busq">Busqueda de propuesta</param>
        /// <returns>Saldos de bancos</returns>
        public List<MovProDTO> getLstCondSaldosBancos(BusqConcentradoDTO busq)
        {
            return PolizaDAO.getLstCondSaldosBancos(busq);
        }
        /// <summary>
        /// Registros del reporte de Analítico de vencimientos de 2 columnas
        /// </summary>
        /// <returns>Saldos de facturas</returns>
        public List<tblC_SaldosCondensados> getLstAnaliticoVencimiento(DateTime fechaBusqueda, List<string> lstCC)
        {
            return PolizaDAO.getLstAnaliticoVencimiento(fechaBusqueda, lstCC);
        }
        public List<MovpolDTO> getMovPolizaEk(DateTime fecha, int poliza, string tp)
        {
            return this.PolizaDAO.getMovPolizaEk(fecha, poliza, tp);
        }
        public List<RepMovPoliza2DTO> getMovPolizaEk(string Estatus, int iPol, int fPol, string iPer, string fPer, string iTp, string fTp, string icc, string fcc)
        {
            return this.PolizaDAO.getMovPolizaEk(Estatus, iPol, fPol, iPer, fPer, iTp, fTp, icc, fcc);
        }
        public List<RepMovPoliza2DTO> getMovPolizaEk(int year, int mes, int poliza, string tp)
        {
            return this.PolizaDAO.getMovPolizaEk(year, mes, poliza, tp);
        }
        public List<ProveedorDTO> getProveedor()
        {
            return this.PolizaDAO.getProveedor();
        }
        public List<ProveedorDTO> CatProveedor(EnkontrolEnum conector)
        {
            return PolizaDAO.CatProveedor(conector);
        }
        public List<CatctaDTO> getCuenta()
        {
            return this.PolizaDAO.getCuenta();
        }
        public List<MovProDTO> getFactura(int numPro)
        {
            return this.PolizaDAO.getFactura(numPro);
        }
        public List<MovpolDTO> getPolizaDesdeFactura(List<MovProDTO> lstConsulta)
        {
            return this.PolizaDAO.getPolizaDesdeFactura(lstConsulta);
        }
        public List<MovpolDTO> getCtaCadena(List<MovpolDTO> lstCadena, bool isIntereses, DateTime dllFecha)
        {
            return this.PolizaDAO.getCtaCadena(lstCadena, isIntereses, dllFecha);
        }
        public Dictionary<string, object> GetCtaCadenaNuevo(List<MovpolDTO> lstCadena, bool isIntereses, DateTime dllFecha)
        {
            return this.PolizaDAO.GetCtaCadenaNuevo(lstCadena, isIntereses, dllFecha);
        }
        public object getObjReferencia(string referencia, int numpro, string iSistema)
        {
            return this.PolizaDAO.getObjReferencia(referencia, numpro, iSistema);
        }
        public OrdenCompraPagoDTO getOrdenCompra(int oc, string cc)
        {
            return this.PolizaDAO.getOrdenCompra(oc, cc);
        }
        public string getTmDescipcion(int clave)
        {
            return this.PolizaDAO.getTmDescipcion(clave);
        }
        public decimal GetTipoCambioRegistro(string tp, string factura, string numProv, int tm, string cc, decimal monto)
        {
            return this.PolizaDAO.GetTipoCambioRegistro(tp, factura, numProv, tm, cc, monto);
        }
        public CatctaDTO getCuentata(int cta, int scta, int sscta)
        {
            return this.PolizaDAO.getCuentata(cta, scta, sscta);
        }
        public CtacomplDTO getCtacompl(int cta, int scta, int sscta)
        {
            return this.PolizaDAO.getCtacompl(cta, scta, sscta);
        }
        public CtaIvaDTO getCtaIva(string iSistema)
        {
            return this.PolizaDAO.getCtaIva(iSistema);
        }
        public DifCambiariaDTO getCtaDiffCambiaria(string iSistema)
        {
            return this.PolizaDAO.getCtaDiffCambiaria(iSistema);
        }
        public CuentaDTO getInterfaceDescripcion(int cta, int scta, int sscta, string tp, int numprov)
        {
            return this.PolizaDAO.getInterfaceDescripcion(cta, scta, sscta, tp, numprov);
        }
        public string getInterfaceSistema(int cta, int scta, int sscta)
        {
            return this.PolizaDAO.getInterfaceSistema(cta, scta, sscta);
        }
        public int getNumPoliza(string tp, DateTime fecha)
        {
            return this.PolizaDAO.getNumPoliza(tp, fecha);
        }
        public bool guardarLstSaldosCondensados(List<tblC_SaldosCondensados> lst)
        {
            return PolizaDAO.guardarLstSaldosCondensados(lst);
        }
        public List<tblC_SaldosCondensados> getAllCondSaldosActivos()
        {
            return PolizaDAO.getAllCondSaldosActivos();
        }
        #region Combobox
        public List<ComboDTO> getComboTipoPoliza()
        {
            return this.PolizaDAO.getComboTipoPoliza();
        }
        public List<ComboDTO> getComboOc()
        {
            return this.PolizaDAO.getComboOc();
        }
        public List<ComboDTO> getComboCentroCostos()
        {
            return PolizaDAO.getComboCentroCostos();
        }
        public List<ComboDTO> getComboAreaCuenta()
        {
            return PolizaDAO.getComboAreaCuenta();
        }
        public List<ComboDTO> getComboTipoMovimiento(string iSistema)
        {
            return this.PolizaDAO.getComboTipoMovimiento(iSistema);
        }
        public List<ComboDTO> getComboTipoMovimiento()
        {
            return PolizaDAO.getComboTipoMovimiento();
        }
        public List<ComboDTO> lstObra()
        {
            return this.PolizaDAO.lstObra();
        }
        #endregion
        public List<tblC_CCProrrateo> getLstCCProrrateo()
        {
            return PolizaDAO.getLstCCProrrateo();
        }
        public List<tblC_CCProrrateo> getLstCCProrrateo(string cc)
        {
            return PolizaDAO.getLstCCProrrateo(cc);
        }
        public bool guardarLstCCProrrateo(List<tblC_CCProrrateo> lst)
        {
            return PolizaDAO.guardarLstCCProrrateo(lst);
        }
        public List<tblC_RelCCPropuesta> getRelCCPropuesta()
        {
            return PolizaDAO.getRelCCPropuesta();
        }
        public List<tblC_RelCCPropuesta> getRelCCPropuesta(string ccSecundario)
        {
            return PolizaDAO.getRelCCPropuesta(ccSecundario);
        }
        public bool guarderRelCCPropuesta(List<tblC_RelCCPropuesta> lst)
        {
            return PolizaDAO.guarderRelCCPropuesta(lst);
        }
        #region Asincrono
        public Task<CatctaDTO> agetCuentata(int cta, int scta, int sscta)
        {
            return this.PolizaDAO.agetCuentata(cta, scta, sscta);
        }
        public Task<string> agetInterfaceSistema(int cta, int scta, int sscta)
        {
            return this.PolizaDAO.agetInterfaceSistema(cta, scta, sscta);
        }
        public Task<List<ComboDTO>> agetComboTipoMovimiento(Task<string> iSistema)
        {
            return this.PolizaDAO.agetComboTipoMovimiento(iSistema);
        }
        public Task<CuentaDTO> agetInterfaceDescripcion(int cta, int scta, int sscta, string tp, int numprov)
        {
            return this.PolizaDAO.agetInterfaceDescripcion(cta, scta, sscta, tp, numprov);
        }
        public Task<object> agetObjReferencia(string referencia, int numpro, string iSistema)
        {
            return this.PolizaDAO.agetObjReferencia(referencia, numpro, iSistema);
        }
        public Task<List<MovpolDTO>> agetMovPolizaEk(DateTime fecha, int poliza, string tp)
        {
            return this.PolizaDAO.agetMovPolizaEk(fecha, poliza, tp);
        }
        #endregion

        public bool aplicarBloqueo()
        {
            return PolizaDAO.aplicarBloqueo();
        }

        public List<SubcontratistaBloqueadoDTO> subcontratistasBloqueados()
        {
            return PolizaDAO.subcontratistasBloqueados();
        }

        #region Peru
        public List<ProveedorDTO> getProveedorPeru()
        {
            return this.PolizaDAO.getProveedorPeru();
        }
        #endregion
    }
}
