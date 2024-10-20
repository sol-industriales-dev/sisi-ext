using Core.DAO.Enkontrol.Almacen;
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

namespace Core.Service.Enkontrol.Almacen
{
    public class AlmacenService : IAlmacenDAO
    {
        public IAlmacenDAO e_almDAO;
        public IAlmacenDAO AlmDAO
        {
            get { return e_almDAO; }
            set { e_almDAO = value; }
        }
        public AlmacenService(IAlmacenDAO almDAO)
        {
            this.AlmDAO = almDAO;
        }

        public List<ComboDTO> FillComboCC()
        {
            return AlmDAO.FillComboCC();
        }
        public List<ComboDTO> FillComboCCTodos()
        {
            return AlmDAO.FillComboCCTodos();
        }

        public List<ValuacionDTO> getInsumos(int almacenID, string cc)
        {
            return AlmDAO.getInsumos(almacenID, cc);
        }

        public bool guardarTraspasos(string ccOrigen, int almacenOrigen, string ccDestino, int almacenDestino, string comentarios, List<ValuacionDTO> insumos)
        {
            return AlmDAO.guardarTraspasos(ccOrigen, almacenOrigen, ccDestino, almacenDestino, comentarios, insumos);
        }

        public Dictionary<string, object> getTraspasosPendientes(string ccOrigen, int almacenOrigen, string ccDestino, int almacenDestino, int folioInterno)
        {
            return AlmDAO.getTraspasosPendientes(ccOrigen, almacenOrigen, ccDestino, almacenDestino, folioInterno);
        }

        public Dictionary<string, object> getTraspasosPendientesOrigen(string ccOrigen, int almacenOrigen, string ccDestino, int almacenDestino, int numeroRequisicion)
        {
            return AlmDAO.getTraspasosPendientesOrigen(ccOrigen, almacenOrigen, ccDestino, almacenDestino, numeroRequisicion);
        }

        public Dictionary<string, object> getTraspasosRechazados(string ccOrigen, int almacenOrigen, string ccDestino, int almacenDestino, int folioInterno)
        {
            return AlmDAO.getTraspasosRechazados(ccOrigen, almacenOrigen, ccDestino, almacenDestino, folioInterno);
        }

        public Dictionary<string, object> guardarAutorizacionesTraspasos(List<TraspasoDTO> listaAutorizados, bool excelCargado, bool esDirecto = false)
        {
            return AlmDAO.guardarAutorizacionesTraspasos(listaAutorizados, excelCargado, esDirecto);
        }

        public Dictionary<string, object> guardarAutorizacionesTraspasosOrigen(List<TraspasoDTO> listaAutorizados)
        {
            return AlmDAO.guardarAutorizacionesTraspasosOrigen(listaAutorizados);
        }

        public Dictionary<string, object> getInformacionInsumo(FiltrosExistenciaInsumoDTO filtros)
        {
            return AlmDAO.getInformacionInsumo(filtros);
        }

        public Dictionary<string, object> getInsumosCatalogo(DataTablesParam param, InsumoCatalogoDTO filtros)
        {
            return AlmDAO.getInsumosCatalogo(param, filtros);
        }

        public Dictionary<string, object> getInformacionInsumoCatalogo(int insumo)
        {
            return AlmDAO.getInformacionInsumoCatalogo(insumo);
        }

        public Dictionary<string, object> guardarNuevoInsumo(InsumoCatalogoDTO insumo)
        {
            return AlmDAO.guardarNuevoInsumo(insumo);
        }

        public Dictionary<string, object> getTipoGrupo(int tipo, int grupo)
        {
            return AlmDAO.getTipoGrupo(tipo, grupo);
        }

        public Dictionary<string, object> GetTipoInsumoPeru(int tipo)
        {
            return AlmDAO.GetTipoInsumoPeru(tipo);
        }

        public Dictionary<string, object> FillComboTipoInsumoPeru()
        {
            return AlmDAO.FillComboTipoInsumoPeru();
        }

        public Dictionary<string, object> FillComboUnidadPeru()
        {
            return AlmDAO.FillComboUnidadPeru();
        }

        public dynamic getUnidades(string term)
        {
            return AlmDAO.getUnidades(term);
        }

        public MemoryStream crearExcelInsumos()
        {
            return AlmDAO.crearExcelInsumos();
        }

        public Dictionary<string, object> cargarExcel(HttpFileCollectionBase archivos)
        {
            return AlmDAO.cargarExcel(archivos);
        }

        public Dictionary<string, object> getNuevaDevolucionEntrada(int almacenID)
        {
            return AlmDAO.getNuevaDevolucionEntrada(almacenID);
        }

        public dynamic getDevolucionEntrada(int almacenID, int numero)
        {
            return AlmDAO.getDevolucionEntrada(almacenID, numero);
        }

        public Dictionary<string, object> getCentroCosto(string cc)
        {
            return AlmDAO.getCentroCosto(cc);
        }

        public List<entradasAlmacenDTO> guardarDevolucionEntrada(MovimientoEnkontrolDTO movimiento)
        {
            return AlmDAO.guardarDevolucionEntrada(movimiento);
        }

        public Dictionary<string, object> getNuevaDevolucionSalida(int almacenID)
        {
            return AlmDAO.getNuevaDevolucionSalida(almacenID);
        }

        public dynamic getDevolucionSalida(int almacenID, int numero)
        {
            return AlmDAO.getDevolucionSalida(almacenID, numero);
        }

        public List<salidasAlmacenDTO> guardarDevolucionSalida(MovimientoEnkontrolDTO movimiento)
        {
            return AlmDAO.guardarDevolucionSalida(movimiento);
        }

        public dynamic getEntradasCompra(int almacen, string cc, int numeroOrdenCompra)
        {
            return AlmDAO.getEntradasCompra(almacen, cc, numeroOrdenCompra);
        }

        public Dictionary<string, object> getNuevaEntradaInventarioFisico(int almacenID)
        {
            return AlmDAO.getNuevaEntradaInventarioFisico(almacenID);
        }

        public dynamic getEntradaInventarioFisico(int almacenID, int numero)
        {
            return AlmDAO.getEntradaInventarioFisico(almacenID, numero);
        }

        public List<entradasAlmacenDTO> guardarEntradaInventarioFisico(MovimientoEnkontrolDTO movimiento)
        {
            return AlmDAO.guardarEntradaInventarioFisico(movimiento);
        }

        public List<entradasAlmacenDTO> getReporteEntradaFisico(int almacen, int numero)
        {
            return AlmDAO.getReporteEntradaFisico(almacen, numero);
        }

        public Dictionary<string, object> getNuevaSalidaInventarioFisico(int almacenID)
        {
            return AlmDAO.getNuevaSalidaInventarioFisico(almacenID);
        }

        public dynamic getSalidaInventarioFisico(int almacenID, int numero)
        {
            return AlmDAO.getSalidaInventarioFisico(almacenID, numero);
        }

        public List<salidasAlmacenDTO> guardarSalidaInventarioFisico(MovimientoEnkontrolDTO movimiento, bool movimientoMasivo)
        {
            return AlmDAO.guardarSalidaInventarioFisico(movimiento, movimientoMasivo);
        }

        public List<salidasAlmacenDTO> ImprimirMovimientoSalidaInventarioFisico(int almacen, int numero)
        {
            return AlmDAO.ImprimirMovimientoSalidaInventarioFisico(almacen, numero);
        }

        public Dictionary<string, object> CargarExcelSalidaFisico(HttpFileCollectionBase archivos)
        {
            return AlmDAO.CargarExcelSalidaFisico(archivos);
        }

        public bool checarUbicacionesValidas(List<MovimientoDetalleEnkontrolDTO> entradas)
        {
            return AlmDAO.checarUbicacionesValidas(entradas);
        }

        public Dictionary<string, object> getNuevaSalidaConsumo(int almacenID)
        {
            return AlmDAO.getNuevaSalidaConsumo(almacenID);
        }

        public dynamic getSalidaConsumo(int almacenID, int numero)
        {
            return AlmDAO.getSalidaConsumo(almacenID, numero);
        }

        public Dictionary<string, object> guardarSalidaConsumo(MovimientoEnkontrolDTO movimiento)
        {
            return AlmDAO.guardarSalidaConsumo(movimiento);
        }

        public Dictionary<string, object> guardarSalidaConsumoOrigen(MovimientoEnkontrolDTO movimiento)
        {
            return AlmDAO.guardarSalidaConsumoOrigen(movimiento);
        }

        public List<HistorialInsumoDTO> getHistorialInsumo(int almacen, int insumo)
        {
            return AlmDAO.getHistorialInsumo(almacen, insumo);
        }

        public List<EmpleadoPendienteLiberacionDTO> getEmpleadosPendientesLiberacion()
        {
            return AlmDAO.getEmpleadosPendientesLiberacion();
        }

        public void guardarBajas(List<EmpleadoPendienteLiberacionDTO> empleados)
        {
            AlmDAO.guardarBajas(empleados);
        }

        public List<HistorialInsumoDTO> getCatalogoUbicaciones(int almacenID)
        {
            return AlmDAO.getCatalogoUbicaciones(almacenID);
        }

        public List<ComboDTO> FillComboTipoMovimiento()
        {
            return AlmDAO.FillComboTipoMovimiento();
        }

        public MovimientoEnkontrolDTO cargarMovimiento(int almacen, int tipo_mov, int numero)
        {
            return AlmDAO.cargarMovimiento(almacen, tipo_mov, numero);
        }

        public dynamic checarAccesoAlmacenista(int almacen)
        {
            return AlmDAO.checarAccesoAlmacenista(almacen);
        }

        public List<salidasAlmacenDTO> imprimirMovimientoSalidaConsumo(int almacen, int numero)
        {
            return AlmDAO.imprimirMovimientoSalidaConsumo(almacen, numero);
        }

        public List<salidasAlmacenDTO> imprimirMovimientoSalidaDevolucion(int almacen, int numero)
        {
            return AlmDAO.imprimirMovimientoSalidaDevolucion(almacen, numero);
        }

        public void insertTraspasosPendientes()
        {
            AlmDAO.insertTraspasosPendientes();
        }

        public List<UbicacionDetalleDTO> getExistencias(int empresa, int insumo, int almacen)
        {
            return AlmDAO.getExistencias(empresa, insumo, almacen);
        }

        public bool cargarExcelTraspasoMasivo(HttpPostedFileBase archivo)
        {
            return AlmDAO.cargarExcelTraspasoMasivo(archivo);
        }

        public dynamic checkPermisoTraspasoMasivo()
        {
            return AlmDAO.checkPermisoTraspasoMasivo();
        }

        public List<salidasAlmacenDTO> imprimirMovimientoEntradaTraspaso(int almacen, int numero)
        {
            return AlmDAO.imprimirMovimientoEntradaTraspaso(almacen, numero);
        }

        public List<salidasAlmacenDTO> imprimirMovimientoSalidaTraspaso(int almacen, int numero)
        {
            return AlmDAO.imprimirMovimientoSalidaTraspaso(almacen, numero);
        }

        public List<CentroCostoDTO> getCentrosCosto()
        {
            return AlmDAO.getCentrosCosto();
        }

        public Dictionary<string, object> getNuevaSalidaConsultaTraspaso(int almacenID)
        {
            return AlmDAO.getNuevaSalidaConsultaTraspaso(almacenID);
        }

        public Dictionary<string, object> getNuevaEntradaConsultaTraspaso(int almacenID)
        {
            return AlmDAO.getNuevaEntradaConsultaTraspaso(almacenID);
        }

        public Tuple<dynamic, List<salidasAlmacenDTO>> getSalidaConsultaTraspaso(int almacenID, int numero)
        {
            return AlmDAO.getSalidaConsultaTraspaso(almacenID, numero);
        }

        public Tuple<dynamic, List<salidasAlmacenDTO>> getEntradaConsultaTraspaso(int almacenID, int numero)
        {
            return AlmDAO.getEntradaConsultaTraspaso(almacenID, numero);
        }

        public bool checkMaquinaStandBy(string cc)
        {
            return AlmDAO.checkMaquinaStandBy(cc);
        }

        public bool checarPermisosFamilias(int almacen, List<int> insumos)
        {
            return AlmDAO.checarPermisosFamilias(almacen, insumos);
        }

        public bool checarPermisoAreaCuenta()
        {
            return AlmDAO.checarPermisoAreaCuenta();
        }

        public List<RequisicionDTO> ObtenerComprasPendientes(List<string> listaCC, int estatus, List<int> listaAlmacenes, DateTime fechaInicio, DateTime fechaFin)
        {
            return AlmDAO.ObtenerComprasPendientes(listaCC, estatus, listaAlmacenes, fechaInicio, fechaFin);
        }

        public bool corregirUbicacionesSalidas()
        {
            return AlmDAO.corregirUbicacionesSalidas();
        }

        public Dictionary<string, object> getUsuarioEnkontrolByID(int empleado)
        {
            return AlmDAO.getUsuarioEnkontrolByID(empleado);
        }

        #region ABC de Almacén
        public List<si_almacenDTO> GetAlmacenes()
        {
            return AlmDAO.GetAlmacenes();
        }
        public List<MovimientosDTO> ObtenerMovimientos()
        {
            return AlmDAO.ObtenerMovimientos();
        }
        public bool EliminarAlmacen(int almacen)
        {
            return AlmDAO.EliminarAlmacen(almacen);
        }

        public Dictionary<string, object> GuardarAlmacen(si_almacenDTO datos)
        {
            return AlmDAO.GuardarAlmacen(datos);
        }

        public Dictionary<string, object> ObtenerAlmacenEditaroAgregar(int almacen)
        {
            return AlmDAO.ObtenerAlmacenEditaroAgregar(almacen);

        }
        public bool EditarAlmacen(si_almacenDTO datos)
        {
            return AlmDAO.EditarAlmacen(datos);
        }
        #endregion

        #region Inventario Físico
        public Dictionary<string, object> cargarExistenciasAlmacen(int almacen, DateTime fecha, bool existentes)
        {
            return AlmDAO.cargarExistenciasAlmacen(almacen, fecha, existentes);
        }

        public Dictionary<string, object> cargarExistenciasAlmacen(int almacen, int insumo, bool existentes)
        {
            return AlmDAO.cargarExistenciasAlmacen(almacen, insumo, existentes);
        }

        public Dictionary<string, object> cargarInventarioFisico(int almacen, DateTime fecha)
        {
            return AlmDAO.cargarInventarioFisico(almacen, fecha);
        }

        public Dictionary<string, object> cargarInventarioFisico(string cc, int almacen, DateTime fecha, int insumoInicial, int insumoFin, bool soloConDiferencia)
        {
            return AlmDAO.cargarInventarioFisico(cc, almacen, fecha, insumoInicial, insumoFin, soloConDiferencia);
        }

        public Dictionary<string, object> cargarIntervaloInsumos()
        {
            return AlmDAO.cargarIntervaloInsumos();
        }

        public Dictionary<string, object> guardarInventarioFisico(List<FisicoDetalleDTO> partidas)
        {
            return AlmDAO.guardarInventarioFisico(partidas);
        }

        public Dictionary<string, object> CargarDescripcionAlmacen(int almacen)
        {
            return AlmDAO.CargarDescripcionAlmacen(almacen);
        }

        public Dictionary<string, object> CargarDescripcionCC(string cc)
        {
            return AlmDAO.CargarDescripcionCC(cc);
        }

        public Tuple<string, List<ReporteInventarioFisicoDTO>> cargarExistenciasAlmacenReporte(int almacen, DateTime fecha, bool existentes)
        {
            return AlmDAO.cargarExistenciasAlmacenReporte(almacen, fecha, existentes);
        }

        public Dictionary<string, object> eliminarPartidaInventarioFisico(FisicoDetalleDTO partida)
        {
            return AlmDAO.eliminarPartidaInventarioFisico(partida);
        }

        public Dictionary<string, object> congelarAlmacenInventarioFisico(int almacen, DateTime fecha)
        {
            return AlmDAO.congelarAlmacenInventarioFisico(almacen, fecha);
        }

        public Dictionary<string, object> cerrarInventarioFisico(int almacen, DateTime fecha)
        {
            return AlmDAO.cerrarInventarioFisico(almacen, fecha);
        }

        public tblAlm_PermisoCierreInventario getPermisoCierreInventario()
        {
            return AlmDAO.getPermisoCierreInventario();
        }
        #endregion
        public List<obtenerExistenciasDTO> obtenerExistenciasvsInventario(int almacen, DateTime fecha, bool existentes, bool ultimoPrecio, int insumoInicio, int insumoFin, bool soloConDiferencia)
        {
            return AlmDAO.obtenerExistenciasvsInventario(almacen, fecha, existentes, ultimoPrecio, insumoInicio, insumoFin, soloConDiferencia);
        }
        public si_almacenDTO ObtenerAlmacenID(int almacen)
        {
            return AlmDAO.ObtenerAlmacenID(almacen);
        }
        public Core.DTO.Almacen.InsumoDTO consultarPrimerInsumo(int insumo)
        {
            return AlmDAO.consultarPrimerInsumo(insumo);
        }
        public Core.DTO.Almacen.InsumoDTO consultarUltimoInsumo(int insumo)
        {
            return AlmDAO.consultarUltimoInsumo(insumo);
        }
        public List<Core.DTO.Almacen.InsumoDTO> Obtenerinsumos(int Almacen, int pagina, int registros)
        {
            return AlmDAO.Obtenerinsumos(Almacen, pagina, registros);
        }

        public Dictionary<string, object> ActualizacionUbicacionInsumo(int almacen, int insumo, string cc, tblAlm_Movimientos movimiento, List<tblAlm_MovimientosDet> detallesMovimientoSalida, List<tblAlm_MovimientosDet> detallesMovimientoEntrada)
        {
            return AlmDAO.ActualizacionUbicacionInsumo(almacen, insumo, cc, movimiento, detallesMovimientoSalida, detallesMovimientoEntrada);
        }

        public List<AreaAlmacenDTO> getAreaAlmacen(string AreaCuenta)
        {
            return AlmDAO.getAreaAlmacen(AreaCuenta);
        }
        public AreaAlmacenDTO GuardarEditarAreaAlmacen(AreaAlmacenDTO parametros)
        {
            return AlmDAO.GuardarEditarAreaAlmacen(parametros);
        }
        public AreaAlmacenDTO EliminarAreaAlmacen(int id)
        {
            return AlmDAO.EliminarAreaAlmacen(id);
        }
        public List<ComboDTO> getAlmacenesAreaDisponibles(int idRelacion)
        {
            return AlmDAO.getAlmacenesAreaDisponibles(idRelacion);
        }
        public List<ComboDTO> getAreaCuentas(int idRelacion)
        {
            return AlmDAO.getAreaCuentas(idRelacion);
        }
        public List<ComboDTO> getTodasAreaCuentas()
        {
            return AlmDAO.getTodasAreaCuentas();
        }
        public Dictionary<string, object> getDetalleAreaAlmacen(int idRelacion)
        {
            return AlmDAO.getDetalleAreaAlmacen(idRelacion);
        }

        public RequisicionCompraConciliacionDTO crearRequisicionCompraConciliacion(string cc, string comentario, TipoCentroCostoEnum tipoCC, decimal precio, int tipoMoneda, decimal precioDolar, decimal porcentajeIVA)
        {
            return AlmDAO.crearRequisicionCompraConciliacion(cc, comentario, tipoCC, precio, tipoMoneda, precioDolar, porcentajeIVA);
        }

        public Dictionary<string, object> CargarExcelSalidaTraspaso(HttpFileCollectionBase archivos)
        {
            return AlmDAO.CargarExcelSalidaTraspaso(archivos);
        }

        #region Remanentes
        public Dictionary<string, object> CargarRemanentes(List<int> listaAlmacenes, DateTime fechaInicio, DateTime fechaFin, int solicitante)
        {
            return AlmDAO.CargarRemanentes(listaAlmacenes, fechaInicio, fechaFin, solicitante);
        }

        public List<ComboDTO> FillComboAlmacenesFisicos()
        {
            return AlmDAO.FillComboAlmacenesFisicos();
        }

        public Dictionary<string, object> EliminarRegistroRemanente(int remanente_id)
        {
            return AlmDAO.EliminarRegistroRemanente(remanente_id);
        }
        #endregion

        #region Catálogo Ubicaciones
        public Dictionary<string, object> GetUbicaciones()
        {
            return AlmDAO.GetUbicaciones();
        }

        public Dictionary<string, object> GuardarNuevaUbicacion(tblAlm_Ubicacion ubicacion)
        {
            return AlmDAO.GuardarNuevaUbicacion(ubicacion);
        }

        public Dictionary<string, object> EditarUbicacion(tblAlm_Ubicacion ubicacion)
        {
            return AlmDAO.EditarUbicacion(ubicacion);
        }

        public Dictionary<string, object> EliminarUbicacion(tblAlm_Ubicacion ubicacion)
        {
            return AlmDAO.EliminarUbicacion(ubicacion);
        }
        #endregion

        public List<tblAlm_Insumo_Tipo> FillGrid_InsumoTipo(tblAlm_Insumo_Tipo obj)
        {
            return AlmDAO.FillGrid_InsumoTipo(obj);
        }

        public void SaveOrUpdate_InsumoTipo(tblAlm_Insumo_Tipo obj)
        {
            AlmDAO.SaveOrUpdate_InsumoTipo(obj);
        }

        public List<tblAlm_Insumo_Grupo> FillGrid_InsumoGrupo(tblAlm_Insumo_Grupo obj)
        {
            return AlmDAO.FillGrid_InsumoGrupo(obj);
        }

        public void SaveOrUpdate_InsumoGrupo(tblAlm_Insumo_Grupo obj)
        {
            AlmDAO.SaveOrUpdate_InsumoGrupo(obj);
        }

        public List<tblAlm_Grupos_Insumo> FillGrid_InsumoFamilia(tblAlm_Grupos_Insumo obj)
        {
            return AlmDAO.FillGrid_InsumoFamilia(obj);
        }

        public void SaveOrUpdate_InsumoFamilia(tblAlm_Grupos_Insumo obj)
        {
            AlmDAO.SaveOrUpdate_InsumoFamilia(obj);
        }

        public List<tblAlm_Insumo_Tipo> FillCboInsumoTipo(bool estatus)
        {
            return AlmDAO.FillCboInsumoTipo(estatus);
        }

        public List<tblAlm_Insumo_Grupo> FillCboInsumoGrupo(bool estatus)
        {
            return AlmDAO.FillCboInsumoGrupo(estatus);
        }

        public List<tblAlm_Insumo> FillGrid_Insumo(tblAlm_Insumo obj)
        {
            return AlmDAO.FillGrid_Insumo(obj);
        }

        public void SaveOrUpdate_Insumo(tblAlm_Insumo obj)
        {
            AlmDAO.SaveOrUpdate_Insumo(obj);
        }
    }
}
