using Core.DAO.Maquinaria.Reporte.ActivoFijo;
using Core.DTO;
using Core.DTO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Reporte.ActivoFijo;
using Core.DTO.Maquinaria.Reporte.ActivoFijo.Colombia.Cedula;
using Core.DTO.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion;
using Core.DTO.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion.CapturaEnkontrol;
using Core.Entity.Maquinaria.Reporte.ActivoFijo;
using Core.Entity.Maquinaria.Reporte.ActivoFijo.Peru;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoServices: IActivoFijoDAO
    {
        private IActivoFijoDAO m_activoFijo;

        public IActivoFijoDAO ActivoFijoDAO
        {
            get { return m_activoFijo; }
            set { m_activoFijo = value; }
        }

        public ActivoFijoServices(IActivoFijoDAO activoFijo)
        {
            this.ActivoFijoDAO = activoFijo;
        }

        public Respuesta AddInsumo(tblC_AF_InsumosOverhaul insumo)
        {
            return this.ActivoFijoDAO.AddInsumo(insumo);
        }

        public Respuesta GetInsumos()
        {
            return this.ActivoFijoDAO.GetInsumos();
        }
        public Respuesta UpdateInsumos(List<tblC_AF_InsumosOverhaul> insumos)
        {
            return this.ActivoFijoDAO.UpdateInsumos(insumos);
        }

        public Respuesta AgregarInsumosAutomaticamente()
        {
            return this.ActivoFijoDAO.AgregarInsumosAutomaticamente();
        }

        #region Póliza de depreciación
        public Respuesta FechaCaptura(int cuenta, bool esOverhaul)
        {
            return this.ActivoFijoDAO.FechaCaptura(cuenta, esOverhaul);
        }

        public Respuesta EliminarPoliza(int polizaId)
        {
            return this.ActivoFijoDAO.EliminarPoliza(polizaId);
        }

        public Respuesta RegistrarPoliza(List<PolizaGeneradaDTO> polizaGenerada)
        {
            return this.ActivoFijoDAO.RegistrarPoliza(polizaGenerada);
        }

        public Respuesta GenerarPoliza(int cuenta, int año, int mes, int semana, int dia, bool esOverhaul, int? idCuentaDepOverhaul)
        {
            return this.ActivoFijoDAO.GenerarPoliza(cuenta, año, mes, semana, dia, esOverhaul, idCuentaDepOverhaul);
        }

        public Respuesta ObtenerPolizaDetalle(int año, int mes, int poliza)
        {
            return this.ActivoFijoDAO.ObtenerPolizaDetalle(año, mes, poliza);
        }

        public Respuesta ObtenerPolizasDepreciacion(int año, int mes, int cuenta)
        {
            return this.ActivoFijoDAO.ObtenerPolizasDepreciacion(año, mes, cuenta);
        }

        public Dictionary<string, object> CuentasDepOverhaul()
        {
            return this.ActivoFijoDAO.CuentasDepOverhaul();
        }
        #endregion

        public bool PolizasCapturadas()
        {
            return this.ActivoFijoDAO.PolizasCapturadas();
        }

        public Respuesta DepreciacionNumEconomico(string noEconomico, DateTime fechaHasta)
        {
            return this.ActivoFijoDAO.DepreciacionNumEconomico(noEconomico, fechaHasta);
        }
        public List<DepreciacionMaquinaConOverhaulDTO> DepreciacionNumEconomicoDTO(List<StandByEcosDTO> noEconomico, DateTime fechaInicio, DateTime fechaHasta)
        {
            return this.ActivoFijoDAO.DepreciacionNumEconomicoDTO(noEconomico, fechaInicio, fechaHasta);
        }

        public Respuesta CalcularEnviosACosto()
        {
            return this.ActivoFijoDAO.CalcularEnviosACosto();
        }

        public Respuesta GenerarPolizaCosto(List<int> idEnvioCosto)
        {
            return this.ActivoFijoDAO.GenerarPolizaCosto(idEnvioCosto);
        }

        public Dictionary<string, object> RegistrarPolizaCosto(PolizaCapturaEnkontrolDTO infoPoliza)
        {
            return this.ActivoFijoDAO.RegistrarPolizaCosto(infoPoliza);
        }

        public Respuesta GenerarPolizaCostoPorInsumo(int idCatMaqDepreciacion)
        {
            return this.ActivoFijoDAO.GenerarPolizaCostoPorInsumo(idCatMaqDepreciacion);
        }

        public Dictionary<string, object> RelacionAutomaticaPolizas()
        {
            return this.ActivoFijoDAO.RelacionAutomaticaPolizas();
        }

        public Dictionary<string, object> getDetalleExcel(List<ActivoFijoDetalleCuentaDTO> datosExcel, DateTime fechaHasta, List<CedulaColombiaDTO> colombia)
        {
            return this.ActivoFijoDAO.getDetalleExcel(datosExcel, fechaHasta, colombia);
        }

        public Dictionary<string, object> GetResumen(DateTime fechaHasta)
        {
            return this.ActivoFijoDAO.GetResumen(fechaHasta);
        }

        public Dictionary<string, object> GetDetalleCuenta(DateTime fechaHasta, int cuenta)
        {
            return this.ActivoFijoDAO.GetDetalleCuenta(fechaHasta, cuenta);
        }

        public Dictionary<string, object> GetMaquinas(int idCuenta, int estatusMaquina, int tipoCaptura)
        {
            return this.ActivoFijoDAO.GetMaquinas(idCuenta, estatusMaquina, tipoCaptura);
        }

        public Dictionary<string, object> RegistrarDepMaquina(List<ActivoFijoRegInfoDepDTO> depMaquina)
        {
            return this.ActivoFijoDAO.RegistrarDepMaquina(depMaquina);
        }

        public Dictionary<string, object> ObtenerDepMaquina(int idDepMaq)
        {
            return this.ActivoFijoDAO.ObtenerDepMaquina(idDepMaq);
        }

        public Dictionary<string, object> AgregarPoliza(ActivoFijoAgregarPolizaDTO infoPoliza)
        {
            return this.ActivoFijoDAO.AgregarPoliza(infoPoliza);
        }

        public Dictionary<string, object> AgregarPolizas(int idMaquina)
        {
            return this.ActivoFijoDAO.AgregarPolizas(idMaquina);
        }

        public Dictionary<string, object> ModificarDepMaquina(List<ActivoFijoRegInfoDepDTO> depMaquina)
        {
            return this.ActivoFijoDAO.ModificarDepMaquina(depMaquina);
        }

        public Dictionary<string, object> EliminarDepMaquina(int idDepMaquina)
        {
            return this.ActivoFijoDAO.EliminarDepMaquina(idDepMaquina);
        }

        public Dictionary<string, object> GetDepreciacionCuenta()
        {
            return this.ActivoFijoDAO.GetDepreciacionCuenta();
        }

        public Dictionary<string, object> ModificarDepreciacionCuenta(List<ActivoFijoDepreciacionCuentasDTO> depCuentas)
        {
            return this.ActivoFijoDAO.ModificarDepreciacionCuenta(depCuentas);
        }

        public Dictionary<string, object> ObtenerPolizasCC(string Cc)
        {
            return this.ActivoFijoDAO.ObtenerPolizasCC(Cc);
        }

        public Dictionary<string, object> GetCentrosCostos()
        {
            return this.ActivoFijoDAO.GetCentrosCostos();
        }

        public Dictionary<string, object> GetAreasCuenta()
        {
            return this.ActivoFijoDAO.GetAreasCuenta();
        }

        public Dictionary<string, object> GetTabulador(int idDepMaquina, bool EsExtraCatMaqDep)
        {
            return this.ActivoFijoDAO.GetTabulador(idDepMaquina, EsExtraCatMaqDep);
        }

        public Dictionary<string, object> GetTabuladorExcel(ActivoFijoInfoTabuladorDTO tabulador)
        {
            return this.ActivoFijoDAO.GetTabuladorExcel(tabulador);
        }

        public Dictionary<string, object> GetConsultaEnExcel(ActivoFijoDepMaquinaResumenDTO resumen, int? cuenta, string equipo, int? estado)
        {
            return this.ActivoFijoDAO.GetConsultaEnExcel(resumen, cuenta, equipo, estado);
        }

        public Dictionary<string, object> GetPeriodosDepreciacion(int IdCatMaquina)
        {
            return this.ActivoFijoDAO.GetPeriodosDepreciacion(IdCatMaquina);
        }

        public Dictionary<string, object> GetCuentas()
        {
            return this.ActivoFijoDAO.GetCuentas();
        }

        public Dictionary<string, object> GetSubCuentas(int cuenta)
        {
            return this.ActivoFijoDAO.GetSubCuentas(cuenta);
        }

        public Dictionary<string, object> GetDepMaquinas(int? maquinaActiva, int? cuenta, string noEconomico, List<int> tipoMovimiento, List<string> areasCuenta, DateTime? fechaHasta, int? cuentaOverhaul, DateTime? fecha, bool todosLosEconomicosMaquinaria)
        {
            return this.ActivoFijoDAO.GetDepMaquinas(maquinaActiva, cuenta, noEconomico, tipoMovimiento, areasCuenta, fechaHasta, cuentaOverhaul, fecha, todosLosEconomicosMaquinaria);
        }

        public Dictionary<string, object> JalarExcel()
        {
            return this.ActivoFijoDAO.JalarExcel();
        }

        public Dictionary<string, object> GetCuentasCBO()
        {
            return this.ActivoFijoDAO.GetCuentasCBO();
        }

        public Dictionary<string, object> GetCuentas(int tipoResultado)
        {
            return this.ActivoFijoDAO.GetCuentas(tipoResultado);
        }

        public Dictionary<string, object> GetTiposMovimiento()
        {
            return this.ActivoFijoDAO.GetTiposMovimiento();
        }

        public Respuesta RelacionEquipoInsumo(int maquinaID)
        {
            return this.ActivoFijoDAO.RelacionEquipoInsumo(maquinaID);
        }
        public Dictionary<string, object> CargarTablaEnvioCosto(bool enviado)
        {
            return this.ActivoFijoDAO.CargarTablaEnvioCosto(enviado);
        }

        #region
        public Respuesta RelacionarInfoExcel_NoMaquinas()
        {
            return this.ActivoFijoDAO.RelacionarInfoExcel_NoMaquinas();
        }
        #endregion

        #region ConstruplanColombia
        public Dictionary<string, object> TipoActivoColombiaCBox()
        {
            return this.ActivoFijoDAO.TipoActivoColombiaCBox();
        }

        public Dictionary<string, object> GetActivosColombia(int tipoActivo, bool esMaquina)
        {
            return this.ActivoFijoDAO.GetActivosColombia(tipoActivo, esMaquina);
        }

        public Dictionary<string, object> GetRelacionPoliza(int idActivo, bool esMaquina)
        {
            return this.ActivoFijoDAO.GetRelacionPoliza(idActivo, esMaquina);
        }
        #endregion

        #region ConstruplanPeru
        public Dictionary<string, object> GetAnios()
        {
            return this.ActivoFijoDAO.GetAnios();
        }

        public Dictionary<string, object> GetCCs()
        {
            return this.ActivoFijoDAO.GetCCs();
        }

        public Dictionary<string, object> GetCuentasPeru()
        {
            return this.ActivoFijoDAO.GetCuentasPeru();
        }

        public Dictionary<string, object> GetActivos(int? anio, string cc, int? cuenta)
        {
            return this.ActivoFijoDAO.GetActivos(anio, cc, cuenta);
        }

        public Dictionary<string, object> GetEconomicosPeru()
        {
            return this.ActivoFijoDAO.GetEconomicosPeru();
        }

        public Dictionary<string, object> GuardarRelacionActivo(tblC_AF_RelacionPolizaPeru obj)
        {
            return this.ActivoFijoDAO.GuardarRelacionActivo(obj);
        }

        public Dictionary<string, object> EliminarRelacionActivo(int id)
        {
            return this.ActivoFijoDAO.EliminarRelacionActivo(id);
        }
        #endregion
    }
}