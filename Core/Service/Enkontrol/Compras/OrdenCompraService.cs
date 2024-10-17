using Core.DAO.Enkontrol.Compras;
using Core.DTO.Enkontrol.Alamcen;
using Core.DTO.Enkontrol.OrdenCompra;
using Core.DTO.Enkontrol.Requisicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.Enkontrol.Compras
{
    public class OrdenCompraService : IOrdenCompraDAO
    {
        #region Atributos
        public IOrdenCompraDAO e_ocDAO;
        #endregion
        #region Propiedades
        public IOrdenCompraDAO OcDAO
        {
            get { return e_ocDAO; }
            set { e_ocDAO = value; }
        }
        #endregion
        #region Constructor
        public OrdenCompraService(IOrdenCompraDAO ocDAO)
        {
            this.OcDAO = ocDAO;
        }
        #endregion
        public bool puedeVerCheckBoxProvNoOptimo()
        {
            return OcDAO.puedeVerCheckBoxProvNoOptimo();
        }

        public List<int> generaNuevaOC(List<GenOrdenCompraDTO> lstOC)
        {
            return OcDAO.generaNuevaOC(lstOC);
        }
        public dynamic getComprador()
        {
            return OcDAO.getComprador();
        }
        public dynamic busqReq(BusqReq busq)
        {
            return OcDAO.busqReq(busq);
        }
        public dynamic busqReqNum(BusqReq busq)
        {
            return OcDAO.busqReqNum(busq);
        }
        public dynamic getPartidas(string cc, int num, int moneda)
        {
            return OcDAO.getPartidas(cc, num, moneda);
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcReqComprador(BusqReq busq)
        {
            return OcDAO.FillComboCcReqComprador(busq);
        }
        public dynamic getProvFromNum(string term)
        {
            return OcDAO.getProvFromNum(term);
        }
        public dynamic getProvFromNom(string term)
        {
            return OcDAO.getProvFromNom(term);
        }

        public OrdenCompraDTO getCompra(string cc, int num, bool esOC_INTERNA, string PERU_tipoCompra = "")
        {
            return OcDAO.getCompra(cc, num, esOC_INTERNA, PERU_tipoCompra);
        }
        public dynamic getCompraRelacionar(string cc, int num)
        {
            return OcDAO.getCompraRelacionar(cc, num);
        }
        public OrdenCompraDTO getCompra_Interna(string cc, int num)
        {
            return OcDAO.getCompra_Interna(cc, num);
        }
        public Dictionary<string, object> updateRetencionesCompra(List<OrdenCompraRetencionesDTO> retenciones)
        {
            return OcDAO.updateRetencionesCompra(retenciones);
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcComComprador()
        {
            return OcDAO.FillComboCcComComprador();
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcComCompradorModalEditar()
        {
            return OcDAO.FillComboCcComCompradorModalEditar();
        }

        public OrdenCompraDTO updateCompra(OrdenCompraDTO compra)
        {
            return OcDAO.updateCompra(compra);
        }
        public OrdenCompraDTO updateCompraInterna(OrdenCompraDTO compra)
        {
            return OcDAO.updateCompraInterna(compra);
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcAut(bool isAuth)
        {
            return OcDAO.FillComboCcAut(isAuth);
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcAutTodas()
        {
            return OcDAO.FillComboCcAutTodas();
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcFiltroPorUsuario()
        {
            return OcDAO.FillComboCcFiltroPorUsuario();
        }

        public List<OrdenCompraDTO> getListaCompras(bool isAuth, string cc, bool propias)
        {
            return OcDAO.getListaCompras(isAuth, cc, propias);
        }

        public List<OrdenCompraDTO> getListaComprasTodas(string cc, bool pendientes, bool propias, int area, int cuenta)
        {
            return OcDAO.getListaComprasTodas(cc, pendientes, propias, area, cuenta);
        }

        public List<VoboDTO> getVobos(OrdenCompraDTO compra)
        {
            return OcDAO.getVobos(compra);
        }

        public List<Tuple<int, List<VoboDTO>>> getVobos(List<OrdenCompraDTO> compras, int empresa, bool esInterna)
        {
            return OcDAO.getVobos(compras, empresa, esInterna);
        }

        public List<dynamic> getAutorizaciones(OrdenCompraDTO compra)
        {
            return OcDAO.getAutorizaciones(compra);
        }

        public List<Tuple<int, List<VoboDTO>>> getAutorizaciones(List<OrdenCompraDTO> compras, int empresa, bool esInterna)
        {
            return OcDAO.getVobos(compras, empresa, esInterna);
        }

        public void autorizarCompra(string cc, int numero, bool isAut, bool esOC_Interna)
        {
            OcDAO.autorizarCompra(cc, numero, isAut, esOC_Interna);
        }

        public void desautorizarCompra(string cc, int numero)
        {
            OcDAO.desautorizarCompra(cc, numero);
        }

        public dynamic getUsuarioEnKontrol(int numEmpleado)
        {
            return OcDAO.getUsuarioEnKontrol(numEmpleado);
        }

        public void voboCompra(OrdenCompraDTO compra, string voboNumero, List<VoboDTO> vobos, bool esOC_Interna)
        {
            OcDAO.voboCompra(compra, voboNumero, vobos, esOC_Interna);
        }

        public bool VerificarOC(OrdenCompraDTO objParamsDTO)
        {
            return OcDAO.VerificarOC(objParamsDTO);
        }

        public bool getFlagPuedeDarVobo(List<int> numUsuariosEnkontrol, OrdenCompraDTO compra)
        {
            return OcDAO.getFlagPuedeDarVobo(numUsuariosEnkontrol, compra);
        }

        public List<OrdenCompraDesautorizacionDTO> getListaComprasDes(string cc)
        {
            return OcDAO.getListaComprasDes(cc);
        }

        public List<entradasAlmacenDTO> guardarSurtido(OrdenCompraDTO compra, List<SurtidoCompraDTO> surtido)
        {
            return OcDAO.guardarSurtido(compra, surtido);
        }
        public List<entradasAlmacenDTO> guardarSurtidoNoInventariable(OrdenCompraDTO compra, List<SurtidoCompraDTO> surtido)
        {
            return OcDAO.guardarSurtidoNoInventariable(compra, surtido);
        }
        public List<CuadroComparativoDTO> buscarCuadros(BusquedaCuadroDTO filtros)
        {
            return OcDAO.buscarCuadros(filtros);
        }

        public dynamic requisicionesNumeros(string cc)
        {
            return OcDAO.requisicionesNumeros(cc);
        }

        public dynamic FillComboAreaCuentaTodas()
        {
            return OcDAO.FillComboAreaCuentaTodas();
        }

        public CuadroComparativoDTO getCuadroDet(CuadroComparativoDTO cuadro)
        {
            return OcDAO.getCuadroDet(cuadro);
        }
        public string getLABFromNum(int num)
        {
            return OcDAO.getLABFromNum(num);
        }

        public InfoProveedorMonedaDTO getProveedorInfo(string num, string PERU_tipoCambio)
        {
            return OcDAO.getProveedorInfo(num, PERU_tipoCambio);
        }
        public Dictionary<string, object> GuardarNuevoCuadro(CuadroComparativoDTO nuevoCuadro)
        {
            return OcDAO.GuardarNuevoCuadro(nuevoCuadro);
        }
        public Dictionary<string, object> UpdateCuadro(CuadroComparativoDTO cuadro)
        {
            return OcDAO.UpdateCuadro(cuadro);
        }

        public Dictionary<string, object> BorrarCuadro(CuadroComparativoDTO cuadro)
        {
            return OcDAO.BorrarCuadro(cuadro);
        }

        public Dictionary<string, object> guardarNuevaCompra(OrdenCompraDTO compra)
        {
            return OcDAO.guardarNuevaCompra(compra);
        }

        public Dictionary<string, object> guardarNuevaCompraInterna(OrdenCompraDTO compra)
        {
            return OcDAO.guardarNuevaCompraInterna(compra);
        }

        public OrdenCompraDTO getRequisicion(string cc, int num)
        {
            return OcDAO.getRequisicion(cc, num);
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoReq()
        {
            return OcDAO.FillComboTipoReq();
        }

        public string getNombreUsuarioEmpleado(int numEmpleado)
        {
            return OcDAO.getNombreUsuarioEmpleado(numEmpleado);
        }

        public string getNombreAlmacen(int numAlmacen)
        {
            return OcDAO.getNombreAlmacen(numAlmacen);
        }

        public RetencionInfoDTO getRetencionInfo(int id_cpto)
        {
            return OcDAO.getRetencionInfo(id_cpto);
        }

        public dynamic getProveedorNumero(string proveedor)
        {
            return OcDAO.getProveedorNumero(proveedor);
        }

        public dynamic getCompradorNumero(string comprador)
        {
            return OcDAO.getCompradorNumero(comprador);
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcAsigComp()
        {
            return OcDAO.FillComboCcAsigComp();
        }

        public Dictionary<string, object> ObtenerComprasPendientes(string cc, int estatus, int proveedor, DateTime fechaInicial, DateTime fechaFinal, string idAreaCuenta, int idCompradorEK)
        {
            return OcDAO.ObtenerComprasPendientes(cc, estatus, proveedor, fechaInicial, fechaFinal, idAreaCuenta, idCompradorEK);
        }
        public Dictionary<string, object> ObtenerComprasSinFactura(string cc, int estatus, int proveedor, DateTime fechaInicial, DateTime fechaFinal, string idAreaCuenta, int idCompradorEK)
        {
            return OcDAO.ObtenerComprasSinFactura(cc, estatus, proveedor, fechaInicial, fechaFinal, idAreaCuenta, idCompradorEK);
        }
        public UltimaCompraDTO getUltimaCompra(CuadroComparativoDetDTO partidaCuadro)
        {
            return OcDAO.getUltimaCompra(partidaCuadro);
        }

        public rptOrdenCompraInfoDTO getOrdenCompraRpt(string cc, int numero, string PERU_tipoCompra)
        {
            return OcDAO.getOrdenCompraRpt(cc, numero, PERU_tipoCompra);
        }
        public rptOrdenCompraInfoDTO getOrdenCompraInternaRpt(string cc, int numero)
        {
            return OcDAO.getOrdenCompraInternaRpt(cc, numero);
        }
        public Dictionary<string, object> puedeCancelar()
        {
            return OcDAO.puedeCancelar();
        }

        public Dictionary<string, object> cancelarCompra(string cc, int numero)
        {
            return OcDAO.cancelarCompra(cc, numero);
        }

        public void CancelarComprasMasivo()
        {
            OcDAO.CancelarComprasMasivo();
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCc()
        {
            return OcDAO.FillComboCc();
        }

        public List<ComboFamiliaInsumoDTO> FillComboFamiliasInsumos()
        {
            return OcDAO.FillComboFamiliasInsumos();
        }

        public Dictionary<string, object> getRequisicionesValidadas(List<string> listCC, List<string> listFamiliasInsumos, List<string> listCompradores, DateTime fechaInicio, DateTime fechaFin, int area, int cuenta, string noEconomico)
        {
            return OcDAO.getRequisicionesValidadas(listCC, listFamiliasInsumos, listCompradores, fechaInicio, fechaFin, area, cuenta, noEconomico);
        }

        public Dictionary<string, object> cancelarParcialCompra(OrdenCompraDTO compra)
        {
            return OcDAO.cancelarParcialCompra(compra);
        }

        public Dictionary<string, object> getPreciosPorProveedor(string cc, int numeroRequisicion, long numeroProveedor)
        {
            return OcDAO.getPreciosPorProveedor(cc, numeroRequisicion, numeroProveedor);
        }

        public Dictionary<string, object> checkEstatusOrdenCompraImpresa(string cc, int numero, string PERU_tipoCompra = "")
        {
            return OcDAO.checkEstatusOrdenCompraImpresa(cc, numero, PERU_tipoCompra);
        }

        public Dictionary<string, object> checkEstatusOrdenCompraImpresaConsulta(string cc, int numero)
        {
            return OcDAO.checkEstatusOrdenCompraImpresaConsulta(cc, numero);
        }

        public Dictionary<string, object> getContadorRequisicionesPendientes()
        {
            return OcDAO.getContadorRequisicionesPendientes();
        }

        public void borrarCompra(string cc, int numero, bool autorizante)
        {
            OcDAO.borrarCompra(cc, numero, autorizante);
        }

        public void BorrarComprasMasivo()
        {
            OcDAO.BorrarComprasMasivo();
        }

        public CuadroComparativoReporteDTO getCuadroReporte(string cc, int numero, int folio)
        {
            return OcDAO.getCuadroReporte(cc, numero, folio);
        }

        public bool checarUbicacionesValidas(OrdenCompraDTO compra, List<SurtidoCompraDTO> entradas)
        {
            return OcDAO.checarUbicacionesValidas(compra, entradas);
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCompradores()
        {
            return OcDAO.FillComboCompradores();
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCompradores(string cc)
        {
            return OcDAO.FillComboCompradores(cc);
        }
        public List<RetencionInfoDTO> getCatalogoRetenciones()
        {
            return OcDAO.getCatalogoRetenciones();
        }

        public MovimientoEnkontrolDTO getMovimientoNoInv(int almacenID, int remision)
        {
            return OcDAO.getMovimientoNoInv(almacenID, remision);
        }

        public dynamic getPresupuestoCC(string cc)
        {
            return OcDAO.getPresupuestoCC(cc);
        }

        public dynamic getPeriodoContable()
        {
            return OcDAO.getPeriodoContable();
        }

        public List<entradasAlmacenDTO> GetDatosReporteEntradaOC(string cc, int? num, long numMovimiento)
        {
            return OcDAO.GetDatosReporteEntradaOC(cc, num, numMovimiento);
        }

        public List<entradasAlmacenDTO> GetDatosReporteEntradaNoInvOC(string cc, int? num, long numMovimiento)
        {
            return OcDAO.GetDatosReporteEntradaNoInvOC(cc, num, numMovimiento);
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboProveedores()
        {
            return OcDAO.FillComboProveedores();
        }

        public dynamic getEntradas(string cc, int numero)
        {
            return OcDAO.getEntradas(cc, numero);
        }

        public void actualizarColocadaFechaYProveedor()
        {
            OcDAO.actualizarColocadaFechaYProveedor();
        }

        public void actualizarImpresa()
        {
            OcDAO.actualizarImpresa();
        }

        public void enviarOCProv(string cc, int numero, string correo, HttpPostedFileBase cotizacion)
        {
            OcDAO.enviarOCProv(cc, numero, correo, cotizacion);
        }

        public Dictionary<string, object> auditoriaEliminarReqOC()
        {
            return OcDAO.auditoriaEliminarReqOC();
        }

        public Dictionary<string, object> getAuditoriaRequisicionesComprasAfectadas()
        {
            return OcDAO.getAuditoriaRequisicionesComprasAfectadas();
        }

        public Dictionary<string, object> getComprasProveedor(string cc, DateTime fechaInicio, DateTime fechaFin, int proveedor, int area, int cuenta)
        {
            return OcDAO.getComprasProveedor(cc, fechaInicio, fechaFin, proveedor, area, cuenta);
        }

        public Dictionary<string, object> getProveedoresCC(string cc, DateTime fechaInicio, DateTime fechaFin)
        {
            return OcDAO.getProveedoresCC(cc, fechaInicio, fechaFin);
        }

        public Dictionary<string, object> getAreasCuentasCCFechaProveedor(string cc, DateTime fechaInicio, DateTime fechaFin, int proveedor)
        {
            return OcDAO.getAreasCuentasCCFechaProveedor(cc, fechaInicio, fechaFin, proveedor);
        }


        #region Esconder boton enviar correo si no son compradores
        public bool usuarioCompradorExiste()
        {
            return OcDAO.usuarioCompradorExiste();
        }
        #endregion

        public List<trazabilidadDTO> getTrazabilidadGeneral(trazabilidad_filtrosDTO filtro)
        {
            return OcDAO.getTrazabilidadGeneral(filtro);
        }
        public List<trazabilidadDTO> getTrazabilidadGeneralv2(trazabilidad_filtrosDTO filtro)
        {
            return OcDAO.getTrazabilidadGeneralv2(filtro);
        }
        public void actualizarComprasDesautorizadas()
        {
            OcDAO.actualizarComprasDesautorizadas();
        }

        public Dictionary<string, object> GetProveedoresInsumos(List<string> listaInsumos)
        {
            return OcDAO.GetProveedoresInsumos(listaInsumos);
        }

        public List<EmpleadoPendienteLiberacionDTO> getEmpleadosPendientesLiberacion()
        {
            return OcDAO.getEmpleadosPendientesLiberacion();
        }
        public void guardarBajas(List<EmpleadoPendienteLiberacionDTO> empleados)
        {
            OcDAO.guardarBajas(empleados);
        }

        #region GENERAR LINK
        public Dictionary<string, object> FillCboProveedoresGenerarLink()
        {
            return OcDAO.FillCboProveedoresGenerarLink();
        }

        public Dictionary<string, object> CEProveedorLink(ProveedorLinkDTO objDTO)
        {
            return OcDAO.CEProveedorLink(objDTO);
        }

        public Dictionary<string, object> EliminarProveedorLink(ProveedorLinkDTO objDTO)
        {
            return OcDAO.EliminarProveedorLink(objDTO);
        }

        public Dictionary<string, object> GetProveedoresLink(ProveedorLinkDTO objDTO)
        {
            return OcDAO.GetProveedoresLink(objDTO);
        }

        public Dictionary<string, object> FillCboProveedoresGenerarLinkRegistrados(ProveedorLinkDTO objDTO)
        {
            return OcDAO.FillCboProveedoresGenerarLinkRegistrados(objDTO);
        }

        public Dictionary<string, object> EnviarCorreoLinkProveedores(ProveedorLinkDTO objDTO)
        {
            return OcDAO.EnviarCorreoLinkProveedores(objDTO);
        }

        public Dictionary<string, object> IndicarEnvioCorreoExternamente(ProveedorLinkDTO objDTO)
        {
            return OcDAO.IndicarEnvioCorreoExternamente(objDTO);
        }
        #endregion

        public Dictionary<string, object> ImpresionMasivaCompras()
        {
            return OcDAO.ImpresionMasivaCompras();
        }

        public Dictionary<string, object> FillComboFormaPagoPeru()
        {
            return OcDAO.FillComboFormaPagoPeru();
        }

        public Dictionary<string, object> FillComboTipoDocumentoPeru()
        {
            return OcDAO.FillComboTipoDocumentoPeru();
        }

        public Dictionary<string, object> RegistrarVoBoAutorizarOrdenCompra(List<OrdenCompraDTO> listaVobos, List<OrdenCompraDTO> listaAutorizados)
        {
            return OcDAO.RegistrarVoBoAutorizarOrdenCompra(listaVobos, listaAutorizados);
        }

        public Dictionary<string, object> GetEmpresaLogueada()
        {
            return OcDAO.GetEmpresaLogueada();
        }

        public Dictionary<string, object> GetTipoCambioPeru()
        {
            return OcDAO.GetTipoCambioPeru();
        }

        public List<dynamic> GetVoBos_SIGOPLANCOLOMBIA(OrdenCompraDTO objParamsDTO)
        {
            return OcDAO.GetVoBos_SIGOPLANCOLOMBIA(objParamsDTO);
        }

        public string getUsuarioEnKontrolCOLOMBIA(int numEmpleado)
        {
            return OcDAO.getUsuarioEnKontrolCOLOMBIA(numEmpleado);
        }

        public void UpdateOCFactura(List<RelFacturaDTO> data)
        {
            OcDAO.UpdateOCFactura(data);
        }
    }
}
