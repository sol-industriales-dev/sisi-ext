
using Core.DAO.Maquinaria.Captura;
using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAO.Maquinaria.Barrenacion;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Barrenacion;
using Core.Enum.Maquinaria.Barrenacion;
using Core.DTO.Maquinaria.Barrenacion;
using Core.DTO.Maquinaria.Barrenacion.Reporte;

namespace Core.Service.Maquinaria.Barrenacion
{
    public class BarrenacionService : IBarrenacionDAO
    {
        #region Variables y constructor
        private IBarrenacionDAO barrenacionDAO { get; set; }

        public BarrenacionService(IBarrenacionDAO iBarrenacionDAO)
        {
            barrenacionDAO = iBarrenacionDAO;
        }
        #endregion

        #region Combos
        public Dictionary<string, object> ObtenerAC()
        {
            return barrenacionDAO.ObtenerAC();
        }

        public Dictionary<string, object> GetComboBancos(string areaCuenta)
        {
            return barrenacionDAO.GetComboBancos(areaCuenta);
        }
        #endregion

        #region Mano de Obra
        public Dictionary<string, object> ObtenerBarrenadorasOperadores(string areaCuenta, int estatusOperadores)
        {
            return barrenacionDAO.ObtenerBarrenadorasOperadores(areaCuenta, estatusOperadores);
        }

        public Dictionary<string, object> ObtenerOperadoresBarrenadora(int barrenadoraID, int turno)
        {
            return barrenacionDAO.ObtenerOperadoresBarrenadora(barrenadoraID, turno);
        }

        public dynamic ObtenerEmpleadosEnKontrol(string term, bool porDesc)
        {
            return barrenacionDAO.ObtenerEmpleadosEnKontrol(term, porDesc);
        }

        public Dictionary<string, object> GuardarOperadoresBarrenadora(List<tblB_ManoObra> listaOperadores)
        {
            return barrenacionDAO.GuardarOperadoresBarrenadora(listaOperadores);
        }
        #endregion

        #region Piezas Barrenadora

        public Dictionary<string, object> GetPiezaID(int piezaID)
        {
            return barrenacionDAO.GetPiezaID(piezaID);
        }
        public Dictionary<string, object> ObtenerBarrenadorasPiezas(string areaCuenta, int estatusPiezas)
        {
            return barrenacionDAO.ObtenerBarrenadorasPiezas(areaCuenta, estatusPiezas);
        }

        public Dictionary<string, object> ObtenerInsumosPorPiezaPrecio(string areCuenta)
        {
            return barrenacionDAO.ObtenerInsumosPorPiezaPrecio(areCuenta);
        }

        public Dictionary<string, object> ObtenerPiezasBarrenadora(int barrenadoraID, string areaCuenta)
        {
            return barrenacionDAO.ObtenerPiezasBarrenadora(barrenadoraID, areaCuenta);
        }

        public dynamic getInsumo(string term, bool porDesc)
        {
            return barrenacionDAO.getInsumo(term, porDesc);
        }

        public Dictionary<string, object> GuardarPiezasBarrenadora(List<tblB_PiezaBarrenadora> listaPiezas, string desechoMartillo, string desechoBarra, bool pzasCompletas)
        {
            return barrenacionDAO.GuardarPiezasBarrenadora(listaPiezas, desechoMartillo, desechoBarra, pzasCompletas);
        }

        public dynamic ObtenerSerieMartilloReparadoNoAsignado(string term)
        {
            return barrenacionDAO.ObtenerSerieMartilloReparadoNoAsignado(term);
        }

        public object ObtenerBarrenadorasAutocomplete(string term)
        {
            return barrenacionDAO.ObtenerBarrenadorasAutocomplete(term);
        }

        public Dictionary<string, object> GuardarNuevaBarrenadora(int maquinaID)
        {
            return barrenacionDAO.GuardarNuevaBarrenadora(maquinaID);
        }
        #endregion

        #region Captura Diaria
        public Dictionary<string, object> ObtenerBarrenadorasCaptura(string areaCuenta, int turno, DateTime fecha)
        {
            return barrenacionDAO.ObtenerBarrenadorasCaptura(areaCuenta, turno, fecha);
        }

        public Dictionary<string, object> GuardarCapturaDiaria(List<tblB_CapturaDiaria> listaCaptura, DateTime fecha)
        {
            return barrenacionDAO.GuardarCapturaDiaria(listaCaptura, fecha);
        }
        #endregion

        #region Reparación
        public Dictionary<string, object> ObtenerPiezasPorReparar()
        {
            return barrenacionDAO.ObtenerPiezasPorReparar();
        }
        public bool ActualizarPiezaEstadoReparacion(TipoMovimientoPiezaEnum tm, int id, string comentario)
        {
            return barrenacionDAO.ActualizarPiezaEstadoReparacion(tm, id, comentario);
        }
        #endregion

        #region Catálogo de Piezas
        public Dictionary<string, object> ObtenerInsumosPorPieza(string areaCuenta)
        {
            return barrenacionDAO.ObtenerInsumosPorPieza(areaCuenta);
        }

        public Dictionary<string, object> AgregarInsumoPieza(tblB_CatalogoPieza nuevoInsumoPieza)
        {
            return barrenacionDAO.AgregarInsumoPieza(nuevoInsumoPieza);
        }

        public Dictionary<string, object> EliminarInsumoPieza(int id)
        {
            return barrenacionDAO.EliminarInsumoPieza(id);
        }
        #endregion

        #region Reporte de captura diaria
        public Dictionary<string, object> CargarCapturasDiarias(List<string> areaCuenta, List<int> barrenadoraID, List<int> turnos, DateTime fechaInicio, DateTime fechaFin)
        {
            return barrenacionDAO.CargarCapturasDiarias(areaCuenta, barrenadoraID, turnos, fechaInicio, fechaFin);
        }

        public ReporteCapturaDTO ObtenerCapturaDiariaPorId(int capturaID)
        {
            return barrenacionDAO.ObtenerCapturaDiariaPorId(capturaID);
        }

        public Dictionary<string, object> ObtenerBarrenadorasPorCC(string cc)
        {
            return barrenacionDAO.ObtenerBarrenadorasPorCC(cc);
        }

        public List<ReporteGeneralCapturaDTO> ObtenerReporteGeneralCapturas(string areaCuenta, int barrenadoraID, DateTime fechaInicio, DateTime fechaFin)
        {
            return barrenacionDAO.ObtenerReporteGeneralCapturas(areaCuenta, barrenadoraID, fechaInicio, fechaFin);
        }

        public Dictionary<string, object> SaveOrUpdatePieza(List<tblB_PiezaBarrenadora> obj)
        {
            return barrenacionDAO.SaveOrUpdatePieza(obj);
        }
        #endregion

        #region Reporte de rendimiento por pieza
        public Dictionary<string, object> CargarRendimientoPiezas(string areaCuenta, List<int> tipoPieza, DateTime fechaInicio, DateTime fechaFin)
        {
            return barrenacionDAO.CargarRendimientoPiezas(areaCuenta, tipoPieza, fechaInicio, fechaFin);
        }

        public RendimientoPiezaDTO CargarRendimientoPieza(int piezaID, DateTime fechaInicio, DateTime fechaFin)
        {
            return barrenacionDAO.CargarRendimientoPieza(piezaID, fechaInicio, fechaFin);
        }
        #endregion

        #region Barrenacion Costo
        public Dictionary<string, object> GuardarBarrenacionCosto(tblB_BarrenacionCosto registroInformacion, List<tblB_BarrenacionCostoOtroDetalle> lstPiezaDetalle, List<tblB_BarrenacionCostoPiezaDetalle> lstPiezaOtroDetalle)
        {
            return barrenacionDAO.GuardarBarrenacionCosto(registroInformacion, lstPiezaDetalle, lstPiezaOtroDetalle);
        }
        public Dictionary<string, object> GetBarrenacionCosto()
        {
            return barrenacionDAO.GetBarrenacionCosto();
        }
        #endregion

        #region Catalógo de Banco
        public Dictionary<string, object> AgregarBanco(tblB_CatalogoBanco nuevoBanco)
        {
            return barrenacionDAO.AgregarBanco(nuevoBanco);
        }

        public Dictionary<string, object> ObtenerBancos(string areaCuenta)
        {
            return barrenacionDAO.ObtenerBancos(areaCuenta);
        }
        #endregion

        public Dictionary<string, object> getInfoInsumo(int barrenadoraID, int insumo, string areaCuenta)
        {
            return barrenacionDAO.getInfoInsumo(barrenadoraID, insumo, areaCuenta);
        }

        public Dictionary<string, object> getPiezaNueva(int insumo, string areaCuenta)
        {
            return barrenacionDAO.getPiezaNueva(insumo, areaCuenta);
        }

        public Dictionary<string, object> setInfoCapturaDiaria(List<tblB_CapturaDiaria> listaCapturaDiaria, List<tblB_PiezaBarrenadora> listaPiezas)
        {
            return barrenacionDAO.setInfoCapturaDiaria(listaCapturaDiaria, listaPiezas);
        }

        public Dictionary<string, object> setReporteEjecutivo(DateTime fechaInicio, DateTime fechaFinal, List<string> areaCuenta, List<int> barrenadorasLista)
        {
            return barrenacionDAO.setReporteEjecutivo(fechaInicio, fechaFinal, areaCuenta, barrenadorasLista);
        }

        public Dictionary<string, object> ObtenerCapturaDiaria(int barrenadoraID, DateTime fechaActual, int turno)
        {
            return barrenacionDAO.ObtenerCapturaDiaria(barrenadoraID, fechaActual, turno);
        }
        public Dictionary<string, object> guardarAgua(string areaCuenta, int turno, string fechaCaptura, decimal litros, int id)
        {
            return barrenacionDAO.guardarAgua(areaCuenta, turno, fechaCaptura, litros, id);
        }

        public Dictionary<string, object> guardarOtrosPrecios(string areaCuenta, int turno, string fechaCaptura, decimal gasto, int id)
        {
            return barrenacionDAO.guardarOtrosPrecios(areaCuenta, turno, fechaCaptura, gasto, id);
        }

        public Dictionary<string, object> EliminarCaptura(int capturaID)
        {
            return barrenacionDAO.EliminarCaptura(capturaID);
        }
        public Dictionary<string, object> CargarRptGeneralCapturas(List<string> areaCuenta, List<int> turno, DateTime fechaInicio, DateTime fechaFin)
        {
            return barrenacionDAO.CargarRptGeneralCapturas(areaCuenta, turno, fechaInicio, fechaFin);
        }

        public Dictionary<string, object> ObtenerOperadores(string areaCuenta)
        {
            return barrenacionDAO.ObtenerOperadores(areaCuenta);
        }

        public Dictionary<string, object> CargarRptOperadores(List<string> areaCuenta, List<int> claveEmpleados, DateTime fechaInicio, DateTime fechaFin)
        {
            return barrenacionDAO.CargarRptOperadores(areaCuenta, claveEmpleados, fechaInicio, fechaFin);
        }
        public Dictionary<string, object> CargarRptEquiposstanby(List<string> areaCuenta, DateTime fechaInicio, DateTime fechaFin)
        {
            return barrenacionDAO.CargarRptEquiposstanby(areaCuenta, fechaInicio, fechaFin);
        }

        public Dictionary<string, object> guardarPagoMensual(string areaCuenta, DateTime fechaCaptura, decimal cantidad, int id)
        {
            return barrenacionDAO.guardarPagoMensual(areaCuenta, fechaCaptura, cantidad, id);
        }

        public Dictionary<string, object> SaveOrUpdatePiezasBarrenadora(List<tblB_PiezaBarrenadora> listaPzas, bool existePza)
        {
            return barrenacionDAO.SaveOrUpdatePiezasBarrenadora(listaPzas, existePza);
        }

        public Dictionary<string, object> SaveOrUpdateCapturaDiaria(List<tblB_CapturaDiaria> listaCapturaDiaria, List<tblB_PiezaBarrenadora> listaPiezas)
        {
            return barrenacionDAO.SaveOrUpdateCapturaDiaria(listaCapturaDiaria, listaPiezas);
        }
        public string getCCDescByAC(string AC)
        {
            return barrenacionDAO.getCCDescByAC(AC);
        }

    }
}

