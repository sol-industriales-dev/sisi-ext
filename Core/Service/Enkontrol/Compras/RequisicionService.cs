using Core.DAO.Enkontrol.Compras;
using Core.DTO.Enkontrol.Requisicion;
using Core.DTO.Enkontrol.Alamcen;
using Core.Entity.Enkontrol.Compras.Requisicion;
using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Enkontrol.Compras;
using System.Web;
using Core.Enum.Enkontrol.Requisicion;
using Core.DTO.Utils.DataTable;
using System.IO;

namespace Core.Service.Enkontrol.Compras
{
    public class RequisicionService : IRequisicionDAO
    {
        #region Atributros
        public IRequisicionDAO e_reqDAO;
        #endregion
        #region Propiedades
        public IRequisicionDAO ReqDAO
        {
            get { return e_reqDAO; }
            set { e_reqDAO = value; }
        }
        #endregion
        #region Constructores
        public RequisicionService(IRequisicionDAO reqDAO)
        {
            this.ReqDAO = reqDAO;
        }
        #endregion
        public Dictionary<string, object> guardar(tblCom_Req req, List<tblCom_ReqDet> det, List<ReqDetalleComentarioDTO> comentarios)
        {
            return this.ReqDAO.guardar(req, det, comentarios);
        }
        public Dictionary<string, object> setAuth(List<tblCom_Req> lst)
        {
            return this.ReqDAO.setAuth(lst);
        }
        public Dictionary<string, object> getNewReq(string cc, string tpRequi)
        {
            return this.ReqDAO.getNewReq(cc, tpRequi);
        }
        public Dictionary<string, object> getRequisicion(string cc, int num, bool esServicio)
        {
            return this.ReqDAO.getRequisicion(cc, num, esServicio);
        }
        public Dictionary<string, object> getReq(string cc, int num)
        {
            return this.ReqDAO.getReq(cc, num);
        }
        public dynamic getInsumos(string term, string cc, bool esServicio)
        {
            return this.ReqDAO.getInsumos(term, cc, esServicio);
        }
        public dynamic getInsumosDesc(string term, string cc, bool esServicio)
        {
            return this.ReqDAO.getInsumosDesc(term, cc, esServicio);
        }
        public dynamic getInsumosByAlmacen(string term, string cc,int almacen)
        {
            return this.ReqDAO.getInsumosByAlmacen(term, cc,almacen);
        }
        public dynamic getInsumosDescByAlmacen(string term, string cc,int almacen)
        {
            return this.ReqDAO.getInsumosDescByAlmacen(term, cc,almacen);
        }
        public dynamic getInsumosByAlmacenEntrada(string term, string cc, int almacen)
        {
            return this.ReqDAO.getInsumosByAlmacenEntrada(term, cc, almacen);
        }
        public dynamic getInsumosDescByAlmacenEntrada(string term, string cc, int almacen)
        {
            return this.ReqDAO.getInsumosDescByAlmacenEntrada(term, cc, almacen);
        }
        public bool isEmpAdmin()
        {
            return this.ReqDAO.isEmpAdmin();
        }
        public dynamic getReq(bool isAuth, List<string> cc)
        {
            return this.ReqDAO.getReq(isAuth, cc);
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboLab()
        {
            return this.ReqDAO.FillComboLab();
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoReq()
        {
            return this.ReqDAO.FillComboTipoReq();
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTxtFolio(int tipo)
        {
            return this.ReqDAO.FillComboTxtFolio(tipo);
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboResponsablePorCc(string cc)
        {
            return this.ReqDAO.FillComboResponsablePorCc(cc);
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcReq(bool isAuth)
        {
            return this.ReqDAO.FillComboCcReq(isAuth);
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcAsigReq()
        {
            return this.ReqDAO.FillComboCcAsigReq();
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcTodos()
        {
            return this.ReqDAO.FillComboCcTodos();
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoFolio()
        {
            return this.ReqDAO.FillComboTipoFolio();
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboAlmacenSurtir()
        {
            return this.ReqDAO.FillComboAlmacenSurtir();
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboAlmacenSurtirAcceso()
        {
            return this.ReqDAO.FillComboAlmacenSurtirAcceso();
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboAlmacenSurtirTodos()
        {
            return this.ReqDAO.FillComboAlmacenSurtirTodos();
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoInsumo()
        {
            return this.ReqDAO.FillComboTipoInsumo();
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboProveedores()
        {
            return this.ReqDAO.FillComboProveedores();
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboProveedoresConsignaLicitacionConvenio(TipoConsignaLicitacionConvenioEnum tipo)
        {
            return this.ReqDAO.FillComboProveedoresConsignaLicitacionConvenio(tipo);
        }
        public dynamic FillComboAreaCuenta(string cc)
        {
            return this.ReqDAO.FillComboAreaCuenta(cc);
        }

        public dynamic FillComboTipoPartida(string cc)
        {
            return this.ReqDAO.FillComboTipoPartida(cc);
        }
        public dynamic getThisEmpleadoEnkontrol()
        {
            return this.ReqDAO.getThisEmpleadoEnkontrol();
        }
        public bool getFolio(string folio, int tipo)
        {
            return this.ReqDAO.getFolio(folio, tipo);
        }
        public string getFolioOrigen(string cc, int num)
        {
            return this.ReqDAO.getFolioOrigen(cc, num);
        }
        public RequisicionDTO getRequisicionSIGOPLAN(string cc, int num, string PERU_tipoRequisicion)
        {
            return this.ReqDAO.getRequisicionSIGOPLAN(cc, num, PERU_tipoRequisicion);
        }
        public Dictionary<string, object> getExistenciaInsumo(int insumo, string cc, int almacen)
        {
            return this.ReqDAO.getExistenciaInsumo(insumo, cc, almacen);
        }
        public Dictionary<string, object> getExistenciaInsumoDetalle(int insumo)
        {
            return this.ReqDAO.getExistenciaInsumoDetalle(insumo);
        }

        public Dictionary<string, object> getExistenciaInsumoDetalleTotal(int insumo)
        {
            return this.ReqDAO.getExistenciaInsumoDetalleTotal(insumo);
        }
        public Dictionary<string, object> getExistenciaInsumoDetalleAlmacenFisico(int insumo)
        {
            return this.ReqDAO.getExistenciaInsumoDetalleAlmacenFisico(insumo);
        }

        public Dictionary<string, object> getExistenciaInsumoDetalleTotalAlmacenFisico(int insumo)
        {
            return this.ReqDAO.getExistenciaInsumoDetalleTotalAlmacenFisico(insumo);
        }

        public void GuardarSurtido(RequisicionDTO info, List<SurtidoDTO> lstSurtido)
        {
            this.ReqDAO.GuardarSurtido(info, lstSurtido);
        }

        public List<tblCom_SurtidoDet> getSurtidoPorReq(string cc, int numero)
        {
            return this.ReqDAO.getSurtidoPorReq(cc, numero);
        }

        public List<SurtidoDetDTO> getSalidas(int almacenOrigenID, int almacenDestinoID)
        {
            return this.ReqDAO.getSalidas(almacenOrigenID, almacenDestinoID);
        }

        public List<salidasAlmacenDTO> GuardarSalidas(List<SurtidoDetDTO> salidas)
        {
            return this.ReqDAO.GuardarSalidas(salidas);
        }

        public List<SurtidoDetDTO> getEntradas(int almacenOrigen, string centroCostoOrigen, int almacenDestino, string centroCostoDestino)
        {
            return this.ReqDAO.getEntradas(almacenOrigen, centroCostoOrigen, almacenDestino, centroCostoDestino);
        }

        public List<SurtidoDetDTO> getSalidaTraspaso(int almacenOrigen, string centroCostoOrigen, int almacenDestino, string centroCostoDestino, int folioTraspaso)
        {
            return this.ReqDAO.getSalidaTraspaso(almacenOrigen, centroCostoOrigen, almacenDestino, centroCostoDestino, folioTraspaso);
        }

        public List<salidasAlmacenDTO> GuardarEntradas(List<SurtidoDetDTO> entradas, int folio_traspaso, int almacenDestinoOriginal)
        {
            return this.ReqDAO.GuardarEntradas(entradas, folio_traspaso, almacenDestinoOriginal);
        }

        public int getUltimaRequisicionNumero(string cc)
        {
            return this.ReqDAO.getUltimaRequisicionNumero(cc);
        }
        public dynamic getMovSalidaAlmacen(int almacen_id, string cc, int folioSalida)
        {
            return this.ReqDAO.getMovSalidaAlmacen(almacen_id, cc, folioSalida);
        }

        #region SALIDA POR CONSUMO
        public List<RequisicionDTO> getReqSalidasConsumo(string cc, int tipo)
        {
            return this.ReqDAO.getReqSalidasConsumo(cc, tipo);
        }
        public List<salidaConsumoDTO> getReqDetSalidasConsumo(string cc, int req, int almacen)
        {
            return this.ReqDAO.getReqDetSalidasConsumo(cc, req, almacen);
        }
        public List<salidasAlmacenDTO> guardarSalidasConsumo(List<SurtidoDetDTO> salidas, bool salidaNormal)
        {
            return this.ReqDAO.guardarSalidasConsumo(salidas, salidaNormal);
        }
        public List<entradasAlmacenDTO> GuardarEntradasConsumo(List<SurtidoDetDTO> entradas)
        {
            return this.ReqDAO.GuardarEntradasConsumo(entradas);
        }
        public List<salidasAlmacenDTO> GuardarSalidasC(List<SurtidoDetDTO> salidas)
        {
            return this.ReqDAO.GuardarSalidasC(salidas);
        }

        #endregion

        #region Pendiente por Surtir
        public List<RequisicionDTO> ObtenerRequisicionesPendientes(List<string> listaCC, List<int> listaAlmacenes, int estatus, int validadoAlmacen, DateTime fechaInicio, DateTime fechaFin)
        {
            return this.ReqDAO.ObtenerRequisicionesPendientes(listaCC, listaAlmacenes, estatus, validadoAlmacen, fechaInicio, fechaFin);
        }
        #endregion

        public rptRequisicionInfoDTO getRequisicionRpt(string cc, int numero, string PERU_tipoRequisicion)
        {
            return this.ReqDAO.getRequisicionRpt(cc, numero, PERU_tipoRequisicion);
        }

        public dynamic getUltimaRequisicionSIGOPLAN(string cc)
        {
            return this.ReqDAO.getUltimaRequisicionSIGOPLAN(cc);
        }

        public Dictionary<string, object> validarSurtido(string cc, int numero)
        {
            return this.ReqDAO.validarSurtido(cc, numero);
        }

        public Dictionary<string, object> validarSurtidoCompras(string cc, int numero)
        {
            return this.ReqDAO.validarSurtidoCompras(cc, numero);
        }

        public Dictionary<string, object> getRequisicionesPorUsuarioProcesadas(List<string> listCC)
        {
            return this.ReqDAO.getRequisicionesPorUsuarioProcesadas(listCC);
        }

        public Dictionary<string, object> validacionesRequisitor(string cc, List<int> numeros)
        {
            return this.ReqDAO.validacionesRequisitor(cc, numeros);
        }

        public Dictionary<string, object> getUbicacionDetalle(string cc, int almacenID, int insumo)
        {
            return this.ReqDAO.getUbicacionDetalle(cc, almacenID, insumo);
        }

        public List<UbicacionDetalleDTO> getUbicacionPorRequisicion(RequisicionDTO requisicion)
        {
            return this.ReqDAO.getUbicacionPorRequisicion(requisicion);
        }

        public Dictionary<string, object> confirmarRequisicion(RequisicionDTO requisicion)
        {
            return this.ReqDAO.confirmarRequisicion(requisicion);
        }

        public dynamic getInsumosAutoComplete(string term)
        {
            return this.ReqDAO.getInsumosAutoComplete(term);
        }

        public dynamic getInsumosDescAutoComplete(string term)
        {
            return this.ReqDAO.getInsumosDescAutoComplete(term);
        }

        public bool checarUbicacionesValidas(List<SurtidoDetDTO> entradas)
        {
            return this.ReqDAO.checarUbicacionesValidas(entradas);
        }

        public dynamic getInsumoInformacion(int insumo, bool esServicio)
        {
            return this.ReqDAO.getInsumoInformacion(insumo, esServicio);
        }
        public dynamic getInsumoInformacionByAlmacen(int insumo, int almacen)
        {
            return this.ReqDAO.getInsumoInformacionByAlmacen(insumo,almacen);
        }

        public dynamic getInsumoInformacionByAlmacenEntrada(int insumo, int almacen)
        {
            return this.ReqDAO.getInsumoInformacionByAlmacenEntrada(insumo, almacen);
        }

        public List<SurtidoDTO> getSurtidoDetalle(string cc, int numero)
        {
            return this.ReqDAO.getSurtidoDetalle(cc, numero);
        }

        public void BorrarRequisicionesMasivo()
        {
            ReqDAO.BorrarRequisicionesMasivo();
        }

        public void borrarRequisicion(string cc, int numero, string tpRequi)
        {
            this.ReqDAO.borrarRequisicion(cc, numero, tpRequi);
        }

        public List<RequisicionSeguimientoDTO> getRequisicionesSeguimiento(List<string> listaCC, List<int> listaTipoInsumo, DateTime fechaInicial, DateTime fechaFinal, int estatus, int requisitor, int compradorSugeridoEnReq)
        {
            return this.ReqDAO.getRequisicionesSeguimiento(listaCC, listaTipoInsumo, fechaInicial, fechaFinal, estatus, requisitor, compradorSugeridoEnReq);
        }

        #region REPORTE TIEMPO DE PROCESO DE OC
        public List<RequisicionSeguimientoDTO> GetTiempoProcesoOC(List<string> listaCC, List<int> listaTipoInsumo, DateTime fechaInicial, DateTime fechaFinal, int estatus, int requisitor, List<string> claveProveedor)
        {
            return this.ReqDAO.GetTiempoProcesoOC(listaCC, listaTipoInsumo, fechaInicial, fechaFinal, estatus, requisitor, claveProveedor);
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboProveedoresReporteProcesoOC()
        {
            return this.ReqDAO.FillComboProveedoresReporteProcesoOC();
        }
        #endregion

        public Dictionary<string, object> cancelarValidado(string cc, int numero)
        {
            return this.ReqDAO.cancelarValidado(cc, numero);
        }

        public List<SurtidoRequisicionDTO> getReporteSurtidoRequisicion(string cc, int numero)
        {
            return this.ReqDAO.getReporteSurtidoRequisicion(cc, numero);
        }

        public bool checkMaquinaStandBy(string cc)
        {
            return this.ReqDAO.checkMaquinaStandBy(cc);
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> getRequisitores()
        {
            return this.ReqDAO.getRequisitores();
        }

        public Dictionary<string, object> getInsumoInformacionSurtido(int insumo, string cc, int numero_requisicion)
        {
            return this.ReqDAO.getInsumoInformacionSurtido(insumo, cc, numero_requisicion);
        }

        public Dictionary<string, object> getUbicacionDetalleSurtido(string cc, int numero_requisicion, int almacenID, int partida, int insumo)
        {
            return this.ReqDAO.getUbicacionDetalleSurtido(cc, numero_requisicion, almacenID, partida, insumo);
        }

        public object getEmpleadoEnKontrolAutocomplete(string term)
        {
            return this.ReqDAO.getEmpleadoEnKontrolAutocomplete(term);
        }

        public Dictionary<string, object> CalcularExistenciasRequisicion(int almacen, List<int> listaInsumos)
        {
            return this.ReqDAO.CalcularExistenciasRequisicion(almacen, listaInsumos);
        }

        #region CRUD Insumos Consignación - Licitación - Convenio
        #region Consigna
        public Dictionary<string, object> GetInsumosConsigna(DataTablesParam param)
        {
            return this.ReqDAO.GetInsumosConsigna(param);
        }

        public Dictionary<string, object> GuardarNuevoInsumoConsigna(tblCom_InsumosConsigna insumo)
        {
            return this.ReqDAO.GuardarNuevoInsumoConsigna(insumo);
        }

        public Dictionary<string, object> EditarInsumoConsigna(tblCom_InsumosConsigna insumo)
        {
            return this.ReqDAO.EditarInsumoConsigna(insumo);
        }

        public Dictionary<string, object> EliminarInsumoConsigna(tblCom_InsumosConsigna insumo)
        {
            return this.ReqDAO.EliminarInsumoConsigna(insumo);
        }

        public Dictionary<string, object> CargarExcelInsumosConsigna(HttpFileCollectionBase archivos)
        {
            return this.ReqDAO.CargarExcelInsumosConsigna(archivos);
        }

        public MemoryStream DescargarExcelInsumosConsigna()
        {
            return this.ReqDAO.DescargarExcelInsumosConsigna();
        }
        #endregion

        #region Licitación
        public Dictionary<string, object> GetInsumosLicitacion(DataTablesParam param)
        {
            return this.ReqDAO.GetInsumosLicitacion(param);
        }

        public Dictionary<string, object> GuardarNuevoInsumoLicitacion(tblCom_InsumosLicitacion insumo)
        {
            return this.ReqDAO.GuardarNuevoInsumoLicitacion(insumo);
        }

        public Dictionary<string, object> EditarInsumoLicitacion(tblCom_InsumosLicitacion insumo)
        {
            return this.ReqDAO.EditarInsumoLicitacion(insumo);
        }

        public Dictionary<string, object> EliminarInsumoLicitacion(tblCom_InsumosLicitacion insumo)
        {
            return this.ReqDAO.EliminarInsumoLicitacion(insumo);
        }

        public Dictionary<string, object> CargarExcelInsumosLicitacion(HttpFileCollectionBase archivos)
        {
            return this.ReqDAO.CargarExcelInsumosLicitacion(archivos);
        }

        public MemoryStream DescargarExcelInsumosLicitacion()
        {
            return this.ReqDAO.DescargarExcelInsumosLicitacion();
        }
        #endregion

        #region Convenio
        public Dictionary<string, object> GetInsumosConvenio(DataTablesParam param)
        {
            return this.ReqDAO.GetInsumosConvenio(param);
        }

        public Dictionary<string, object> GuardarNuevoInsumoConvenio(tblCom_InsumosConvenio insumo)
        {
            return this.ReqDAO.GuardarNuevoInsumoConvenio(insumo);
        }

        public Dictionary<string, object> EditarInsumoConvenio(tblCom_InsumosConvenio insumo)
        {
            return this.ReqDAO.EditarInsumoConvenio(insumo);
        }

        public Dictionary<string, object> EliminarInsumoConvenio(tblCom_InsumosConvenio insumo)
        {
            return this.ReqDAO.EliminarInsumoConvenio(insumo);
        }

        public Dictionary<string, object> CargarExcelInsumosConvenio(HttpFileCollectionBase archivos)
        {
            return this.ReqDAO.CargarExcelInsumosConvenio(archivos);
        }

        public MemoryStream DescargarExcelInsumosConvenio()
        {
            return this.ReqDAO.DescargarExcelInsumosConvenio();
        }
        #endregion
        #endregion

        public bool EnviarCorreoRequisicion(string cc, int numero, List<string> listaCorreos, List<Byte[]> downloadPDF, string link)
        {
            return this.ReqDAO.EnviarCorreoRequisicion(cc, numero, listaCorreos, downloadPDF, link);
        }

        public Dictionary<string, object> GetArticulosConsignaLicitacionConvenioPorProveedor(int proveedor, TipoConsignaLicitacionConvenioEnum tipo)
        {
            return this.ReqDAO.GetArticulosConsignaLicitacionConvenioPorProveedor(proveedor, tipo);
        }

        public Dictionary<string, object> GetInsumoProveedorConsigna(int insumo, int proveedor)
        {
            return this.ReqDAO.GetInsumoProveedorConsigna(insumo, proveedor);
        }

        public Dictionary<string, object> GetInsumoProveedorConvenio(int insumo, int proveedor)
        {
            return this.ReqDAO.GetInsumoProveedorConvenio(insumo, proveedor);
        }

        public Dictionary<string, object> GetInsumoProveedorLicitacion(int insumo, int proveedor)
        {
            return this.ReqDAO.GetInsumoProveedorLicitacion(insumo, proveedor);
        }
    }
}
