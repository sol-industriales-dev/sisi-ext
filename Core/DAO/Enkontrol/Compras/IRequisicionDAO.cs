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

namespace Core.DAO.Enkontrol.Compras
{
    public interface IRequisicionDAO
    {
        Dictionary<string, object> guardar(tblCom_Req req, List<tblCom_ReqDet> det, List<ReqDetalleComentarioDTO> comentarios);
        Dictionary<string, object> setAuth(List<tblCom_Req> lst);
        Dictionary<string, object> getNewReq(string cc, string tpRequi);
        Dictionary<string, object> getRequisicion(string cc, int num, bool esServicio);
        Dictionary<string, object> getReq(string cc, int num);
        dynamic getInsumos(string term, string cc, bool esServicio);
        dynamic getInsumosDesc(string term, string cc, bool esServicio);
        dynamic getInsumosByAlmacen(string term, string cc,int almacen);
        dynamic getInsumosDescByAlmacen(string term, string cc,int almacen);
        dynamic getInsumosByAlmacenEntrada(string term, string cc, int almacen);
        dynamic getInsumosDescByAlmacenEntrada(string term, string cc, int almacen);
        bool isEmpAdmin();
        dynamic getReq(bool isAuth, List<string> cc);
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboLab();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoReq();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboTxtFolio(int tipo);
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboResponsablePorCc(string cc);
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcReq(bool isAuth);
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcAsigReq();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcTodos();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoFolio();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboAlmacenSurtir();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboAlmacenSurtirAcceso();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboAlmacenSurtirTodos();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoInsumo();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboProveedores();
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboProveedoresConsignaLicitacionConvenio(TipoConsignaLicitacionConvenioEnum tipo);
        dynamic FillComboAreaCuenta(string cc);
        dynamic getThisEmpleadoEnkontrol();
        bool getFolio(string folio, int tipo);
        string getFolioOrigen(string cc, int num);
        RequisicionDTO getRequisicionSIGOPLAN(string cc, int num, string PERU_tipoRequisicion);
        Dictionary<string, object> getExistenciaInsumo(int insumo, string cc, int almacen);
        Dictionary<string, object> getExistenciaInsumoDetalle(int insumo);
        Dictionary<string, object> getExistenciaInsumoDetalleTotal(int insumo);
        Dictionary<string, object> getExistenciaInsumoDetalleAlmacenFisico(int insumo);
        Dictionary<string, object> getExistenciaInsumoDetalleTotalAlmacenFisico(int insumo);
        void GuardarSurtido(RequisicionDTO info, List<SurtidoDTO> lstSurtido);
        List<tblCom_SurtidoDet> getSurtidoPorReq(string cc, int numero);
        List<SurtidoDetDTO> getSalidas(int almacenOrigenID, int almacenDestinoID);
        List<salidasAlmacenDTO> GuardarSalidas(List<SurtidoDetDTO> salidas);
        List<SurtidoDetDTO> getEntradas(int almacenOrigen, string centroCostoOrigen, int almacenDestino, string centroCostoDestino);
        List<SurtidoDetDTO> getSalidaTraspaso(int almacenOrigen, string centroCostoOrigen, int almacenDestino, string centroCostoDestino, int folioTraspaso);
        List<salidasAlmacenDTO> GuardarEntradas(List<SurtidoDetDTO> entradas, int folio_traspaso, int almacenDestinoOriginal);
        int getUltimaRequisicionNumero(string cc);
        dynamic getMovSalidaAlmacen(int almacen_id, string cc, int folioSalida);

        #region SALIDA POR CONSUMO
        List<RequisicionDTO> getReqSalidasConsumo(string cc, int tipo);
        List<salidaConsumoDTO> getReqDetSalidasConsumo(string cc, int req, int almacen);
        List<salidasAlmacenDTO> guardarSalidasConsumo(List<SurtidoDetDTO> salidas, bool salidaNormal);
        List<entradasAlmacenDTO> GuardarEntradasConsumo(List<SurtidoDetDTO> entradas);
        List<salidasAlmacenDTO> GuardarSalidasC(List<SurtidoDetDTO> salidas);
        #endregion

        #region Pendiente por Surtir
        List<RequisicionDTO> ObtenerRequisicionesPendientes(List<string> listaCC, List<int> listaAlmacenes, int estatus, int validadoAlmacen, DateTime fechaInicio, DateTime fechaFin);
        #endregion

        rptRequisicionInfoDTO getRequisicionRpt(string cc, int numero, string PERU_tipoRequisicion);
        dynamic getUltimaRequisicionSIGOPLAN(string cc);
        Dictionary<string, object> validarSurtido(string cc, int numero);
        Dictionary<string, object> validarSurtidoCompras(string cc, int numero);
        Dictionary<string, object> getRequisicionesPorUsuarioProcesadas(List<string> listCC);
        Dictionary<string, object> validacionesRequisitor(string cc, List<int> numeros);
        Dictionary<string, object> getUbicacionDetalle(string cc, int almacenID, int insumo);
        List<UbicacionDetalleDTO> getUbicacionPorRequisicion(RequisicionDTO requisicion);
        Dictionary<string, object> confirmarRequisicion(RequisicionDTO requisicion);
        dynamic getInsumosAutoComplete(string term);
        dynamic getInsumosDescAutoComplete(string term);
        bool checarUbicacionesValidas(List<SurtidoDetDTO> entradas);
        dynamic getInsumoInformacion(int insumo, bool esServicio);
        dynamic getInsumoInformacionByAlmacen(int insumo, int almacen);
        dynamic getInsumoInformacionByAlmacenEntrada(int insumo, int almacen);
        List<SurtidoDTO> getSurtidoDetalle(string cc, int numero);
        void BorrarRequisicionesMasivo();
        void borrarRequisicion(string cc, int numero, string tpRequi);
        List<RequisicionSeguimientoDTO> getRequisicionesSeguimiento(List<string> listaCC, List<int> listaTipoInsumo, DateTime fechaInicial, DateTime fechaFinal, int estatus,int requisitor, int compradorSugeridoEnReq);

        #region REPORTE TIEMPO DE PROCESO DE OC
        List<RequisicionSeguimientoDTO> GetTiempoProcesoOC(List<string> listaCC, List<int> listaTipoInsumo, DateTime fechaInicial, DateTime fechaFinal, int estatus, int requisitor, List<string> claveProveedor);
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboProveedoresReporteProcesoOC();
        
        #endregion

        Dictionary<string, object> cancelarValidado(string cc, int numero);
        List<SurtidoRequisicionDTO> getReporteSurtidoRequisicion(string cc, int numero);
        bool checkMaquinaStandBy(string cc);
        List<Core.DTO.Principal.Generales.ComboDTO> getRequisitores();
        Dictionary<string, object> getInsumoInformacionSurtido(int insumo, string cc, int numero_requisicion);
        Dictionary<string, object> getUbicacionDetalleSurtido(string cc, int numero_requisicion, int almacenID, int partida, int insumo);
        object getEmpleadoEnKontrolAutocomplete(string term);
        Dictionary<string, object> CalcularExistenciasRequisicion(int almacen, List<int> listaInsumos);

        #region CRUD Insumos Consignación - Licitación - Convenio
        #region Consigna
        Dictionary<string, object> GetInsumosConsigna(DataTablesParam param);
        Dictionary<string, object> GuardarNuevoInsumoConsigna(tblCom_InsumosConsigna insumo);
        Dictionary<string, object> EditarInsumoConsigna(tblCom_InsumosConsigna insumo);
        Dictionary<string, object> EliminarInsumoConsigna(tblCom_InsumosConsigna insumo);
        Dictionary<string, object> CargarExcelInsumosConsigna(HttpFileCollectionBase archivos);
        MemoryStream DescargarExcelInsumosConsigna();
        #endregion

        #region Licitación
        Dictionary<string, object> GetInsumosLicitacion(DataTablesParam param);
        Dictionary<string, object> GuardarNuevoInsumoLicitacion(tblCom_InsumosLicitacion insumo);
        Dictionary<string, object> EditarInsumoLicitacion(tblCom_InsumosLicitacion insumo);
        Dictionary<string, object> EliminarInsumoLicitacion(tblCom_InsumosLicitacion insumo);
        Dictionary<string, object> CargarExcelInsumosLicitacion(HttpFileCollectionBase archivos);
        MemoryStream DescargarExcelInsumosLicitacion();
        #endregion

        #region Convenio
        Dictionary<string, object> GetInsumosConvenio(DataTablesParam param);
        Dictionary<string, object> GuardarNuevoInsumoConvenio(tblCom_InsumosConvenio insumo);
        Dictionary<string, object> EditarInsumoConvenio(tblCom_InsumosConvenio insumo);
        Dictionary<string, object> EliminarInsumoConvenio(tblCom_InsumosConvenio insumo);
        Dictionary<string, object> CargarExcelInsumosConvenio(HttpFileCollectionBase archivos);
        MemoryStream DescargarExcelInsumosConvenio();
        #endregion
        #endregion

        bool EnviarCorreoRequisicion(string cc, int numero, List<string> listaCorreos, List<Byte[]> downloadPDF, string link);
        Dictionary<string, object> GetArticulosConsignaLicitacionConvenioPorProveedor(int proveedor, TipoConsignaLicitacionConvenioEnum tipo);
        Dictionary<string, object> GetInsumoProveedorConsigna(int insumo, int proveedor);
        Dictionary<string, object> GetInsumoProveedorConvenio(int insumo, int proveedor);
        Dictionary<string, object> GetInsumoProveedorLicitacion(int insumo, int proveedor);
    }
}
