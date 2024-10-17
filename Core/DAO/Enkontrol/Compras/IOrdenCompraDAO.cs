using Core.DTO.Enkontrol.Alamcen;
using Core.DTO.Enkontrol.OrdenCompra;
using Core.DTO.Enkontrol.Requisicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Enkontrol.Compras
{
    public interface IOrdenCompraDAO
    {
        bool puedeVerCheckBoxProvNoOptimo();

        List<int> generaNuevaOC(List<GenOrdenCompraDTO> lstOC);
        /// <summary>
        /// Consulta si el usuario actual es comprador en Enkontrol
        /// </summary>
        /// <returns>Empleado y nombre del comprador</returns>
        dynamic getComprador();
        /// <summary>
        /// Busca las requisiciones autorizadas sin terminar según los parametro del filtro
        /// </summary>
        /// <param name="busq">objeto con los valores de busqueda</param>
        /// <returns>Listado de requisiciones</returns>
        dynamic busqReq(BusqReq busq);
        /// <summary>
        /// Busca los número máximos y mínimos de requisiciones autorizadas sin terminar
        /// </summary>
        /// <param name="busq">fechas y listado de CC a filtrar</param>
        /// <returns>Número de requisiciones mínima y máxima</returns>
        dynamic busqReqNum(BusqReq busq);
        /// <summary>
        /// Buscar partidas para generar ordenes de compra
        /// </summary>
        /// <param name="cc">Centro de costo o area cuenta de la partida</param>
        /// <param name="num">Número de la requisicion</param>
        /// <param name="moneda">ID moneda</param>
        /// <returns>Las partidas que tienen colocadas por ordenar</returns>
        dynamic getPartidas(string cc, int num, int moneda);
        /// <summary>
        /// Consulta de Centro de costos o área cuenta de compradores y requisicones a colocar
        /// </summary>
        /// <param name="busq">objeto con los valores de busqueda</param>
        /// <returns>Combobox de Centro de costos</returns>
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcReqComprador(BusqReq busq);
        /// <summary>
        /// Autocompletado de proveedor
        /// </summary>
        /// <param name="term">Número de proveedor</param>
        /// <returns>Listado de proveedores</returns>
        dynamic getProvFromNum(string term);
        /// <summary>
        /// Autocompletado de proveedor
        /// </summary>
        /// <param name="term">Nombre de proveedor</param>
        /// <returns>Listado de proveedores</returns>
        dynamic getProvFromNom(string term);

        OrdenCompraDTO getCompra(string cc, int num, bool esOC_INTERNA, string PERU_tipoCompra = "");
        dynamic getCompraRelacionar(string cc, int num);
        OrdenCompraDTO getCompra_Interna(string cc, int num);
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcComComprador();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcComCompradorModalEditar();
        OrdenCompraDTO updateCompra(OrdenCompraDTO compra);
        OrdenCompraDTO updateCompraInterna(OrdenCompraDTO compra);
        Dictionary<string, object> updateRetencionesCompra(List<OrdenCompraRetencionesDTO> retenciones);

        dynamic FillComboAreaCuentaTodas();

        List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcAut(bool isAuth);
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcAutTodas();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcFiltroPorUsuario();
        List<OrdenCompraDTO> getListaCompras(bool isAuth, string cc, bool propias);
        List<OrdenCompraDTO> getListaComprasTodas(string cc, bool pendientes, bool propias, int area, int cuenta);
        List<VoboDTO> getVobos(OrdenCompraDTO compra);
        List<Tuple<int, List<VoboDTO>>> getVobos(List<OrdenCompraDTO> compras, int empresa, bool esInterna);
        List<dynamic> getAutorizaciones(OrdenCompraDTO compra);
        List<Tuple<int, List<VoboDTO>>> getAutorizaciones(List<OrdenCompraDTO> compras, int empresa, bool esInterna);
        void autorizarCompra (string cc, int numero, bool isAut, bool esOC_Interna);
        void desautorizarCompra(string cc, int numero);
        dynamic getUsuarioEnKontrol(int numEmpleado);
        void voboCompra(OrdenCompraDTO compra, string voboNumero, List<VoboDTO> vobos, bool esOC_Interna);
        bool VerificarOC(OrdenCompraDTO objParamsDTO);
        bool getFlagPuedeDarVobo(List<int> numUsuariosEnkontrol, OrdenCompraDTO compra);
        List<OrdenCompraDesautorizacionDTO> getListaComprasDes(string cc);
        List<entradasAlmacenDTO> guardarSurtido(OrdenCompraDTO compra, List<SurtidoCompraDTO> surtido);
        List<entradasAlmacenDTO> guardarSurtidoNoInventariable(OrdenCompraDTO compra, List<SurtidoCompraDTO> surtido);
        List<CuadroComparativoDTO> buscarCuadros(BusquedaCuadroDTO filtros);
        dynamic requisicionesNumeros(string cc);
        CuadroComparativoDTO getCuadroDet(CuadroComparativoDTO cuadro);
        string getLABFromNum(int num);
        InfoProveedorMonedaDTO getProveedorInfo(string num, string PERU_tipoCambio);
        Dictionary<string, object> GuardarNuevoCuadro(CuadroComparativoDTO nuevoCuadro);
        Dictionary<string, object> UpdateCuadro(CuadroComparativoDTO cuadro);
        Dictionary<string, object> BorrarCuadro(CuadroComparativoDTO cuadro);
        Dictionary<string, object> guardarNuevaCompra(OrdenCompraDTO compra);
        Dictionary<string, object> guardarNuevaCompraInterna(OrdenCompraDTO compra);
        OrdenCompraDTO getRequisicion(string cc, int num);
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoReq();
        string getNombreUsuarioEmpleado(int numEmpleado);
        string getNombreAlmacen(int numAlmacen);
        RetencionInfoDTO getRetencionInfo(int id_cpto);
        dynamic getProveedorNumero(string proveedor);
        dynamic getCompradorNumero(string comprador);
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcAsigComp();
        Dictionary<string, object> ObtenerComprasPendientes(string cc, int estatus, int proveedor, DateTime fechaInicial, DateTime fechaFinal, string idAreaCuenta, int idCompradorEK);
        Dictionary<string, object> ObtenerComprasSinFactura(string cc, int estatus, int proveedor, DateTime fechaInicial, DateTime fechaFinal, string idAreaCuenta, int idCompradorEK);
        UltimaCompraDTO getUltimaCompra(CuadroComparativoDetDTO partidaCuadro);
        rptOrdenCompraInfoDTO getOrdenCompraRpt(string cc, int numero, string PERU_tipoCompra);
        rptOrdenCompraInfoDTO getOrdenCompraInternaRpt(string cc, int numero);
        Dictionary<string, object> puedeCancelar();
        Dictionary<string, object> cancelarCompra(string cc, int numero);
        void CancelarComprasMasivo();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboCc();
        List<ComboFamiliaInsumoDTO> FillComboFamiliasInsumos();
        Dictionary<string, object> getRequisicionesValidadas(List<string> listCC, List<string> listFamiliasInsumos, List<string> listCompradores, DateTime fechaInicio, DateTime fechaFin, int area, int cuenta, string noEconomico);
        Dictionary<string, object> cancelarParcialCompra(OrdenCompraDTO compra);
        Dictionary<string, object> getPreciosPorProveedor(string cc, int numeroRequisicion, long numeroProveedor);
        Dictionary<string, object> checkEstatusOrdenCompraImpresa(string cc, int numero, string PERU_tipoCompra = "");
        Dictionary<string, object> checkEstatusOrdenCompraImpresaConsulta(string cc, int numero);
        Dictionary<string, object> getContadorRequisicionesPendientes();
        void borrarCompra(string cc, int numero, bool autorizante);
        void BorrarComprasMasivo();
        CuadroComparativoReporteDTO getCuadroReporte(string cc, int numero, int folio);
        bool checarUbicacionesValidas(OrdenCompraDTO compra, List<SurtidoCompraDTO> entradas);
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboCompradores();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboCompradores(string cc);
        List<RetencionInfoDTO> getCatalogoRetenciones();
        MovimientoEnkontrolDTO getMovimientoNoInv(int almacenID, int remision);
        dynamic getPresupuestoCC(string cc);
        dynamic getPeriodoContable();

        /// <summary>
        /// Obtiene los datos necesarios para un generar un reporte sobre una entrada de una orden de compra.
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="num"></param>
        /// <param name="numMovimiento"></param>
        /// <returns></returns>
        List<entradasAlmacenDTO> GetDatosReporteEntradaOC(string cc, int? num, long numMovimiento);
        List<entradasAlmacenDTO> GetDatosReporteEntradaNoInvOC(string cc, int? num, long numMovimiento);
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboProveedores();
        dynamic getEntradas(string cc, int numero);
        void actualizarColocadaFechaYProveedor();
        void actualizarImpresa();
        void enviarOCProv(string cc, int numero, string correo, HttpPostedFileBase cotizacion);
        Dictionary<string, object> auditoriaEliminarReqOC();
        Dictionary<string, object> getAuditoriaRequisicionesComprasAfectadas();
        Dictionary<string, object> getComprasProveedor(string cc, DateTime fechaInicio, DateTime fechaFin, int proveedor, int area, int cuenta);
        Dictionary<string, object> getProveedoresCC(string cc, DateTime fechaInicio, DateTime fechaFin);
        Dictionary<string, object> getAreasCuentasCCFechaProveedor(string cc, DateTime fechaInicio, DateTime fechaFin, int proveedor);

        #region Esconder boton enviar correo si no son compradores
        bool usuarioCompradorExiste();
        #endregion
        List<trazabilidadDTO> getTrazabilidadGeneral(trazabilidad_filtrosDTO filtro);
        List<trazabilidadDTO> getTrazabilidadGeneralv2(trazabilidad_filtrosDTO filtro);
        void actualizarComprasDesautorizadas();
        Dictionary<string, object> GetProveedoresInsumos(List<string> listaInsumos);

        List<EmpleadoPendienteLiberacionDTO> getEmpleadosPendientesLiberacion();
        void guardarBajas(List<EmpleadoPendienteLiberacionDTO> empleados);

        #region GENERAR LINK
        Dictionary<string, object> FillCboProveedoresGenerarLink();

        Dictionary<string, object> CEProveedorLink(ProveedorLinkDTO objDTO);

        Dictionary<string, object> EliminarProveedorLink(ProveedorLinkDTO objDTO);

        Dictionary<string, object> GetProveedoresLink(ProveedorLinkDTO objDTO);

        Dictionary<string, object> FillCboProveedoresGenerarLinkRegistrados(ProveedorLinkDTO objDTO);

        Dictionary<string, object> EnviarCorreoLinkProveedores(ProveedorLinkDTO objDTO);

        Dictionary<string, object> IndicarEnvioCorreoExternamente(ProveedorLinkDTO objDTO);
        #endregion

        Dictionary<string, object> ImpresionMasivaCompras();
        Dictionary<string, object> FillComboFormaPagoPeru();
        Dictionary<string, object> FillComboTipoDocumentoPeru();
        Dictionary<string, object> RegistrarVoBoAutorizarOrdenCompra(List<OrdenCompraDTO> listaVobos, List<OrdenCompraDTO> listaAutorizados);
        Dictionary<string, object> GetEmpresaLogueada();
        Dictionary<string, object> GetTipoCambioPeru();
        List<dynamic> GetVoBos_SIGOPLANCOLOMBIA(OrdenCompraDTO objParamsDTO);
        string getUsuarioEnKontrolCOLOMBIA(int numEmpleado);
        void UpdateOCFactura(List<RelFacturaDTO> data);
    }
}