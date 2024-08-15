using Core.DTO.Contabilidad.FlujoEfectivo;
using Core.DTO.Contabilidad.Poliza;
using Core.DTO.Contabilidad.Propuesta.Nomina;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Reportes
{
    public interface IFlujoEfectivoArrendadora
    {
        #region Guardar
        bool guardarCcVisto(tblC_FED_CcVisto cc);
        bool guardarConcepto(tblC_FE_CatConcepto obj, List<tblC_FE_RelConceptoTm> rel);
        bool guardarConceptoDirecto(tblC_FED_CatConcepto obj, List<tblC_FED_RelConceptoTm> rel);
        bool guardarMovPol(List<tblC_FE_MovPol> lst);
        bool guardarCorte(BusqFlujoEfectivoDTO busq);
        bool cancelarCorte(BusqFlujoEfectivoDTO busq);
        bool GuardarPlaneacion(List<tblC_FED_CapPlaneacion> lst);
        List<tblC_FED_DetProyeccionCierre> guardarDetProyeccionCierre(List<tblC_FED_DetProyeccionCierre> lst, bool esCC);
        bool eliminarDetProyeccionCierre(int id);
        bool GuardarGpoReserva(tblC_FED_CatGrupoReserva gpo);
        #endregion
        #region Concepto
        List<tblC_FE_CatConcepto> getCatConcepto();
        List<tblC_FED_CatConcepto> getCatConceptoDir();
        List<tblC_FE_RelConceptoTm> getRelConceptoTm();
        List<tblC_FED_RelConceptoTm> getRelConceptoDirTm();
        #endregion
        #region ¿Que pasa si?
        List<tblC_FED_CapPlaneacion> getPlaneacionCierre(BusqFlujoEfectivoDTO busq);
        List<tblC_FED_CapPlaneacion> getPlaneacionCierre(BusqProyeccionCierreDTO busq);
        List<tblC_FED_DetProyeccionCierre> getPlaneacionCierreDetalle(BusqProyeccionCierreDTO busq);
        List<tblC_FED_DetProyeccionCierre> getPlaneacionCierreDetalle(BusqFlujoEfectivoDTO busq);
        List<tblC_FED_DetProyeccionCierre> getPlaneacionCierreDetalle_Optimizado(BusqProyeccionCierreDTO busq);
        List<tblC_FED_DetProyeccionCierre> getPlaneacionCierreDetalle_Optimizado(BusqFlujoEfectivoDTO busq);
        List<tblC_FED_DetProyeccionCierre> getLstDetProyeccionCierre(List<int> lstId);        
        List<tblC_FED_DetProyeccionCierre> getLstDetProyeccionCierre(BusqProyeccionCierreDTO busq);
        List<tblC_FED_DetProyeccionCierre> getLstDetProyeccionCierre(BusqFlujoEfectivoDTO busq);
        List<tblC_FED_DetProyeccionCierre> initLstDetProyeccionCierre(BusqFlujoEfectivoDTO busq);
        List<tblC_FED_DetProyeccionCierre> initLstDetProyeccionCierre_Optimizado(BusqFlujoEfectivoDTO busq);
        List<tblC_FED_CatGrupoReserva> getLstGpoReserva();
        List<tblC_FED_CatGrupoReserva> getAllGpoReserva();
        #endregion
        #region Flujo Efectivo
        List<MovpolDTO> getLstMovPolAcumulado(BusqFlujoEfectivoDTO busq);
        List<CatctaDTO> getCatCtaDeudoresDiversios();
        List<MovpolDTO> getLstMovPol(BusqFlujoEfectivoDTO busq);
        List<tblC_FE_MovPol> getFE_LstMovPol(BusqFlujoEfectivoDTO busq);
        bool verificaYActualizaAreaCuentaEnMovimientoPolizas();
        List<tblC_FE_MovPol> getLstMovPolFlujoEfectivo(BusqFlujoEfectivoDTO busq);
        List<tblC_FE_MovPol> getLstMovPolFlujoEfectivoOperativo(BusqFlujoEfectivoDTO busq);
        List<tblC_FE_MovPol> getLstMovPolFlujoEfectivoDirecto(BusqFlujoEfectivoDTO busq);
        List<SalContCcDTO> getLstSalContCC(BusqFlujoEfectivoDTO busq);
        List<tblC_FE_CatConcepto> getCatConceptoActivo();
        List<tblC_FED_CatConcepto> getCatConceptoDirActivo();
        List<tblC_FE_MovPol> getLstMovPolActiva(BusqFlujoEfectivoDTO busq);
        List<tblC_FE_MovPol> getLstMovPolActiva();
        List<MovpolDTO> getLstMovPolFlujoTotal(BusqFlujoEfectivoDTO busq);
        Task<List<MovpolDTO>> taskLstMovPolFlujoTotal(BusqFlujoEfectivoDTO busq);
        List<MovpolDTO> taskLstMovPolFlujoTotal_Optimizado(FiltroPolizasDTO busq);
        List<MovpolDTO> taskLstMovPolFlujoTotal_Optimizado2(BusqFlujoEfectivoDTO busq);
        List<tblC_FED_SaldoInicial> getLstSaldoInicial(int anio);
        List<tblC_FE_SaldoInicial> getLstFE_SaldoInicial(int anio);
        List<tblC_FED_CcVisto> getLstCCvistos(int anio, int semana);
        List<Core.DTO.Contabilidad.CcDTO> lstObra();
        List<tblC_FED_RelObraUsuario> getRelObraUsuario();
        List<tblC_FED_CapPlaneacion> getLstSaldoInicialCCTM(BusqFlujoEfectivoDTO busq);
        #endregion
        #region Planeacion
        List<tblC_FED_CapPlaneacion> getPlaneacion();
        IQueryable<tblC_FED_CapPlaneacion> getPlaneacionOptimizado(BusqFlujoEfectivoDTO busq);
        List<tblC_FED_CapPlaneacion> getPlaneacion(BusqFlujoEfectivoDTO busq);
        List<tblC_FED_PlaneacionDet> getPlaneacionDetalles();
        #endregion
        #region combobox
        List<ComboDTO> getCboGrupoConcepto();
        List<ComboGroupDTO> getCboGpoConcepto();
        List<ComboDTO> getComboAreaCuentaConCentroCostos();
        List<ComboDTO> getComboAreaCuentaConCentroCostosPorUsuario();
        List<ComboDTO> getCboCCActivosSigoplan();
        List<ComboDTO> getCboCCTodosSigoplan();
        List<ComboDTO> getLstGrupoReserva();
        List<ComboDTO> getComboAreaCuenta();
        #endregion
        #region Catalogo de Detalle
        Dictionary<string, object> ObtenerInfoConceptos(string cc, string ac, int semana, int anio, bool esCC);
        Dictionary<string, object> getDetallePlaneacion(int concepto, string cc, string ac, int semana, int anio, int tipo, bool esCC);
        Dictionary<string, object> saveOrUpdateDetalle(List<tblC_FED_PlaneacionDet> lst);
        Dictionary<string, object> getGastosProyecto(DateTime fechaInicio, DateTime fechaFin, string cc, int semana, int anio);
        Dictionary<string, object> saveDetallesMasivos(List<tblC_FED_PlaneacionDet> listaPlaneacionDet, int idConceptoDir, int anio, int semana, string ac, string cc);
        Dictionary<string, object> getGastosOperativos(DateTime fechaInicio, DateTime fechaFin, string cc, int semana, int anio);
        Dictionary<string, object> getEfectivoRecibido(DateTime fechaInicio, DateTime fechaFin, string cc, string ac, int semana, int anio, bool esCC);
        Dictionary<string, object> getListaClientes();
        Dictionary<string, object> getDescripcionesPlaneacion(int conceptoID, string cc, int semana, int anio);
        Dictionary<string, object> getDetalleDescripcionPlaneacion(PlaneacionDetDTO planeacionDetDTO, string cc);
        Dictionary<string, object> getCadenasProductivas(DateTime fechaInicio, DateTime fechaFin, string ac, string cc, int semana, int anio, bool esCC);
        Dictionary<string, object> getCargaNomina(List<PeriodosNominaDTO> lstPeriodo, string cc, int semana, int anio);
        Dictionary<string, object> getDetallesPlaneacionPPal(int conceptoID, string ac, string cc, int semana, bool esCC, int anio);
        Dictionary<string, object> getSubNivelDetallePlaneacion(int conceptoID, string ac, string cc, int semana, bool esCC, int anio, int tipo);
        Dictionary<string, object> getSubDetalle(string ac, string cc, int semana, bool esCC, int anio, int conceptoID, int numProv, int numcte);
        #endregion
        #region Solo Arrendadora
        BusqFlujoEfectivoDTO setAreaCuentaPorCentroCostos(BusqFlujoEfectivoDTO busq);
        decimal getFlujoEfectivoTotalArrendadora(BusqFlujoEfectivoDTO busq);
        List<tblC_FED_DetProyeccionCierre> getPlaneacionCierreArrendadora(BusqProyeccionCierreDTO busq);
        Task<List<tblC_FED_DetProyeccionCierre>> getLstDetProyeccionCierreArrendadora(BusqFlujoEfectivoDTO busq);
        List<tblC_FED_CapPlaneacion> getFlujoDirectoArrendadora(BusqFlujoEfectivoDTO busq);
        #endregion

        #region Datos Kubrix
        List<tblC_FED_DetProyeccionCierre> getIngresosEstimadosKubrix(BusqFlujoEfectivoDTO busq, string concepto, int semana);
        List<tblC_FED_DetProyeccionCierre> getCostosEstimadosKubrix(BusqFlujoEfectivoDTO busq, string concepto, int semana);
        #endregion

        List<string> getACCerradas();
        List<tblC_FED_Corte> getSemanasCorte();
        tblC_FED_Corte getCorteActaul();
    }
}
