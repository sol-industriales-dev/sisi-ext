using Core.DAO.Maquinaria.Reporte;
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

namespace Core.Service.Maquinaria.Reporte
{
    public class RentabilidadServices : IRentabilidadDAO
    {
        #region Atributos
        private IRentabilidadDAO m_rentabilidadDAO { get; set; }
        #endregion
        #region Propiedades
        public IRentabilidadDAO RentabilidadDAO
        {
            get { return m_rentabilidadDAO; }
            set { m_rentabilidadDAO = value; }
        }
        #endregion
        #region Contructores
        public RentabilidadServices(IRentabilidadDAO rentabilidadDAO)
        {
            RentabilidadDAO = rentabilidadDAO;
        }
        #endregion
        public List<RentabilidadDTO> getLstRentabilidad(BusqRentabilidadDTO busq)
        {
            return RentabilidadDAO.getLstRentabilidad(busq);
        }
        public List<RentabilidadDTO> getLstRentabilidadDetalle(BusqRentabilidadDTO busq)
        {
            return RentabilidadDAO.getLstRentabilidadDetalle(busq);
        }
        #region Combobox
        public List<ComboDTO> cboTipo()
        {
            return RentabilidadDAO.cboTipo();
        }
        public List<ComboDTO> cboGrupo(BusqRentabilidadDTO busq)
        {
            return RentabilidadDAO.cboGrupo(busq);
        }
        public List<ComboDTO> cboModelo(BusqRentabilidadDTO busq)
        {
            return RentabilidadDAO.cboModelo(busq);
        }
        public List<ComboDTO> cboMaquina(BusqRentabilidadDTO busq)
        {
            return RentabilidadDAO.cboMaquina(busq);
        }
        #endregion


        public string ObtenerModeloEconomico(string noEconomico)
        {
            return RentabilidadDAO.ObtenerModeloEconomico(noEconomico);
        }

        public List<ComboDTO> getListaCCByUsuario(int usuarioID, int tipo)
        {
            return RentabilidadDAO.getListaCCByUsuario(usuarioID, tipo);
        }

        public List<ComboDTO> getListaCC()
        {
            return RentabilidadDAO.getListaCC();
        }

        public decimal ObtenerObraCostoHorario(string noEconomico)
        {
            return RentabilidadDAO.ObtenerObraCostoHorario(noEconomico);
        }

        #region Analisis
        public List<RentabilidadDTO> getLstAnalisis(BusqAnalisisDTO busq)
        {
            return RentabilidadDAO.getLstAnalisis(busq);
        }
        #endregion

        #region Kubrix
        public List<RentabilidadDTO> getLstKubrix(BusqKubrixDTO busq)
        {
            return RentabilidadDAO.getLstKubrix(busq);
        }
        public List<RentabilidadDTO> getLstKubrixConstruplan(BusqKubrixDTO busq)
        {
            return RentabilidadDAO.getLstKubrixConstruplan(busq);
        }
        public List<Tuple<int, string>> getRelacionEcoTipo(List<string> economicos)
        {
            return RentabilidadDAO.getRelacionEcoTipo(economicos);
        }
        public List<RentabilidadDTO> getLstKubrixDetalle(BusqKubrixDTO busq)
        {
            return RentabilidadDAO.getLstKubrixDetalle(busq);
        }
        public List<RentabilidadDTO> getLstKubrixIngresosPendientes(BusqKubrixDTO busq)
        {
            return RentabilidadDAO.getLstKubrixIngresosPendientes(busq);
        }
        public List<tblM_CatMaquina> fillComboMaquinaria(int grupoID, List<int> modeloID)
        {
            return RentabilidadDAO.fillComboMaquinaria(grupoID, modeloID);
        }
        public List<tblM_CatGrupoMaquinaria> FillGrupoEquipo()
        {
            return RentabilidadDAO.FillGrupoEquipo();
        }

        public List<tblM_CatModeloEquipo> FillModeloEquipo(int grupoID)
        {
            return RentabilidadDAO.FillModeloEquipo(grupoID);
        }
        public List<RentabilidadDTO> getLstKubrixIngresosEstimacion(BusqKubrixDTO busq, bool corte)
        {
            return RentabilidadDAO.getLstKubrixIngresosEstimacion(busq, corte);
        }
        public List<RentabilidadDTO> getLstKubrixIngresosPendientesGenerar(BusqKubrixDTO busq, int usuarioID, bool corte)
        {
            return RentabilidadDAO.getLstKubrixIngresosPendientesGenerar(busq, usuarioID, corte);
        }
        public List<string> getFletesActivos()
        {
            return RentabilidadDAO.getFletesActivos();
        }
        public List<tblM_KBDivision> getDivisiones()
        {
            return RentabilidadDAO.getDivisiones();
        }
        public List<tblM_KBUsuarioResponsable> getResponsabilesAC(int usuarioID)
        {
            return RentabilidadDAO.getResponsabilesAC(usuarioID);
        }
                public List<string> getACDivision(int divisionID)
        {
            return RentabilidadDAO.getACDivision(divisionID);
        }

        public List<string> getACResponsable(int responsableID)
        {
            return RentabilidadDAO.getACResponsable(responsableID);
        }
        public bool checkResponsable(int usuarioID, int responsableID)
        {
            return RentabilidadDAO.checkResponsable(usuarioID, responsableID);
        }
        public bool guardarLstCC(List<string> listaCC)
        {
            return RentabilidadDAO.guardarLstCC(listaCC);
        }
        #endregion

        #region Corte
        public int guardarCorteArrendadora(int usuario, int tipo)
        {
            return RentabilidadDAO.guardarCorteArrendadora(usuario, tipo);
        }
        public int guardarCorteConstruplan(int usuario, int tipoCorte)
        {
            return RentabilidadDAO.guardarCorteConstruplan(usuario, tipoCorte);
        }
        public List<DateTime> getLstFechasCortes(int tipo)
        {
            return RentabilidadDAO.getLstFechasCortes(tipo);
        }
        public List<tblM_KBCorte> getCortesPorFecha(DateTime fecha, int tipo)
        {
            return RentabilidadDAO.getCortesPorFecha(fecha, tipo);
        }
        public List<tblM_KBCorte> getCortesAnt(DateTime fecha, int tipo)
        {
            return RentabilidadDAO.getCortesAnt(fecha, tipo);
        }
        public List<tblM_KBCorte> getCortesPorMes(DateTime fecha)
        {
            return RentabilidadDAO.getCortesPorMes(fecha);
        }
        public List<CortePpalDTO> getLstKubrixCorte(int corteID, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, int semana, int usuarioID, bool reporteCostos)
        {
            return RentabilidadDAO.getLstKubrixCorte(corteID, areaCuenta, modelos, economico, fechaInicio, fechaFin, semana, usuarioID, reporteCostos);
        }
        public List<CortePpalDTO> getLstKubrixCortes(List<int> cortes, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaInicio, List<DateTime> fechasFin, int usuarioID, bool reporteCostos)
        {
            return RentabilidadDAO.getLstKubrixCortes(cortes, areaCuenta, modelos, economico, fechaInicio, fechasFin, usuarioID, reporteCostos);
        }
        public List<CortePpalDTO> getLstKubrixCortesAnteriores(int anio, List<string> areaCuenta, List<int> modelos, string economico, int usuarioID, bool reporteCostos)
        {
            return RentabilidadDAO.getLstKubrixCortesAnteriores(anio, areaCuenta, modelos, economico, usuarioID, reporteCostos);
        }
        public List<ComboDTO> getEconomicoEstatus(List<string> economicos)
        {
            return RentabilidadDAO.getEconomicoEstatus(economicos);
        }
        public List<CorteDTO> getLstKubrixCorteDet(int corteID, int tipo, int columna, int conceptoCuentas, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, int semana, int usuarioID, string division, string areaCuentaCol, string economicoCol, bool reporteCostos)
        {
            return RentabilidadDAO.getLstKubrixCorteDet(corteID, tipo, columna, conceptoCuentas, modelos, economico, fechaInicio, fechaFin, areaCuenta, semana, usuarioID, division, areaCuentaCol, economicoCol, reporteCostos);
        }
        public List<CorteDTO> getLstKubrixCorteDetCompleto(int corteID, int tipo, int columna, int conceptoCuentas, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, int semana, int usuarioID, string division, string areaCuentaCol, string economicoCol, bool reporteCostos)
        {
            return RentabilidadDAO.getLstKubrixCorteDetCompleto(corteID, tipo, columna, conceptoCuentas, modelos, economico, fechaInicio, fechaFin, areaCuenta, semana, usuarioID, division, areaCuentaCol, economicoCol, reporteCostos);
        }
        public List<CorteDTO> getLstKubrixCorteDetCompleto(int corteID, int tipo, int columna, int renglon, List<int> modelos, string economico, 
            DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, int semana, int usuarioID, string divisionCol, string areaCuentaCol,
            string economicoCol, string subcuentaFiltro, string subsubcuentaFiltro, string divisionFiltro, string areaCuentaFiltro, string conciliacionFiltro, string economicoFiltro, int empresa, bool reporteCostos)
        {
            return RentabilidadDAO.getLstKubrixCorteDetCompleto(corteID, tipo, columna, renglon, modelos, economico, fechaInicio, fechaFin, areaCuenta, semana, usuarioID, divisionCol, areaCuentaCol, economicoCol, subcuentaFiltro, subsubcuentaFiltro, divisionFiltro, areaCuentaFiltro, conciliacionFiltro, economicoFiltro, empresa, reporteCostos);
        }
        public List<CorteDTO> getLstKubrixCorteDetCompletoCplan(int corteID, int tipo, int columna, int renglon, List<int> modelos, string economico,
            DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, int semana, int usuarioID, string divisionCol, string areaCuentaCol,
            string economicoCol, string subcuentaFiltro, string subsubcuentaFiltro, string divisionFiltro, string areaCuentaFiltro, string conciliacionFiltro, string economicoFiltro, int empresa, bool reporteCostos, int acumulado)
        {
            return RentabilidadDAO.getLstKubrixCorteDetCompletoCplan(corteID, tipo, columna, renglon, modelos, economico, fechaInicio, fechaFin, areaCuenta, semana, usuarioID, divisionCol, areaCuentaCol, economicoCol, subcuentaFiltro, subsubcuentaFiltro, divisionFiltro, areaCuentaFiltro, conciliacionFiltro, economicoFiltro, empresa, reporteCostos, acumulado);
        }
        public List<CorteDTO> getLstKubrixCorteAnterior(int corteID, int tipo, int columna, int renglon, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, int usuarioID, string divisionCol, string areaCuentaCol, string economicoCol, bool reporteCostos)
        {
            return RentabilidadDAO.getLstKubrixCorteAnterior(corteID, tipo, columna, renglon, modelos, economico, fechaInicio, fechaFin, areaCuenta, usuarioID, divisionCol, areaCuentaCol, economicoCol, reporteCostos);
        }
        public List<CorteDTO> getLstKubrixCorteCostoEstimado(int corteID, DateTime fechaInicio, DateTime fechaFin, int tipoGuardado)
        {
            return RentabilidadDAO.getLstKubrixCorteCostoEstimado(corteID, fechaInicio, fechaFin, tipoGuardado);
        }
        public List<ComboDTO> getCuentasDesc()
        {
            return RentabilidadDAO.getCuentasDesc();
        }
        public List<CorteDTO> getLstKubrixCorteActualDet(int corteID, int tipo, int columna, int conceptoCuentas, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, int semana, int usuarioID, List<CorteDTO> lstAnterior, string division, string areaCuentaCol, string economicoCol, bool reporteCostos)
        {
            return RentabilidadDAO.getLstKubrixCorteActualDet(corteID, tipo, columna, conceptoCuentas, modelos, economico, fechaInicio, fechaFin, areaCuenta, semana, usuarioID, lstAnterior, division, areaCuentaCol, economicoCol, reporteCostos);
        }
        public tblM_KBCorte getCorteByID(int corteID)
        {
            return RentabilidadDAO.getCorteByID(corteID);
        }
        public tblM_KBCorte getCorteByIDArrendadora()
        {
            return RentabilidadDAO.getCorteByIDArrendadora();
        }
        public List<tblM_KBCatCuenta> getCuentasDescripcion()
        {
            return RentabilidadDAO.getCuentasDescripcion();
        }
        public List<ComboDTO> getGrupoMaquinas()
        {
            return RentabilidadDAO.getGrupoMaquinas();
        }
        //public List<RentabilidadDTO> getLstKubrixDetalleCorte(int corteID, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, string cuenta, int tipo, int tipoEquipoMayor)
        //{
        //    return RentabilidadDAO.getLstKubrixDetalleCorte(corteID, areaCuenta, modelos, economico, fechaInicio, fechaFin, cuenta, tipo, tipoEquipoMayor);
        //}
        public bool GuardadoLineaCorte(int id, int corteID, string concepto, decimal monto, string cc, string ac, DateTime fecha, string conciliacion, int empresa, int tipoGuardado)
        {
            return RentabilidadDAO.GuardadoLineaCorte(id, corteID, concepto, monto, cc, ac, fecha, conciliacion, empresa, tipoGuardado);
        }
        public bool EliminarLineaCorte(int id)
        {
            return RentabilidadDAO.EliminarLineaCorte(id);
        }
        public bool CheckCostoEstimadoCerrado(int corteID)
        {
            return RentabilidadDAO.CheckCostoEstimadoCerrado(corteID);
        }
        public bool CerrarCostoEst(int corteID)
        {
            return RentabilidadDAO.CerrarCostoEst(corteID);
        }
        #endregion

        #region CXP
        public List<CuentasPendientesDTO> getLstCXP(BusqKubrixDTO busq)
        {
            return RentabilidadDAO.getLstCXP(busq);
        }
        public List<CuentasPendientesDTO> getLstCXC(BusqKubrixDTO busq)
        {
            return RentabilidadDAO.getLstCXC(busq);
        }
        public Dictionary<string, object> getLstCXCReporte(BusqKubrixDTO busq)
        {
            return RentabilidadDAO.getLstCXCReporte(busq);
        }

        public MemoryStream DescargarExcelCXC(List<CuentasPendientesDTO> data, DateTime fechaCorte)
        {
            return RentabilidadDAO.DescargarExcelCXC(data, fechaCorte);
        }
        #endregion

        #region Balanza
        public List<BalanzaKubrixDTO> getBalanza(DateTime fechaCorte, int empresa, int tipo)
        {
            return RentabilidadDAO.getBalanza(fechaCorte, empresa, tipo);
        }
        #endregion

        #region Catalógo Divisiones
        public Dictionary<string, object> SaveOrUpdateDivision(tblM_KBDivision nuevaDivision)
        {
            return RentabilidadDAO.SaveOrUpdateDivision(nuevaDivision);
        }
        public Dictionary<string, object> GetInfoDivision(int areaCuenta, bool estatus)
        {
            return RentabilidadDAO.GetInfoDivision(areaCuenta, estatus);
        }
        public List<ComboDTO> getComboAreaCuenta()
        {
            return RentabilidadDAO.getComboAreaCuenta();
        }
        public Dictionary<string, object> getDivisionByID(int divisionID)
        {
            return RentabilidadDAO.getDivisionByID(divisionID);
        }
        public tblM_KBDivision getDivisionByNombre(string nombreDivision)
        {
            return RentabilidadDAO.getDivisionByNombre(nombreDivision);
        }
        public Dictionary<string, object> bajaDivision(int divisionID)
        {
            return RentabilidadDAO.bajaDivision(divisionID);
        }
        public List<tblM_KBDivisionDetalle> getDivisionesDetalle()
        {
            return RentabilidadDAO.getDivisionesDetalle();
        }
        #endregion

        #region Catalogo Fletes

        public Dictionary<string, object> SaveOrUpdateFlete(tblM_KBFletes nuevo)
        {
            return RentabilidadDAO.SaveOrUpdateFlete(nuevo);
        }
        public Dictionary<string, object> GetInfoFletes()
        {
            return RentabilidadDAO.GetInfoFletes();
        }

        public Dictionary<string, object> CboEconomico()
        {
            return RentabilidadDAO.CboEconomico();
        }

        #endregion

        #region Administracion de usuarios por centro Costos

        public Dictionary<string, object> SaveOrUpdateAdministacionUsuarios(tblM_KBUsuarioResponsable nuevoUsuario)
        {
            return RentabilidadDAO.SaveOrUpdateAdministacionUsuarios(nuevoUsuario);
        }
        public Dictionary<string, object> GetInfoAdministracionUsuarios(int estatus)
        {
            return RentabilidadDAO.GetInfoAdministracionUsuarios(estatus);
        }

        public Dictionary<string, object> getReponsableByID(int id)
        {
            return RentabilidadDAO.getReponsableByID(id);
        }

        public Dictionary<string, object> getListaUsuarios()
        {
            return RentabilidadDAO.getListaUsuarios();
        }

        public   Dictionary<string, object> bajaUsuario(int id)
        {
            return RentabilidadDAO.bajaUsuario(id);
        }

        #endregion

        #region Reportes autogenerados
        public MemoryStream crearExcelSaldosCplan(DateTime fechaCorte)
        {
            return RentabilidadDAO.crearExcelSaldosCplan(fechaCorte);
        }
        public MemoryStream crearExcelSaldosColombia(DateTime fechaCorte)
        {
            return RentabilidadDAO.crearExcelSaldosColombia(fechaCorte);
        }
        public MemoryStream crearExcelSaldosCplanEici(DateTime fechaCorte)
        {
            return RentabilidadDAO.crearExcelSaldosCplanEici(fechaCorte);
        }
        public MemoryStream crearExcelClientesCplan(DateTime fechaCorte, int tipo)
        {
            return RentabilidadDAO.crearExcelClientesCplan(fechaCorte, tipo);
        }
        public MemoryStream crearExcelVencimientosCplan(DateTime fechaCorte)
        {
            return RentabilidadDAO.crearExcelVencimientosCplan(fechaCorte);
        }
        public MemoryStream crearExcelSaldosCplanIntegradora(DateTime fechaCorte)
        {
            return RentabilidadDAO.crearExcelSaldosCplanIntegradora(fechaCorte);
        }

        public MemoryStream crearExcelSemanalKubrix(int corteID, int tipo, List<string> areaCuenta)
        {
            return RentabilidadDAO.crearExcelSemanalKubrix(corteID, tipo, areaCuenta);
        }
        public MemoryStream crearExcelPorSubcuentaKubrix(int corteID, int tipo, List<string> areaCuenta)
        {
            return RentabilidadDAO.crearExcelPorSubcuentaKubrix(corteID, tipo, areaCuenta);
        }
        public List<byte[]> ExcelSaldosCplan(int anio, EnkontrolEnum empresa)
        {
            return RentabilidadDAO.ExcelSaldosCplan(anio, empresa);
        }
        public byte[] ExcelSaldosCplanVirtual(int anio)
        {
            return RentabilidadDAO.ExcelSaldosCplanVirtual(anio);
        }
        public byte[] ExcelClientesCplan(EnkontrolEnum empresa)
        {
            return RentabilidadDAO.ExcelClientesCplan(empresa);
        }
        public byte[] ExcelVencimientosCplan(EnkontrolEnum empresa)
        {
            return RentabilidadDAO.ExcelVencimientosCplan(empresa);
        }
        public byte[] ExcelSemanalKubrix(int corteID)
        {
            return RentabilidadDAO.ExcelSemanalKubrix(corteID);
        }
        public byte[] ExcelPorSubcuentaKubrix(int corteID)
        {
            return RentabilidadDAO.ExcelPorSubcuentaKubrix(corteID);
        }
        public byte[] ExcelEntradasSemanas(DetallesSemanalDTO detalles)
        {
            return RentabilidadDAO.ExcelEntradasSemanas(detalles);
        }
        public List<DetallesSemanalDTO> GetMovimientosNuevos(int corteID, List<string> areasCuenta)
        {
            return RentabilidadDAO.GetMovimientosNuevos(corteID, areasCuenta);
        }
        #endregion

        #region subirExcelEstimados

        public int CargarExcelEstimados(byte[] bin, int corteID)
        {
            return RentabilidadDAO.CargarExcelEstimados(bin, corteID);
        }
        public int CargarExcelEstimados(byte[] bin)
        {
            return RentabilidadDAO.CargarExcelEstimados(bin);
        }
        #endregion

        #region Flujo Efectivo 
        
        public List<tblM_KBCorteDet> GetEstimadosArrendadora()
        {
            return RentabilidadDAO.GetEstimadosArrendadora();
        }

        public int GuardarEstimadosArrendadora()
        {
            return RentabilidadDAO.GuardarEstimadosArrendadora();
        }
        public string GetCCByAC(string AC)
        {
            return RentabilidadDAO.GetCCByAC(AC);
        }
        #endregion

        public List<CortesDetDTO> obtenerCortesArrendadora(int corteID, DateTime fechaFin)
        {
            return RentabilidadDAO.obtenerCortesArrendadora(corteID, fechaFin);
        }
        public List<tblP_CC> obtenerCentrosCostos()
        {
            return RentabilidadDAO.obtenerCentrosCostos();
        }
        #region Stored Procedures Kubrix
        public List<CorteDetDTO> getLstKubrixCortesArrendadora(int corteID, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaFin, bool reporteCostos, int acumulado)
        {
            return RentabilidadDAO.getLstKubrixCortesArrendadora(corteID, areaCuenta, modelos, economico, fechaFin, reporteCostos, acumulado);
        }
        public List<CorteDTO> getLstKubrixCortesArrendadoraDetalle(int corteID, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaFin, bool reporteCostos, int acumulado, int concepto, int semana, bool aplicaSinDivision, bool aplicaCompacto)
        {
            return RentabilidadDAO.getLstKubrixCortesArrendadoraDetalle(corteID, areaCuenta, modelos, economico, fechaFin, reporteCostos, acumulado, concepto, semana, aplicaSinDivision, aplicaCompacto);
        }
        public List<CorteDetDTO> getLstKubrixCortesConstruplan(int corteID, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaFin, bool reporteCostos, int acumulado)
        {
            return RentabilidadDAO.getLstKubrixCortesConstruplan(corteID, areaCuenta, modelos, economico, fechaFin, reporteCostos, acumulado);
        }
        public List<CorteDetDTO> getLstKubrixCortesArrendadoraCentrosCostos(int corteID, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaFin, bool reporteCostos, int acumulado)
        {
            return RentabilidadDAO.getLstKubrixCortesArrendadoraCentrosCostos(corteID, areaCuenta, modelos, economico, fechaFin, reporteCostos, acumulado);
        }
        
        #endregion

        public List<ComboDTO> getListaCCConstruplan(int usuarioID)
        {
            return RentabilidadDAO.getListaCCConstruplan(usuarioID);
        }
        public List<ComboDTO> getListaFechasConstruplan(int tipoCorte)
        {
            return RentabilidadDAO.getListaFechasConstruplan(tipoCorte);
        }
        public Dictionary<string, object> cargarInformacionNivel1(int corteID, List<string> listaCC, int usuarioID)
        {
            return RentabilidadDAO.cargarInformacionNivel1(corteID, listaCC, usuarioID);
        }
    }
}
