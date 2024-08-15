using Core.DTO.Maquinaria.Barrenacion;
using Core.DTO.Maquinaria.Barrenacion.Reporte;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Barrenacion;
using Core.Entity.Maquinaria.Captura;
using Core.Enum.Maquinaria.Barrenacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Barrenacion
{
    public interface IBarrenacionDAO
    {
        #region Combos 
        #endregion
        /// <summary>
        /// Obtiene un listado con las area cuentas existentes, poniendo como valor areaCuenta en cada item.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> ObtenerAC();
   
        #region Mano de Obra
        /// <summary>
        /// Obtiene todas las barrenadoras en base a su centro de costos y el estatus de sus operadores asignados.
        /// </summary>
        /// <param name="areaCuenta">Area cuenta para tomar como filtro.</param>
        /// <param name="estatusOperadores">Estatus de los operadores. 2[Todos] - 1 [Operadores asignados] - 0 [Sin asignar]</param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerBarrenadorasOperadores(string areaCuenta, int estatusOperadores);

        /// <summary>
        /// Obtiene a los operadores de una barrenadora en un turno específico.
        /// </summary>
        /// <param name="barrenadoraID">Identificador de la barrenadora.</param>
        /// <param name="turno"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerOperadoresBarrenadora(int barrenadoraID, int turno);

        /// <summary>
        /// Autocomplete de empleados de EnKontrol.
        /// </summary>
        /// <param name="term">Referencia por el cuál se buscará al empleado.</param>
        /// <param name="porDesc">Indica si el empleado se buscará por su nombre o por su clave.</param>
        /// <returns></returns>
        dynamic ObtenerEmpleadosEnKontrol(string term, bool porDesc);

        /// <summary>
        /// Guarda o actualiza los operadores asignados a una barrenadora.
        /// </summary>
        /// <param name="listaOperadores">Lista de operadores asignados.</param>
        /// <returns></returns>
        Dictionary<string, object> GuardarOperadoresBarrenadora(List<tblB_ManoObra> listaOperadores);
        #endregion

        #region Piezas Barrenadora


        /// <summary>
        /// Obtiene las barrenadoras en base a un centro de costos y el estatus de sus piezas asignadas.
        /// </summary>
        /// <param name="areaCuenta">Area cuenta para tomar como filtro.</param>
        /// <param name="estatusPiezas">Esatus de las piezas de la barrenadora. 2 [Todas] -  1 [Piezas completas] -  0 [Piezas faltantes]</param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerBarrenadorasPiezas(string areaCuenta, int estatusPiezas);


        Dictionary<string, object> ObtenerInsumosPorPiezaPrecio(string areaCuenta);

        /// <summary>
        /// Obtiene las piezas asignadas de una barrenadora.
        /// </summary>
        /// <param name="barrenadoraID">Identificador de la barrenadora.</param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerPiezasBarrenadora(int barrenadoraID, string areaCuenta);

        /// <summary>
        /// Obtiene la información de un insumo por número de insumo o descripción.
        /// </summary>
        /// <param name="term">Número o descripción del insumo por buscar.</param>
        /// <param name="porDesc">Indica si la búsqueda es por descripción o por número de insumo.</param>
        /// <returns></returns>
        dynamic getInsumo(string term, bool porDesc);
        dynamic ObtenerSerieMartilloReparadoNoAsignado(string term);
        /// <summary>
        /// Guarda o actualiza las piezas relacionadas a una barrenadora.
        /// </summary>
        /// <param name="listaPiezas">Lista de piezas pertenecientes a la barrenadora.</param>
        /// <returns></returns>
        Dictionary<string, object> GuardarPiezasBarrenadora(List<tblB_PiezaBarrenadora> listaPiezas, string desechoMartillo, string desechoBarra, bool pzasCompletas);

        /// <summary>
        /// Obtiene una lista de equipos en base a una porción del número económico proporcionado.
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        object ObtenerBarrenadorasAutocomplete(string term);

        /// <summary>
        /// Da de alta un equipo como barrenadora para poder inicializar el tracking de sus componentes, etc.
        /// </summary>
        /// <param name="maquinaID">Identificador del equipo que se dará de alta.</param>
        /// <returns></returns>
        Dictionary<string, object> GuardarNuevaBarrenadora(int maquinaID);
        
      
        #endregion

        #region Captura Diaria
        /// <summary>
        /// Obtiene a las barrenadoras disponibles para su captura diaria.
        /// </summary>
        /// <param name="areaCuenta">Area cuenta a utilizar como filtro.</param>
        /// <param name="turno">Turno a utilizar como filtro.</param>
        /// <param name="fecha">Fecha que se capturará.</param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerBarrenadorasCaptura(string areaCuenta, int turno, DateTime fecha);

        /// <summary>
        /// Guarda las horas diaras trabajadas de una barrenadora, así como otros datos.
        /// </summary>
        /// <param name="listaCaptura">Lista con la información a guardar.</param>
        /// <param name="fecha">Fecha de la que se quiere hacer la captura.</param>
        /// <returns></returns>
        Dictionary<string, object> GuardarCapturaDiaria(List<tblB_CapturaDiaria> listaCaptura, DateTime fecha);
        #endregion

        #region Reparación
        Dictionary<string, object> ObtenerPiezasPorReparar();
        bool ActualizarPiezaEstadoReparacion(TipoMovimientoPiezaEnum tm, int id, string comentario);
        #endregion

        #region Catálogo de Piezas
        Dictionary<string, object> ObtenerInsumosPorPieza(string areaCuenta);

        Dictionary<string, object> AgregarInsumoPieza(tblB_CatalogoPieza nuevoInsumoPieza);

        Dictionary<string, object> EliminarInsumoPieza(int id);
        #endregion

        #region Reporte de captura diaria
        /// <summary>
        /// Obtiene información sobre las capturas hechas en un rango de fechas específico.
        /// </summary>
        /// <param name="areaCuenta"></param>
        /// <param name="turno"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        Dictionary<string, object> CargarCapturasDiarias(List<string> areaCuenta, List<int> barrenadoraID, List<int> turnos, DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Obtiene la información de una captura diaria
        /// </summary>
        /// <param name="capturaID">Identificador de la captura diaria.</param>
        /// <returns></returns>
        ReporteCapturaDTO ObtenerCapturaDiariaPorId(int capturaID);

        /// <summary>
        /// Obtiene un listado de capturas diarias de barrenación con información relevante.
        /// </summary>
        /// <param name="areaCuenta"></param>
        /// <param name="barrenadoraID"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        List<ReporteGeneralCapturaDTO> ObtenerReporteGeneralCapturas(string areaCuenta, int barrenadoraID, DateTime fechaInicio, DateTime fechaFin);
        #endregion

        #region Reporte de rendimiendo por pieza
        /// <summary>
        /// Obtiene información relevante al rendimiento de un conjunto de piezas de una barrenadora.
        /// </summary>
        /// <param name="areaCuenta"></param>
        /// <param name="tipoPieza"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        Dictionary<string, object> CargarRendimientoPiezas(string areaCuenta, List<int> tipoPieza, DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Obtiene información relevante al rendimiento de una pieza de una barrenadora.
        /// </summary>
        /// <param name="piezaID"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        RendimientoPiezaDTO CargarRendimientoPieza(int piezaID, DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Obtiene un listado de todas las barrenadoras activas filtrado por CC.
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerBarrenadorasPorCC(string cc);

        //Actualizacion de piezas de barrenacion()
        Dictionary<string, object> SaveOrUpdatePieza(List<tblB_PiezaBarrenadora> obj);
        Dictionary<string, object> GetPiezaID(int piezaID);
        #endregion

        #region Barrenacion Costo
        Dictionary<string, object> GuardarBarrenacionCosto(tblB_BarrenacionCosto registroInformacion, List<tblB_BarrenacionCostoOtroDetalle> lstPiezaDetalle, List<tblB_BarrenacionCostoPiezaDetalle> lstPiezaOtroDetalle);
        Dictionary<string, object> GetBarrenacionCosto();
        #endregion

        #region Catalógo banco
        Dictionary<string, object> AgregarBanco(tblB_CatalogoBanco nuevoBanco);

        Dictionary<string, object> ObtenerBancos(string areaCuenta);
        Dictionary<string, object> GetComboBancos(string areaCuenta);

        #endregion

        Dictionary<string, object> getInfoInsumo(int barrenadoraID, int insumo, string areaCuenta);
        Dictionary<string, object> getPiezaNueva(int insumo, string areaCuenta);
        Dictionary<string, object> setInfoCapturaDiaria(List<tblB_CapturaDiaria> listaCapturaDiaria, List<tblB_PiezaBarrenadora> listaPiezas);
       Dictionary<string, object> setReporteEjecutivo(DateTime fechaInicio, DateTime fechaFinal, List<string> areaCuenta, List<int> barrenadorasLista);
        Dictionary<string, object> ObtenerCapturaDiaria(int barrenadoraID, DateTime fechaActual,int turno);
        Dictionary<string, object> guardarAgua(string areaCuenta, int turno, string fechaCaptura, decimal litros, int id);
        Dictionary<string, object> guardarOtrosPrecios(string areaCuenta, int turno, string fechaCaptura, decimal gasto, int id);
        Dictionary<string, object> EliminarCaptura(int capturaID);

        Dictionary<string, object> CargarRptGeneralCapturas(List<string> areaCuenta, List<int> turno, DateTime fechaInicio, DateTime fechaFin);

        Dictionary<string, object> ObtenerOperadores(string areaCuenta);
        Dictionary<string, object> CargarRptOperadores(List<string> areaCuenta, List<int> claveEmpleados, DateTime fechaInicio, DateTime fechaFin);
        Dictionary<string, object> CargarRptEquiposstanby(List<string> areaCuenta, DateTime fechaInicio, DateTime fechaFin);
        Dictionary<string, object> guardarPagoMensual(string areaCuenta, DateTime fechaCaptura, decimal cantidad, int id);
        Dictionary<string, object> SaveOrUpdatePiezasBarrenadora(List<tblB_PiezaBarrenadora> listaPzas,bool completas);

        Dictionary<string, object> SaveOrUpdateCapturaDiaria(List<tblB_CapturaDiaria> listaCapturaDiaria, List<tblB_PiezaBarrenadora> listaPiezas);

        string getCCDescByAC(string AC);
    }
}
