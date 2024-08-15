using Core.DAO.RecursosHumanos.Captura;
using Core.DTO;
using Core.DTO.Contabilidad.Propuesta.Nomina;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos;
using Core.Entity.Principal.Usuarios;
using Core.Entity.RecursosHumanos.Captura;
using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.RecursosHumanos.Captura
{
    public class BonoService : IBonoDAO
    {
        #region Atributos
        private IBonoDAO m_interfazDAO;
        #endregion Atributos
        #region Propiedades
        private IBonoDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades
        #region Constructores
        public BonoService(IBonoDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores
        #region Guardar
        public bool CrearPlantilla(tblRH_BN_Plantilla plan, bool isGestion)
        {
            return interfazDAO.CrearPlantilla(plan, isGestion);
        }
        public bool ActualizarPlantilla(tblRH_BN_Plantilla plan)
        {
            return interfazDAO.ActualizarPlantilla(plan);
        }
        public bool CrearPlantilla_Cuadrado(tblRH_BN_Plantilla_Cuadrado plan, bool isGestion)
        {
            return interfazDAO.CrearPlantilla_Cuadrado(plan, isGestion);
        }
        public bool ActualizarPlantilla_Cuadrado(tblRH_BN_Plantilla_Cuadrado plan)
        {
            return interfazDAO.ActualizarPlantilla_Cuadrado(plan);
        }
        public bool ActualizarEvaluacion(tblRH_BN_Evaluacion plan)
        {
            return interfazDAO.ActualizarEvaluacion(plan);
        }
        public bool EnviarCorreoEvaluacion(tblRH_BN_Evaluacion data, bool actualizacion)
        {
            return interfazDAO.EnviarCorreoEvaluacion(data,actualizacion);
        }
        #endregion
        #region GestionPLantilla
        public List<tblRH_BN_Plantilla> getLstGestionBono(BusqBono busq)
        {
            return interfazDAO.getLstGestionBono(busq);
        }
        public List<tblRH_BN_Evaluacion> getLstGestionEvaluacion(BusqBono busq)
        {
            return interfazDAO.getLstGestionEvaluacion(busq);
        }

        public string getCCNotificacion(int? autID)
        {
            return interfazDAO.getCCNotificacion(autID);
        }

        public Tuple<List<ComparacionPlantillaBonoDTO>, List<ComparacionPlantillaBonoDTO>> ObtenerComparativaVersionesPlantilla(tblRH_BN_Plantilla plantilla)
        {
            return interfazDAO.ObtenerComparativaVersionesPlantilla(plantilla);
        }
        #endregion
        #region CapturaPlantilla
        public tblRH_BN_Plantilla getPlantilla(int id)
        {
            return interfazDAO.getPlantilla(id);
        }
        public tblRH_BN_Evaluacion getEvaluacionByID(int id)
        {
            return interfazDAO.getEvaluacionByID(id);
        }
        public tblRH_BN_Plantilla getPlantilla(string cc, bool isGestion)
        {
            return interfazDAO.getPlantilla(cc, isGestion);
        }

        public List<BonoPuestoDTO> ActualizarPuestos(List<tblRH_CatPuestos> puestos, string cc, bool isGestion)
        {
            return interfazDAO.ActualizarPuestos(puestos, cc,isGestion);
        }
        public List<tblRH_BN_Plantilla_Aut> getLstAuth(int plantillaID)
        {
            return interfazDAO.getLstAuth(plantillaID);
        }
        public tblRH_BN_Plantilla_Cuadrado getPlantilla_Cuadrado(int id)
        {
            return interfazDAO.getPlantilla_Cuadrado(id);
        }
        public tblRH_BN_Plantilla_Cuadrado getPlantilla_Cuadrado(string cc, bool isGestion)
        {
            return interfazDAO.getPlantilla_Cuadrado(cc, isGestion);
        }
        public List<BonoPuestoDTO> ActualizarPuestos_Cuadrado(List<tblRH_CatPuestos> puestos, string cc, bool isGestion)
        {
            return interfazDAO.ActualizarPuestos_Cuadrado(puestos, cc, isGestion);
        }
        #endregion
        #region Evaluacion
        public List<EmpleadoPuestoDTO> getLstUsuariosFormPuestos(BusqBonoEvaluacion busq)
        {
            return interfazDAO.getLstUsuariosFormPuestos(busq);
        }
        public tblRH_BN_Plantilla getPlantillaAutorizada(BusqBonoEvaluacion busq)
        {
            return interfazDAO.getPlantillaAutorizada(busq);
        }
        public tblRH_BN_Evaluacion getEvaluacion(int idPlantilla)
        {
            return interfazDAO.getEvaluacion(idPlantilla);
        }
        #endregion
        #region Incidencias
        public List<PeriodosNominaDTO> getPeriodoActual()
        {
            return interfazDAO.getPeriodoActual();
        }
        public List<PeriodosNominaDTO> getPeriodoActual(int tipoNomina)
        {
            return interfazDAO.getPeriodoActual(tipoNomina);
        }
        public List<PeriodosNominaDTO> getPeriodo(int anio, int periodo)
        {
            return interfazDAO.getPeriodo(anio, periodo);
        }
        public List<PeriodosNominaDTO> getPeriodos(int anio, int tipoNomina)
        {
            return interfazDAO.getPeriodos(anio,tipoNomina);
        }
        public List<PeriodosNominaDTO> getPeriodosRestantes(int anio, int tipoNomina)
        {
            return interfazDAO.getPeriodosRestantes(anio, tipoNomina);
        }
        public IncidenciasPaqueteDTO getIncidenciaAuth(int incidenciaID, int anio, int periodo, int tipo_nomina, string cc)
        {
            return interfazDAO.getIncidenciaAuth(incidenciaID, anio, periodo, tipo_nomina, cc);
        }
        public IncidenciasPaqueteDTO getLstIncidenciasEnk(BusqIncidenciaDTO busq)
        {
            return interfazDAO.getLstIncidenciasEnk(busq);
        }

        public Dictionary<string, object> authIncidenciaSIGOPLAN_ENKONTROL(tblRH_BN_Incidencia obj)
        {
            return interfazDAO.authIncidenciaSIGOPLAN_ENKONTROL(obj);
        }
        public int desAuthIncidenciaSIGOPLAN_ENKONTROL(tblRH_BN_Incidencia obj)
        {
            return interfazDAO.desAuthIncidenciaSIGOPLAN_ENKONTROL(obj);
        }
        public int RevisarFechaCierre(List<tblRH_BN_Incidencia> obj)
        {
            return interfazDAO.RevisarFechaCierre(obj);
        }
        public Tuple<bool, string> GuardarIncidencia(IncidenciasPaqueteDTO paquete, List<HttpPostedFileBase> archivos, List<int> lstClaveEmpleados)
        {
            return interfazDAO.GuardarIncidencia(paquete, archivos, lstClaveEmpleados);
        }
        public bool GuardarIncidenciaSincronizar(IncidenciasPaqueteDTO paquete)
        {
            return interfazDAO.GuardarIncidenciaSincronizar(paquete);
        }
        
        #endregion
        #region combobox
        public List<ComboDTO> getTblP_CC()
        {
            return interfazDAO.getTblP_CC();
        }
        public List<ComboDTO> getTblP_CCconPlantilla()
        {
            return interfazDAO.getTblP_CCconPlantilla();
        }
        public List<ComboDTO> getcboCcMonto()
        {
            return interfazDAO.getcboCcMonto();
        }
        public List<ComboDTO> getcboCcDepto()
        {
            return interfazDAO.getcboCcDepto();
        }
        public List<ComboDTO> getCboAutorizante(string cc)
        {
            return interfazDAO.getCboAutorizante(cc);
        }
        public List<ComboDTO> getCboIncidecnciaConcepto()
        {
            return interfazDAO.getCboIncidecnciaConcepto();
        }
        #endregion
        #region listaNegra
        public Respuesta GuardarEnListaNegra(tblRH_BN_ListaNegra empleado)
        {
            return interfazDAO.GuardarEnListaNegra(empleado);
        }

        public Respuesta EmpleadosListaNegra()
        {
            return interfazDAO.EmpleadosListaNegra();
        }

        public Respuesta EliminarDeListaNegra(int claveEmpleado)
        {
            return interfazDAO.EliminarDeListaNegra(claveEmpleado);
        }
        #endregion
        #region listaBlanca
        public Respuesta GuardarEnListaBlanca(tblRH_BN_ListaBlanca empleado)
        {
            return interfazDAO.GuardarEnListaBlanca(empleado);
        }

        public Respuesta EmpleadosListaBlanca()
        {
            return interfazDAO.EmpleadosListaBlanca();
        }

        public Respuesta EliminarDeListaBlanca(int claveEmpleado)
        {
            return interfazDAO.EliminarDeListaBlanca(claveEmpleado);
        }
        #endregion

        public empleadosEvaluacionDTO getEmpleadosEvaluar(string cc, int periodo, int tipoNomina, DateTime fechaPeriodo)
        {
            return interfazDAO.getEmpleadosEvaluar(cc,periodo,tipoNomina, fechaPeriodo);
        }
        public List<tblRH_BN_Evaluacion_Det> getEmpleadosUnicos(string cc, int tipoNomina)
        {
            return interfazDAO.getEmpleadosUnicos(cc,tipoNomina);
        }

        public int guardarEvaluacion(tblRH_BN_Evaluacion obj, List<tblRH_BN_Evaluacion_Aut> aut)
        {
            return interfazDAO.guardarEvaluacion(obj,aut);
        }
        public bool guardarEvaluacionDet(List<tblRH_BN_Evaluacion_Det> det)
        {
            return interfazDAO.guardarEvaluacionDet(det);
        }
        public int actualizarEvaluacion(tblRH_BN_Evaluacion obj, List<tblRH_BN_Evaluacion_Det> det)
        {
            return interfazDAO.actualizarEvaluacion(obj, det);
        }

        public List<incidenciasPendientesDTO> getIncidenciasPendiente()
        {
            return interfazDAO.getIncidenciasPendiente();
        }

        #region Peru
        public IncidenciasPaqueteDTO getLstIncidenciasPeru(BusqIncidenciaDTO busq)
        {
            return interfazDAO.getLstIncidenciasPeru(busq);
        }
        public List<incidenciasPendientesDTO> getIncidenciasPendientePeru()
        {
            return interfazDAO.getIncidenciasPendientePeru();
        }
        public IncidenciasPaqueteDTO getIncidenciaAuthPeru(int incidenciaID, int anio = 0, int periodo = 0, int tipo_nomina = 0, string cc = "")
        {
            return interfazDAO.getIncidenciaAuthPeru(incidenciaID, anio, periodo, tipo_nomina, cc);
        }
        #endregion
    }
}
