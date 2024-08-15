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

namespace Core.DAO.Maquinaria.Reporte.ActivoFijo
{
    public interface IActivoFijoDAO
    {
        Respuesta AddInsumo(tblC_AF_InsumosOverhaul insumo);
        Respuesta GetInsumos();
        Respuesta UpdateInsumos(List<tblC_AF_InsumosOverhaul> insumos);
        Respuesta AgregarInsumosAutomaticamente();

        #region Póliza de depreciación
        Respuesta FechaCaptura(int cuenta, bool esOverhaul);
        Respuesta EliminarPoliza(int polizaId);
        Respuesta RegistrarPoliza(List<PolizaGeneradaDTO> polizaGenerada);
        Respuesta GenerarPoliza(int cuenta, int año, int mes, int semana, int dia, bool esOverhaul, int? idCuentaDepOverhaul);
        Respuesta ObtenerPolizaDetalle(int año, int mes, int poliza);
        Respuesta ObtenerPolizasDepreciacion(int año, int mes, int cuenta);
        Dictionary<string, object> CuentasDepOverhaul();
        #endregion

        bool PolizasCapturadas();

        Respuesta DepreciacionNumEconomico(string noEconomico, DateTime fechaHasta);
        List<DepreciacionMaquinaConOverhaulDTO> DepreciacionNumEconomicoDTO(List<StandByEcosDTO> noEconomico, DateTime fechaInicio, DateTime fechaHasta);

        Respuesta CalcularEnviosACosto();
        Respuesta GenerarPolizaCosto(List<int> idEnvioCosto);
        Dictionary<string, object> RegistrarPolizaCosto(PolizaCapturaEnkontrolDTO infoPoliza);
        Respuesta GenerarPolizaCostoPorInsumo(int idCatMaqDepreciacion);
        Dictionary<string, object> RelacionAutomaticaPolizas();

        Dictionary<string, object> GetResumen(DateTime fechaHasta);
        Dictionary<string, object> GetDetalleCuenta(DateTime fechaHasta, int cuenta);
        Dictionary<string, object> getDetalleExcel(List<ActivoFijoDetalleCuentaDTO> datosExcel, DateTime fechaHasta, List<CedulaColombiaDTO> colombia);
        Dictionary<string, object> GetMaquinas(int idCuenta, int estatusMaquina, int tipoCaptura);
        Dictionary<string, object> RegistrarDepMaquina(List<ActivoFijoRegInfoDepDTO> depMaquina);
        Dictionary<string, object> ObtenerDepMaquina(int idDepMaq);
        Dictionary<string, object> AgregarPoliza(ActivoFijoAgregarPolizaDTO infoPoliza);
        Dictionary<string, object> AgregarPolizas(int idMaquina);
        Dictionary<string, object> ModificarDepMaquina(List<ActivoFijoRegInfoDepDTO> depMaquina);
        Dictionary<string, object> EliminarDepMaquina(int idDepMaquina);
        Dictionary<string, object> GetDepreciacionCuenta();
        Dictionary<string, object> ModificarDepreciacionCuenta(List<ActivoFijoDepreciacionCuentasDTO> depCuentas);
        Dictionary<string, object> ObtenerPolizasCC(string Cc);
        Dictionary<string, object> GetCentrosCostos();
        Dictionary<string, object> GetAreasCuenta();
        Dictionary<string, object> GetTabulador(int idDepMaquina, bool EsExtraCatMaqDep);
        Dictionary<string, object> GetTabuladorExcel(ActivoFijoInfoTabuladorDTO tabulador);
        Dictionary<string, object> GetConsultaEnExcel(ActivoFijoDepMaquinaResumenDTO resumen, int? cuenta, string equipo, int? estado);
        Dictionary<string, object> GetPeriodosDepreciacion(int IdCatMaquina);
        Dictionary<string, object> GetCuentas();
        Dictionary<string, object> GetSubCuentas(int cuenta);
        Dictionary<string, object> GetDepMaquinas(int? maquinaActiva, int? cuenta, string noEconomico, List<int> tipoMovimiento, List<string> areasCuenta, DateTime? fechHasta, int? cuentaOvervaul, DateTime? fecha, bool todosLosEconomicosMaquinaria);
        Dictionary<string, object> JalarExcel();
        Dictionary<string, object> GetCuentasCBO();
        Dictionary<string, object> GetCuentas(int tipoResultado);
        Dictionary<string, object> GetTiposMovimiento();

        //-->Relacion maquina - isnumo
        Respuesta RelacionEquipoInsumo(int maquinaID);
        Dictionary<string, object> CargarTablaEnvioCosto(bool enviado);
        //<--

        #region Carga de información desde excel
        /// <summary>
        /// Por cada registro en la tabla tblC_AF_ExcelNoMaquinas se busca una póliza de coincidencia en la tabla
        /// de movimientos de pólizas en enkontrol, al encontrar coincidencias mezcla la información y la registra en la
        /// tabla de relacion activos-pólizas (tblC_AF_PolizaAltaBaja_NoMaquina).
        /// 
        /// La información de la tabla tblC_AF_ExcelNoMaquinas se carga directamente desde la BD con un archivo excel especifico
        /// </summary>
        /// <returns></returns>
        Respuesta RelacionarInfoExcel_NoMaquinas();
        #endregion

        #region ConstruplanColombia
        Dictionary<string, object> TipoActivoColombiaCBox();
        Dictionary<string, object> GetActivosColombia(int tipoActivo, bool esMaquina);
        Dictionary<string, object> GetRelacionPoliza(int idActivo, bool esMaquina);
        #endregion

        #region ConstruplanPeru
        Dictionary<string, object> GetAnios();
        Dictionary<string, object> GetCCs();
        Dictionary<string, object> GetCuentasPeru();
        Dictionary<string, object> GetActivos(int? anio, string cc, int? cuenta);
        Dictionary<string, object> GetEconomicosPeru();
        Dictionary<string, object> GuardarRelacionActivo(tblC_AF_RelacionPolizaPeru obj);
        Dictionary<string, object> EliminarRelacionActivo(int id);
        #endregion
    }
}