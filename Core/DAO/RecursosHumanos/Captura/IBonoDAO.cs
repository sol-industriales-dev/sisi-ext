using Core.Entity.RecursosHumanos.Catalogo;
using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.RecursosHumanos;
using Core.DTO.Principal.Generales;
using Core.DTO.Contabilidad.Propuesta.Nomina;
using Core.DTO;
using System.Web;

namespace Core.DAO.RecursosHumanos.Captura
{
    public interface IBonoDAO
    {
        #region Guardar
        bool CrearPlantilla(tblRH_BN_Plantilla plan, bool isGestion);
        bool ActualizarPlantilla(tblRH_BN_Plantilla plan);
        bool CrearPlantilla_Cuadrado(tblRH_BN_Plantilla_Cuadrado plan, bool isGestion);
        bool ActualizarPlantilla_Cuadrado(tblRH_BN_Plantilla_Cuadrado plan);
        bool ActualizarEvaluacion(tblRH_BN_Evaluacion plan);
        bool EnviarCorreoEvaluacion(tblRH_BN_Evaluacion data, bool actualizacion);
        #endregion
        #region GestionPLantillas
        List<tblRH_BN_Plantilla> getLstGestionBono(BusqBono busq);
        List<tblRH_BN_Evaluacion> getLstGestionEvaluacion(BusqBono busq);
        string getCCNotificacion(int? autID);

        Tuple<List<ComparacionPlantillaBonoDTO>, List<ComparacionPlantillaBonoDTO>> ObtenerComparativaVersionesPlantilla(tblRH_BN_Plantilla plantilla);
        #endregion
        #region CapturaPlantilla
        tblRH_BN_Plantilla getPlantilla(int id);
        tblRH_BN_Evaluacion getEvaluacionByID(int id);
        tblRH_BN_Plantilla getPlantilla(string cc, bool isGestion);

        List<BonoPuestoDTO> ActualizarPuestos(List<tblRH_CatPuestos> puestos, string cc, bool isGestion);

        List<tblRH_BN_Plantilla_Aut> getLstAuth(int plantillaID);
        tblRH_BN_Plantilla_Cuadrado getPlantilla_Cuadrado(int id);
        tblRH_BN_Plantilla_Cuadrado getPlantilla_Cuadrado(string cc, bool isGestion);
        List<BonoPuestoDTO> ActualizarPuestos_Cuadrado(List<tblRH_CatPuestos> puestos, string cc, bool isGestion);
        #endregion
        #region Evaluacion
        List<EmpleadoPuestoDTO> getLstUsuariosFormPuestos(BusqBonoEvaluacion busq);
        tblRH_BN_Plantilla getPlantillaAutorizada(BusqBonoEvaluacion busq);
        tblRH_BN_Evaluacion getEvaluacion(int idPlantilla);
        #endregion
        #region Incidencias
        List<PeriodosNominaDTO> getPeriodoActual();
        List<PeriodosNominaDTO> getPeriodoActual(int tipoNomina);
        List<PeriodosNominaDTO> getPeriodo(int anio, int periodo);
        List<PeriodosNominaDTO> getPeriodos(int anio, int tipoNomina);
        List<PeriodosNominaDTO> getPeriodosRestantes(int anio, int tipoNomina);
        IncidenciasPaqueteDTO getLstIncidenciasEnk(BusqIncidenciaDTO busq);
        IncidenciasPaqueteDTO getIncidenciaAuth(int incidenciaID, int anio, int periodo, int tipo_nomina, string cc);
        Dictionary<string, object> authIncidenciaSIGOPLAN_ENKONTROL(tblRH_BN_Incidencia obj);
        int desAuthIncidenciaSIGOPLAN_ENKONTROL(tblRH_BN_Incidencia obj);
        int RevisarFechaCierre(List<tblRH_BN_Incidencia> obj);
        Tuple<bool, string> GuardarIncidencia(IncidenciasPaqueteDTO paquete, List<HttpPostedFileBase> archivos, List<int> lstClaveEmpleados);
        bool GuardarIncidenciaSincronizar(IncidenciasPaqueteDTO paquete);
        #endregion
        #region combobox
        List<ComboDTO> getTblP_CC();
        List<ComboDTO> getTblP_CCconPlantilla();
        List<ComboDTO> getcboCcMonto();
        List<ComboDTO> getcboCcDepto();
        List<ComboDTO> getCboAutorizante(string cc);
        List<ComboDTO> getCboIncidecnciaConcepto();
        #endregion
        #region listaNegra
        Respuesta GuardarEnListaNegra(tblRH_BN_ListaNegra empleado);
        Respuesta EmpleadosListaNegra();
        Respuesta EliminarDeListaNegra(int claveEmpleado);
        #endregion
        #region listaBlanca
        Respuesta GuardarEnListaBlanca(tblRH_BN_ListaBlanca empleado);
        Respuesta EmpleadosListaBlanca();
        Respuesta EliminarDeListaBlanca(int claveEmpleado);
        #endregion

        empleadosEvaluacionDTO getEmpleadosEvaluar(string cc, int periodo, int tipoNomina, DateTime fechaPeriodo);
        List<tblRH_BN_Evaluacion_Det> getEmpleadosUnicos(string cc,int tipoNomina);
        int guardarEvaluacion(tblRH_BN_Evaluacion obj, List<tblRH_BN_Evaluacion_Aut> aut);
        bool guardarEvaluacionDet(List<tblRH_BN_Evaluacion_Det> det);
        int actualizarEvaluacion(tblRH_BN_Evaluacion obj, List<tblRH_BN_Evaluacion_Det> det);
        List<incidenciasPendientesDTO> getIncidenciasPendiente();
        #region Peru
        IncidenciasPaqueteDTO getLstIncidenciasPeru(BusqIncidenciaDTO busq);
        List<incidenciasPendientesDTO> getIncidenciasPendientePeru();
        IncidenciasPaqueteDTO getIncidenciaAuthPeru(int incidenciaID, int anio = 0, int periodo = 0, int tipo_nomina = 0, string cc = "");
        #endregion
    }
}
