using Core.DTO.Almacen;
using Core.DTO.Enkontrol;
using Core.DTO.Enkontrol.Alamcen;
using Core.DTO.Enkontrol.Requisicion;
using Core.DTO.Enkontrol.Tablas;
using Core.DTO.Principal.Generales;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.Entity.Enkontrol.Compras.OrdenCompra;
using Core.DTO.Enkontrol.Tablas.Almacen;
using Core.Enum.Enkontrol.Compras;
using Core.Entity.Enkontrol.Compras;
using Core.DTO.Utils.DataTable;

namespace Core.DAO.Enkontrol.Almacen
{
    public interface IAlmacenDAO
    {
        List<ComboDTO> FillComboCC();
        List<ComboDTO> FillComboCCTodos();

        List<ValuacionDTO> getInsumos(int almacenID, string cc);
        bool guardarTraspasos(string ccOrigen, int almacenOrigen, string ccDestino, int almacenDestino, string comentarios, List<ValuacionDTO> insumos);
        Dictionary<string, object> getTraspasosPendientes(string ccOrigen, int almacenOrigen, string ccDestino, int almacenDestino, int folioInterno);
        Dictionary<string, object> getTraspasosPendientesOrigen(string ccOrigen, int almacenOrigen, string ccDestino, int almacenDestino, int numeroRequisicion);
        Dictionary<string, object> getTraspasosRechazados(string ccOrigen, int almacenOrigen, string ccDestino, int almacenDestino, int folioInterno);
        Dictionary<string, object> guardarAutorizacionesTraspasos(List<TraspasoDTO> listaAutorizados, bool excelCargado, bool esDirecto = false);
        Dictionary<string, object> guardarAutorizacionesTraspasosOrigen(List<TraspasoDTO> listaAutorizados);
        Dictionary<string, object> getInformacionInsumo(FiltrosExistenciaInsumoDTO filtros);
        Dictionary<string, object> getInsumosCatalogo(DataTablesParam param, InsumoCatalogoDTO filtros);
        Dictionary<string, object> getInformacionInsumoCatalogo(int insumo);
        Dictionary<string, object> guardarNuevoInsumo(InsumoCatalogoDTO insumo);
        Dictionary<string, object> getTipoGrupo(int tipo, int grupo);
        Dictionary<string, object> GetTipoInsumoPeru(int tipo);
        Dictionary<string, object> FillComboTipoInsumoPeru();
        Dictionary<string, object> FillComboUnidadPeru();
        dynamic getUnidades(string term);
        MemoryStream crearExcelInsumos();
        Dictionary<string, object> cargarExcel(HttpFileCollectionBase archivos);
        Dictionary<string, object> getNuevaDevolucionEntrada(int almacenID);
        dynamic getDevolucionEntrada(int almacenID, int numero);
        Dictionary<string, object> getCentroCosto(string cc);
        List<entradasAlmacenDTO> guardarDevolucionEntrada(MovimientoEnkontrolDTO movimiento);
        Dictionary<string, object> getNuevaDevolucionSalida(int almacenID);
        dynamic getDevolucionSalida(int almacenID, int numero);
        List<salidasAlmacenDTO> guardarDevolucionSalida(MovimientoEnkontrolDTO movimiento);
        dynamic getEntradasCompra(int almacen, string cc, int numeroOrdenCompra);
        Dictionary<string, object> getNuevaEntradaInventarioFisico(int almacenID);
        dynamic getEntradaInventarioFisico(int almacenID, int numero);
        List<entradasAlmacenDTO> guardarEntradaInventarioFisico(MovimientoEnkontrolDTO movimiento);
        List<entradasAlmacenDTO> getReporteEntradaFisico(int almacen, int numero);
        Dictionary<string, object> getNuevaSalidaInventarioFisico(int almacenID);
        dynamic getSalidaInventarioFisico(int almacenID, int numero);
        List<salidasAlmacenDTO> guardarSalidaInventarioFisico(MovimientoEnkontrolDTO movimiento, bool movimientoMasivo);
        List<salidasAlmacenDTO> ImprimirMovimientoSalidaInventarioFisico(int almacen, int numero);
        Dictionary<string, object> CargarExcelSalidaFisico(HttpFileCollectionBase archivos);
        bool checarUbicacionesValidas(List<MovimientoDetalleEnkontrolDTO> entradas);
        Dictionary<string, object> getNuevaSalidaConsumo(int almacenID);
        dynamic getSalidaConsumo(int almacenID, int numero);
        Dictionary<string, object> guardarSalidaConsumo(MovimientoEnkontrolDTO movimiento);
        Dictionary<string, object> guardarSalidaConsumoOrigen(MovimientoEnkontrolDTO movimiento);
        List<HistorialInsumoDTO> getHistorialInsumo(int almacen, int insumo);
        List<EmpleadoPendienteLiberacionDTO> getEmpleadosPendientesLiberacion();
        void guardarBajas(List<EmpleadoPendienteLiberacionDTO> empleados);
        List<HistorialInsumoDTO> getCatalogoUbicaciones(int almacenID);
        List<ComboDTO> FillComboTipoMovimiento();
        MovimientoEnkontrolDTO cargarMovimiento(int almacen, int tipo_mov, int numero);
        dynamic checarAccesoAlmacenista(int almacen);
        List<salidasAlmacenDTO> imprimirMovimientoSalidaConsumo(int almacen, int numero);
        List<salidasAlmacenDTO> imprimirMovimientoSalidaDevolucion(int almacen, int numero);
        void insertTraspasosPendientes();
        List<UbicacionDetalleDTO> getExistencias(int empresa, int insumo, int almacen);
        bool cargarExcelTraspasoMasivo(HttpPostedFileBase archivo);
        dynamic checkPermisoTraspasoMasivo();
        List<salidasAlmacenDTO> imprimirMovimientoEntradaTraspaso(int almacen, int numero);
        List<salidasAlmacenDTO> imprimirMovimientoSalidaTraspaso(int almacen, int numero);
        List<CentroCostoDTO> getCentrosCosto();
        Dictionary<string, object> getNuevaSalidaConsultaTraspaso(int almacenID);
        Dictionary<string, object> getNuevaEntradaConsultaTraspaso(int almacenID);
        Tuple<dynamic, List<salidasAlmacenDTO>> getSalidaConsultaTraspaso(int almacenID, int numero);
        Tuple<dynamic, List<salidasAlmacenDTO>> getEntradaConsultaTraspaso(int almacenID, int numero);
        bool checkMaquinaStandBy(string cc);
        bool checarPermisosFamilias(int almacen, List<int> insumos);
        bool checarPermisoAreaCuenta();
        List<RequisicionDTO> ObtenerComprasPendientes(List<string> listaCC, int estatus, List<int> listaAlmacenes, DateTime fechaInicio, DateTime fechaFin);
        bool corregirUbicacionesSalidas();

        #region ABD de Almacén
        List<si_almacenDTO> GetAlmacenes();
        List<MovimientosDTO> ObtenerMovimientos();
        Dictionary<string, object> GuardarAlmacen(si_almacenDTO datos);
        #endregion
        Dictionary<string, object> getUsuarioEnkontrolByID(int empleado);

        #region Inventario Físico
        Dictionary<string, object> cargarExistenciasAlmacen(int almacen, DateTime fecha, bool existentes);
        Dictionary<string, object> cargarExistenciasAlmacen(int almacen, int insumo, bool existentes);
        Dictionary<string, object> cargarInventarioFisico(int almacen, DateTime fecha);
        Dictionary<string, object> cargarInventarioFisico(string cc, int almacen, DateTime fecha, int insumoInicial, int insumoFin, bool soloConDiferencia);
        Dictionary<string, object> cargarIntervaloInsumos();
        Dictionary<string, object> guardarInventarioFisico(List<FisicoDetalleDTO> partidas);
        Dictionary<string, object> CargarDescripcionAlmacen(int almacen);
        Dictionary<string, object> CargarDescripcionCC(string cc);
        Tuple<string, List<ReporteInventarioFisicoDTO>> cargarExistenciasAlmacenReporte(int almacen, DateTime fecha, bool existentes);
        Dictionary<string, object> eliminarPartidaInventarioFisico(FisicoDetalleDTO partida);
        Dictionary<string, object> congelarAlmacenInventarioFisico(int almacen, DateTime fecha);
        Dictionary<string, object> cerrarInventarioFisico(int almacen, DateTime fecha);
        tblAlm_PermisoCierreInventario getPermisoCierreInventario();
        #endregion
        bool EliminarAlmacen(int almacen);

        Dictionary<string, object> ObtenerAlmacenEditaroAgregar(int almacen);
        bool EditarAlmacen(si_almacenDTO datos);
        List<obtenerExistenciasDTO> obtenerExistenciasvsInventario(int almacen, DateTime fecha, bool existentes, bool ultimoPrecio, int insumoInicio, int insumoFin, bool soloConDiferencia);

        si_almacenDTO ObtenerAlmacenID(int almacen);
        Core.DTO.Almacen.InsumoDTO consultarPrimerInsumo(int insumo);
        Core.DTO.Almacen.InsumoDTO consultarUltimoInsumo(int insumo);
        List<Core.DTO.Almacen.InsumoDTO> Obtenerinsumos(int Almacen, int pagina, int registros);

        Dictionary<string, object> ActualizacionUbicacionInsumo(int almacen, int insumo, string cc, tblAlm_Movimientos movimiento, List<tblAlm_MovimientosDet> detallesMovimientoSalida, List<tblAlm_MovimientosDet> detallesMovimientoEntrada);


        List<AreaAlmacenDTO> getAreaAlmacen(string AreaCuenta);
        AreaAlmacenDTO GuardarEditarAreaAlmacen(AreaAlmacenDTO parametros);
        AreaAlmacenDTO EliminarAreaAlmacen(int id);
        List<ComboDTO> getAlmacenesAreaDisponibles(int idRelacion);
        List<ComboDTO> getAreaCuentas(int idRelacion);
        List<ComboDTO> getTodasAreaCuentas();
        Dictionary<string, object> getDetalleAreaAlmacen(int idRelacion);
        RequisicionCompraConciliacionDTO crearRequisicionCompraConciliacion(string cc, string comentario, TipoCentroCostoEnum tipoCC, decimal precio, int tipoMoneda, decimal precioDolar, decimal porcentajeIVA);

        Dictionary<string, object> CargarExcelSalidaTraspaso(HttpFileCollectionBase archivos);

        #region Remanentes
        Dictionary<string, object> CargarRemanentes(List<int> listaAlmacenes, DateTime fechaInicio, DateTime fechaFin, int solicitante);
        List<ComboDTO> FillComboAlmacenesFisicos();
        Dictionary<string, object> EliminarRegistroRemanente(int remanente_id);
        #endregion

        #region Catálogo Ubicaciones
        Dictionary<string, object> GetUbicaciones();
        Dictionary<string, object> GuardarNuevaUbicacion(tblAlm_Ubicacion ubicacion);
        Dictionary<string, object> EditarUbicacion(tblAlm_Ubicacion ubicacion);
        Dictionary<string, object> EliminarUbicacion(tblAlm_Ubicacion ubicacion);
        #endregion
    }
}
