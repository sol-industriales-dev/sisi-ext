using Core.DTO.Maquinaria.Reporte.Rentabilidad;
using Core.DTO.Principal.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Maquinaria.Reporte.Analisis;
using Core.DTO.Maquinaria.Reporte.Kubrix;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Rentabilidad;
using Core.DTO.Maquinaria.Rentabilidad;
using System.IO;
using Core.Enum.Multiempresa;
using Core.DTO.Utils;
using Core.Entity.Principal.Multiempresa;

namespace Core.DAO.Maquinaria.Reporte
{
    public interface IRentabilidadDAO
    {
        List<RentabilidadDTO> getLstRentabilidad(BusqRentabilidadDTO busq);
        List<RentabilidadDTO> getLstRentabilidadDetalle(BusqRentabilidadDTO busq);
        #region combobox
        List<ComboDTO> cboTipo();
        List<ComboDTO> cboGrupo(BusqRentabilidadDTO busq);
        List<ComboDTO> cboModelo(BusqRentabilidadDTO busq);
        List<ComboDTO> cboMaquina(BusqRentabilidadDTO busq);
        #endregion

        /// <summary>
        /// Obtiene la descripción del modelo de un equipo según el número económico.
        /// </summary>
        /// <param name="noEconomico"></param>
        /// <returns></returns>
        string ObtenerModeloEconomico(string noEconomico);
        decimal ObtenerObraCostoHorario(string noEconomico);
        List<ComboDTO> getListaCCByUsuario(int usuarioID, int tipo);

        List<ComboDTO> getListaCC();

        #region Analisis
        List<RentabilidadDTO> getLstAnalisis(BusqAnalisisDTO busq);
        #endregion

        #region Kubrix
        List<RentabilidadDTO> getLstKubrix(BusqKubrixDTO busq);
        List<RentabilidadDTO> getLstKubrixConstruplan(BusqKubrixDTO busq);
        List<Tuple<int, string>> getRelacionEcoTipo(List<string> economicos);
        List<RentabilidadDTO> getLstKubrixDetalle(BusqKubrixDTO busq);
        List<RentabilidadDTO> getLstKubrixIngresosPendientes(BusqKubrixDTO busq);
        List<tblM_CatMaquina> fillComboMaquinaria(int grupoID, List<int> modeloID);
        List<tblM_CatGrupoMaquinaria> FillGrupoEquipo();
        List<tblM_CatModeloEquipo> FillModeloEquipo(int grupoID);
        List<RentabilidadDTO> getLstKubrixIngresosEstimacion(BusqKubrixDTO busq, bool corte);
        List<RentabilidadDTO> getLstKubrixIngresosPendientesGenerar(BusqKubrixDTO busq, int usuarioID, bool corte);
        List<string> getFletesActivos();
        List<tblM_KBDivision> getDivisiones();
        List<tblM_KBUsuarioResponsable> getResponsabilesAC(int usuarioID);
        List<string> getACDivision(int divisionID);
        List<string> getACResponsable(int responsableID);
        bool checkResponsable(int usuarioID, int responsableID);
        bool guardarLstCC(List<string> listaCC);
        #endregion

        #region Corte
        int guardarCorteArrendadora(int usuario, int tipo);
        int guardarCorteConstruplan(int usuario, int tipoCorte);
        List<DateTime> getLstFechasCortes(int tipo);
        List<tblM_KBCorte> getCortesPorFecha(DateTime fecha, int tipo);
        List<tblM_KBCorte> getCortesAnt(DateTime fecha, int tipo);
        List<tblM_KBCorte> getCortesPorMes(DateTime fecha);
        List<CortePpalDTO> getLstKubrixCorte(int corteID, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, int semana, int usuarioID, bool reporteCostos);
        List<CortePpalDTO> getLstKubrixCortes(List<int> cortes, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaInicio, List<DateTime> fechasFin, int usuarioID, bool reporteCostos);
        List<CortePpalDTO> getLstKubrixCortesAnteriores(int anio, List<string> areaCuenta, List<int> modelos, string economico, int usuarioID, bool reporteCostos);
        List<ComboDTO> getEconomicoEstatus(List<string> economicos);
        List<CorteDTO> getLstKubrixCorteDet(int corteID, int tipo, int columna, int conceptoCuentas, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, int semana, int usuarioID, string division, string areaCuentaCol, string economicoCol, bool reporteCostos);
        List<CorteDTO> getLstKubrixCorteDetCompleto(int corteID, int tipo, int columna, int conceptoCuentas, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, int semana, int usuarioID, string division, string areaCuentaCol, string economicoCol, bool reporteCostos);
        List<CorteDTO> getLstKubrixCorteDetCompleto(int corteID, int tipo, int columna, int renglon, List<int> modelos, string economico,
            DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, int semana, int usuarioID, string divisionCol, string areaCuentaCol,
            string economicoCol, string subcuentaFiltro, string subsubcuentaFiltro, string divisionFiltro, string areaCuentaFiltro, string conciliacionFiltro, string economicoFiltro, int empresa, bool reporteCostos);
        List<CorteDTO> getLstKubrixCorteDetCompletoCplan(int corteID, int tipo, int columna, int renglon, List<int> modelos, string economico,
            DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, int semana, int usuarioID, string divisionCol, string areaCuentaCol,
            string economicoCol, string subcuentaFiltro, string subsubcuentaFiltro, string divisionFiltro, string areaCuentaFiltro, string conciliacionFiltro, string economicoFiltro, int empresa, bool reporteCostos, int acumulado);
        List<CorteDTO> getLstKubrixCorteAnterior(int corteID, int tipo, int columna, int renglon, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, int usuarioID, string divisionCol, string areaCuentaCol, string economicoCol, bool reporteCostos);
        List<CorteDTO> getLstKubrixCorteCostoEstimado(int corteID, DateTime fechaInicio, DateTime fechaFin, int tipoGuardado);
        List<ComboDTO> getCuentasDesc();
        List<CorteDTO> getLstKubrixCorteActualDet(int corteID, int tipo, int columna, int conceptoCuentas, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, int semana, int usuarioID, List<CorteDTO> lstAnterior, string division, string areaCuentaCol, string economicoCol, bool reporteCostos);
        tblM_KBCorte getCorteByID(int corteID);
        tblM_KBCorte getCorteByIDArrendadora();
        List<tblM_KBCatCuenta> getCuentasDescripcion();
        List<ComboDTO> getGrupoMaquinas();
        //List<RentabilidadDTO> getLstKubrixDetalleCorte(int corteID, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, string cuenta, int tipo, int tipoEquipoMayor);
        bool GuardadoLineaCorte(int id, int corteID, string concepto, decimal monto, string cc, string ac, DateTime fecha, string conciliacion, int empresa, int tipoGuardado);
        bool EliminarLineaCorte(int id);
        bool CheckCostoEstimadoCerrado(int corteID);
        bool CerrarCostoEst(int corteID);
        #endregion

        #region CXP
        List<CuentasPendientesDTO> getLstCXP(BusqKubrixDTO busq);
        List<CuentasPendientesDTO> getLstCXC(BusqKubrixDTO busq);
        Dictionary<string, object> getLstCXCReporte(BusqKubrixDTO busq);
        MemoryStream DescargarExcelCXC(List<CuentasPendientesDTO> data, DateTime fechaCorte);
        #endregion

        #region Balanza
        List<BalanzaKubrixDTO> getBalanza(DateTime fechaCorte, int empresa, int tipo);
        #endregion

        #region Catalólo Divisiones
        Dictionary<string, object> SaveOrUpdateDivision(tblM_KBDivision nuevaDivision);
        Dictionary<string, object> GetInfoDivision(int areaCuenta, bool estatus);
        List<ComboDTO> getComboAreaCuenta();
        Dictionary<string, object> getDivisionByID(int divisionID);
        tblM_KBDivision getDivisionByNombre(string nombreDivision);
        Dictionary<string, object> bajaDivision(int divisionID);
        List<tblM_KBDivisionDetalle> getDivisionesDetalle();

        #endregion

        #region Catalogo Fletes
        Dictionary<string, object> SaveOrUpdateFlete(tblM_KBFletes nuevo);
        Dictionary<string, object> GetInfoFletes();
        Dictionary<string, object> CboEconomico();
        #endregion

        #region Administracion de usuarios por centro de costos
        Dictionary<string, object> SaveOrUpdateAdministacionUsuarios(tblM_KBUsuarioResponsable nuevoUsuario);
        Dictionary<string, object> GetInfoAdministracionUsuarios(int estatus);
        Dictionary<string, object> getReponsableByID(int id);
        Dictionary<string, object> getListaUsuarios();
        Dictionary<string, object> bajaUsuario(int id);
        #endregion
        #region Reportes autogenerados
        MemoryStream crearExcelSaldosCplan(DateTime fechaCorte);
        MemoryStream crearExcelSaldosColombia(DateTime fechaCorte);
        MemoryStream crearExcelSaldosCplanEici(DateTime fechaCorte);
        MemoryStream crearExcelClientesCplan(DateTime fechaCorte, int tipo);
        MemoryStream crearExcelVencimientosCplan(DateTime fechaCorte);
        MemoryStream crearExcelSaldosCplanIntegradora(DateTime fechaCorte);

        MemoryStream crearExcelSemanalKubrix(int corteID, int tipo, List<string> areaCuenta);
        MemoryStream crearExcelPorSubcuentaKubrix(int corteID, int tipo, List<string> areaCuenta);

        List<byte[]> ExcelSaldosCplan(int anio, EnkontrolEnum empresa);
        byte[] ExcelSaldosCplanVirtual(int anio);
        byte[] ExcelClientesCplan(EnkontrolEnum empresa);
        byte[] ExcelVencimientosCplan(EnkontrolEnum empresa);
        byte[] ExcelSemanalKubrix(int corteID);
        byte[] ExcelPorSubcuentaKubrix(int corteID);
        byte[] ExcelEntradasSemanas(DetallesSemanalDTO detalles);
        List<DetallesSemanalDTO> GetMovimientosNuevos(int corteID, List<string> areasCuenta);
        
        #endregion
        #region subirExcelEstimados
        int CargarExcelEstimados(byte[] bin, int corteID);
        int CargarExcelEstimados(byte[] bin);
        #endregion

        #region Flujo Efectivo
        List<tblM_KBCorteDet> GetEstimadosArrendadora();
        int GuardarEstimadosArrendadora();
        #endregion

        string GetCCByAC(string AC);
        List<CortesDetDTO> obtenerCortesArrendadora(int corteID, DateTime fechaFin);
        List<tblP_CC> obtenerCentrosCostos();
        #region Stored Procedures Kubrix
        List<CorteDetDTO> getLstKubrixCortesArrendadora(int corteID, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaFin, bool reporteCostos, int acumulado);
        List<CorteDTO> getLstKubrixCortesArrendadoraDetalle(int corteID, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaFin, bool reporteCostos, int acumulado, int concepto, int semana, bool aplicaSinDivision, bool aplicaCompacto);
        List<CorteDetDTO> getLstKubrixCortesConstruplan(int corteID, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaFin, bool reporteCostos, int acumulado);
        List<CorteDetDTO> getLstKubrixCortesArrendadoraCentrosCostos(int corteID, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaFin, bool reporteCostos, int acumulado);
        
        #endregion

        #region Nuevas Funciones para Kubrix Cplan
        List<ComboDTO> getListaCCConstruplan(int usuarioID);
        List<ComboDTO> getListaFechasConstruplan(int tipoCorte);
        Dictionary<string, object> cargarInformacionNivel1(int corteID, List<string> listaCC, int usuarioID);
        #endregion
    }
}
