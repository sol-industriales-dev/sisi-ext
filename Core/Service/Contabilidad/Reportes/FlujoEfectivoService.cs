using Core.DAO.Contabilidad.Reportes;
using Core.DTO.Contabilidad.FlujoEfectivo;
using Core.DTO.Contabilidad.Poliza;
using Core.DTO.Contabilidad.Propuesta.Nomina;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using Core.Enum.Administracion.FlujoEfectivo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.Reportes
{
    public class FlujoEfectivoService : IFlujoEfectivoDAO
    {
        #region Atributos
        private IFlujoEfectivoDAO r_EfectivoDAO;
        #endregion
        #region Propiedades
        public IFlujoEfectivoDAO REfectivoDAO
        {
            get { return r_EfectivoDAO; }
            set { r_EfectivoDAO = value; }
        }
        #endregion
        #region Contructor
        public FlujoEfectivoService(IFlujoEfectivoDAO rEfectivo)
        {
            REfectivoDAO = rEfectivo;
        }
        #endregion
        #region Guardar
        public bool guardarCcVisto(tblC_FED_CcVisto cc)
        {
            return r_EfectivoDAO.guardarCcVisto(cc);
        }
        public bool guardarConcepto(tblC_FE_CatConcepto obj, List<tblC_FE_RelConceptoTm> rel)
        {
            return r_EfectivoDAO.guardarConcepto(obj, rel);
        }
        public bool guardarConceptoDirecto(tblC_FED_CatConcepto obj, List<tblC_FED_RelConceptoTm> rel)
        {
            return r_EfectivoDAO.guardarConceptoDirecto(obj, rel);
        }
        public bool guardarMovPol(List<tblC_FE_MovPol> lst)
        {
            return r_EfectivoDAO.guardarMovPol(lst);
        }
        public bool guardarCorte(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.guardarCorte(busq);
        }
        public bool cancelarCorte(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.cancelarCorte(busq);
        }
        public bool GuardarPlaneacion(List<tblC_FED_CapPlaneacion> lst)
        {
            return r_EfectivoDAO.GuardarPlaneacion(lst);
        }
        public List<tblC_FED_DetProyeccionCierre> guardarDetProyeccionCierre(List<tblC_FED_DetProyeccionCierre> lst)
        {
            return r_EfectivoDAO.guardarDetProyeccionCierre(lst);
        }
        public bool eliminarDetProyeccionCierre(int id)
        {
            return r_EfectivoDAO.eliminarDetProyeccionCierre(id);
        }
        public bool GuardarGpoReserva(tblC_FED_CatGrupoReserva gpo)
        {
            return r_EfectivoDAO.GuardarGpoReserva(gpo);
        }
        #endregion
        #region Concepto
        public List<tblC_FE_CatConcepto> getCatConcepto()
        {
            return r_EfectivoDAO.getCatConcepto();
        }
        public List<tblC_FED_CatConcepto> getCatConceptoDir()
        {
            return r_EfectivoDAO.getCatConceptoDir();
        }
        public List<tblC_FE_RelConceptoTm> getRelConceptoTm()
        {
            return r_EfectivoDAO.getRelConceptoTm();
        }
        public List<tblC_FED_RelConceptoTm> getRelConceptoDirTm()
        {
            return r_EfectivoDAO.getRelConceptoDirTm();
        }
        public List<tblC_FED_RelConceptoTm> getRelConceptoDirTmArr()
        {
            return r_EfectivoDAO.getRelConceptoDirTmArr();
        }
        #endregion
        #region ¿Que pasa si?
        public List<tblC_FED_CapPlaneacion> getPlaneacionCierre(BusqProyeccionCierreDTO busq)
        {
            return r_EfectivoDAO.getPlaneacionCierre(busq);
        }
        public List<tblC_FED_DetProyeccionCierre> getPlaneacionCierreDetalle(BusqProyeccionCierreDTO busq)
        {
            return r_EfectivoDAO.getPlaneacionCierreDetalle(busq);
        }
        public List<tblC_FED_DetProyeccionCierre> getPlaneacionCierreDetalle(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.getPlaneacionCierreDetalle(busq);
        }
        public List<tblC_FED_DetProyeccionCierre> getLstDetProyeccionCierre(BusqProyeccionCierreDTO busq)
        {
            return r_EfectivoDAO.getLstDetProyeccionCierre(busq);
        }
        public List<tblC_FED_DetProyeccionCierre> getLstDetReservaDelAnio(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.getLstDetReservaDelAnio(busq);
        }
        public List<tblC_FED_DetProyeccionCierre> getLstDetProyeccionCierre(List<int> lstId)
        {
            return r_EfectivoDAO.getLstDetProyeccionCierre(lstId);
        }
        public List<tblC_FED_DetProyeccionCierre> getLstDetProyeccionCierre(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.getLstDetProyeccionCierre(busq);
        }
        public Task<List<tblC_FED_DetProyeccionCierre>> initLstDetProyeccionCierre(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.initLstDetProyeccionCierre(busq);
        }
        public List<tblC_FED_DetProyeccionCierre> initLstDetProyeccionCierre_Optimizado(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.initLstDetProyeccionCierre_Optimizado(busq);
        }
        public List<tblC_FED_CatGrupoReserva> getLstGpoReserva()
        {
            return r_EfectivoDAO.getLstGpoReserva();
        }
        public List<tblC_FED_CatGrupoReserva> getAllGpoReserva()
        {
            return r_EfectivoDAO.getAllGpoReserva();
        }
        #endregion
        #region Flujo Efectivo
        public List<MovpolDTO> getLstMovPolAcumulado(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.getLstMovPolAcumulado(busq);
        }
        public List<CatctaDTO> getCatCtaDeudoresDiversios()
        {
            return r_EfectivoDAO.getCatCtaDeudoresDiversios();
        }
        public List<MovpolDTO> getLstMovPol(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.getLstMovPol(busq);
        }
        public List<tblC_FE_MovPol> getLstMovPolFlujoEfectivo(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.getLstMovPolFlujoEfectivo(busq);
        }
        public List<tblC_FE_MovPol> getLstMovPolFlujoEfectivoOperativo(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.getLstMovPolFlujoEfectivoOperativo(busq);
        }
        public Task<List<tblC_FE_MovPol>> getLstMovPolFlujoEfectivoDirecto(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.getLstMovPolFlujoEfectivoDirecto(busq);
        }
        public List<tblC_FE_MovPol> getLstMovPolFlujoEfectivoDirecto_Optimizado(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.getLstMovPolFlujoEfectivoDirecto_Optimizado(busq);
        }
        public List<tblC_FE_MovPol> getLstMovPolFlujoEfectivoDirecto_Optimizado(tipoDetalleEnum tipo, BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.getLstMovPolFlujoEfectivoDirecto_Optimizado(tipo,busq);
        }
        public Task<List<tblC_FED_PlaneacionDet>> getLstDetPlaneacionDirecto(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.getLstDetPlaneacionDirecto(busq);
        }
        public List<tblC_FED_PlaneacionDet> getLstDetPlaneacionDirecto_Optimizado(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.getLstDetPlaneacionDirecto_Optimizado(busq);
        }
        public List<SalContCcDTO> getLstSalContCC(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.getLstSalContCC(busq);
        }
        public List<tblC_FE_CatConcepto> getCatConceptoActivo()
        {
            return r_EfectivoDAO.getCatConceptoActivo();
        }
        public List<tblC_FED_CatConcepto> getCatConceptoDirActivo()
        {
            return r_EfectivoDAO.getCatConceptoDirActivo();
        }
        public List<tblC_FE_MovPol> getLstMovPolActiva(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.getLstMovPolActiva(busq);
        }
        public List<tblC_FED_Corte> getLstCortes()
        {
            return r_EfectivoDAO.getLstCortes();
        }
        public List<tblC_FE_MovPol> getLstMovPolActiva()
        {
            return r_EfectivoDAO.getLstMovPolActiva();
        }
        public List<MovpolDTO> getLstMovPolFlujoTotal(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.getLstMovPolFlujoTotal(busq);
        }
        public Task<List<MovpolDTO>> taskLstMovPolFlujoTotal(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.taskLstMovPolFlujoTotal(busq);
        }
        public List<MovpolDTO> taskLstMovPolFlujoTotal_Optimizado(FiltroPolizasDTO filtro)
        {
            return r_EfectivoDAO.taskLstMovPolFlujoTotal_Optimizado(filtro);
        }
        public List<tblC_FED_SaldoInicial> getLstSaldoInicial(int anio)
        {
            return r_EfectivoDAO.getLstSaldoInicial(anio);
        }
        public List<tblC_FE_SaldoInicial> getLstFE_SaldoInicial(int anio)
        {
            return r_EfectivoDAO.getLstFE_SaldoInicial(anio);
        }
        public List<tblC_FED_CcVisto> getLstCCvistos(int anio, int semana)
        {
            return r_EfectivoDAO.getLstCCvistos(anio, semana);
        }
        public List<tblC_FED_RelObraUsuario> getRelObraUsuario()
        {
            return r_EfectivoDAO.getRelObraUsuario();
        }
        public List<tblC_FED_CapPlaneacion> getFlujoTodoEfctivoYCostos(int anio, int noSemana)
        {
            return r_EfectivoDAO.getFlujoTodoEfctivoYCostos(anio, noSemana);
        }
        #endregion
        #region Planeacion
        public List<tblC_FED_CapPlaneacion> getPlaneacion(bool esConciliado)
        {
            return r_EfectivoDAO.getPlaneacion(esConciliado);
        }
        public List<tblC_FED_CapPlaneacion> getPlaneacion(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.getPlaneacion(busq);
        }
        public IQueryable<tblC_FED_CapPlaneacion> getPlaneacionOptimizado(BusqFlujoEfectivoDTO busq)
        {
            return r_EfectivoDAO.getPlaneacionOptimizado(busq);
        }
        #endregion
        #region combobox
        public List<ComboDTO> getCboGrupoConcepto()
        {
            return r_EfectivoDAO.getCboGrupoConcepto();
        }
        public List<ComboGroupDTO> getCboGpoConcepto()
        {
            return r_EfectivoDAO.getCboGpoConcepto();
        }
        public List<ComboDTO> getCboCCActivosSigoplan()
        {
            return r_EfectivoDAO.getCboCCActivosSigoplan();
        }
        public List<ComboDTO> getLstGrupoReserva()
        {
            return r_EfectivoDAO.getLstGrupoReserva();
        }
        public List<ComboDTO> getComboAreaCuenta()
        {
            return r_EfectivoDAO.getComboAreaCuenta();
        }
        #endregion
        #region Planeacion detalle
        public Dictionary<string, object> getGastosOperativos(DateTime fechaInicio, DateTime fechaFin, string cc, int semana, int anio)
        {
            return r_EfectivoDAO.getGastosOperativos(fechaInicio, fechaFin, cc, semana, anio);
        }

        public Dictionary<string, object> ObtenerInfoConceptos(string cc, int semana, int anio)
        {
            return r_EfectivoDAO.ObtenerInfoConceptos(cc, semana, anio);
        }
        public Dictionary<string, object> getDetallePlaneacion(int concepto, string cc, int semana, int anio, int tipo)
        {
            return r_EfectivoDAO.getDetallePlaneacion(concepto, cc, semana, anio, tipo);
        }
        public Dictionary<string, object> saveOrUpdateDetalle(List<tblC_FED_PlaneacionDet> lst)
        {
            return r_EfectivoDAO.saveOrUpdateDetalle(lst);
        }
        public Dictionary<string, object> getGastosProyecto(DateTime fechaInicio, DateTime fechaFin, string cc, int semana, int anio)
        {
            return r_EfectivoDAO.getGastosProyecto(fechaInicio, fechaFin, cc, semana, anio);
        }
        public Dictionary<string, object> getEfectivoRecibido(DateTime fechaInicio, DateTime fechaFin, string cc, int semana, int anio)
        {
            return r_EfectivoDAO.getEfectivoRecibido(fechaInicio, fechaFin, cc, semana, anio);
        }

        public Dictionary<string, object> saveDetallesMasivos(List<tblC_FED_PlaneacionDet> listaPlaneacionDet, int idConceptoDir, int anio, int semana, string cc)
        {
            return r_EfectivoDAO.saveDetallesMasivos(listaPlaneacionDet, idConceptoDir, anio, semana, cc);
        }

        public Dictionary<string, object> getListaClientes()
        {
            return r_EfectivoDAO.getListaClientes();
        }

        public Dictionary<string, object> getDescripcionesPlaneacion(int conceptoID, string cc, int semana, int anio)
        {
            return r_EfectivoDAO.getDescripcionesPlaneacion(conceptoID, cc, semana, anio);
        }

        public Dictionary<string, object> getDetalleDescripcionPlaneacion(PlaneacionDetDTO planeacionDetDTO, string cc)
        {
            return r_EfectivoDAO.getDetalleDescripcionPlaneacion(planeacionDetDTO, cc);

        }
        public Dictionary<string, object> getCadenasProductivas(DateTime fechaInicio, DateTime fechaFin, string cc, int semana, int anio)
        {
            return r_EfectivoDAO.getCadenasProductivas(fechaInicio, fechaFin, cc, semana, anio);
        }

        public Dictionary<string, object> getCargaNomina(List<PeriodosNominaDTO> lstPeriodo, string cc, int semana, int anio)
        {
            return r_EfectivoDAO.getCargaNomina(lstPeriodo, cc, semana, anio);
        }

        public Dictionary<string, object> getDetallesPlaneacionPPal(int conceptoID, string cc, int semana, int anio, bool esConciliado)
        {
            return r_EfectivoDAO.getDetallesPlaneacionPPal(conceptoID, cc, semana, anio, esConciliado);
        }

        public Dictionary<string, object> getSubNivelDetallePlaneacion(int conceptoID, string cc, int semana, int anio, int tipo)
        {
            return r_EfectivoDAO.getSubNivelDetallePlaneacion(conceptoID, cc, semana, anio, tipo);
        }

        public Dictionary<string, object> getSubDetalle(string cc, int semana, int anio, int concepto, int numProv, int numcte)
        {
            return r_EfectivoDAO.getSubDetalle(cc, semana, anio, concepto, numProv, numcte);
        }

        public List<tblC_FED_Corte> getSemanasCorte() {
            return r_EfectivoDAO.getSemanasCorte();
        }
        public tblC_FED_Corte getCorteActaul()
        {
            return r_EfectivoDAO.getCorteActaul();
        }
        #endregion
    }
}
