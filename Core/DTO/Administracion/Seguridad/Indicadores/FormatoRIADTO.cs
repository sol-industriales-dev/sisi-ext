using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class FormatoRIADTO
    {
        #region Datos Generales
        public int? tipoAccidente_id { get; set; }
        public int subclasificacionID { get; set; }
        public string tipoAccidente { get; set; }
        public bool esExterno { get; set; }
        public string empresa { get; set; }
        public string cc { get; set; }
        public int claveContratista { get; set; }
        public int departamento_id { get; set; }
        public string departamento { get; set; }
        public string lugarAccidente { get; set; }
        public string fechaAccidente { get; set; }
        public string horaAccidente { get; set; }
        public string diaSemana { get; set; }
        public int tipoLesion_id { get; set; }
        public string tipoLesion { get; set; }
        public int parteCuerpo_id { get; set; }
        public bool actividadRutinaria { get; set; }
        public int agenteImplicado_id { get; set; }
        public string agenteImplicado { get; set; }
        public bool trabajoPlaneado { get; set; }
        public string trabajoRealizaba { get; set; }
        public int tipoContacto_id { get; set; }
        public string tipoContacto { get; set; }
        public int protocoloTrabajo_id { get; set; }
        public string protocoloTrabajo { get; set; }
        public string accionInmediata { get; set; }
        #endregion

        #region Persona implicada en el accidente
        public int claveEmpleado { get; set; }
        public string nombreEmpleado { get; set; }
        public int edadEmpleado { get; set; }
        public string puestoEmpleado { get; set; }
        public int experienciaEmpleado_id { get; set; }
        public string experienciaEmpleado { get; set; }
        public int antiguedadEmpleado_id { get; set; }
        public string antiguedadEmpleado { get; set; }
        public int turnoEmpleado_id { get; set; }
        public string turnoEmpleado { get; set; }
        public int horasTrabajadasEmpleado { get; set; }
        public int diasTrabajadosEmpleado { get; set; }
        public bool capacitadoEmpleado { get; set; }
        public bool accidentesAnterioresEmpleado { get; set; }
        public int claveSupervisor { get; set; }
        public string supervisorCargoEmpleado { get; set; }
        public string nombreEmpleadoExterno { get; set; }
        #endregion

        #region Descripción del accidente
        public string descripcionAccidente { get; set; }
        #endregion

        #region riesgo
        public int riesgo { get; set; }
        public string descripcionRiesgo { get; set; }
        #endregion

        #region Grupo de trabajo para la investigación
        public List<GrupoInvestigacionDTO> grupoInvestigacion { get; set; }
        #endregion

        #region Orden cronológico para la investigación
        public string instruccionTrabajo { get; set; }
        public string porqueSehizo { get; set; }
        public List<string> ordenCronologico { get; set; }
        #endregion

        #region Técnica de Investigación
        public int tecnicaInvestigacion_id { get; set; }
        public string tecnicaInvestigacion { get; set; }
        #endregion

        #region Análisis de causas
        public List<string> eventoDetonador { get; set; }
        public List<string> causaInmediata { get; set; }
        public List<string> causaBasica { get; set; }
        public List<string> causaRaiz { get; set; }
        #endregion

        #region Medidas de control
        public string lugarJunta { get; set; }
        public string fechaJunta { get; set; }
        public string horaInicio { get; set; }
        public string horaFin { get; set; }
        public List<MedidaControlDTO> medidasControl { get; set; }
        #endregion
    }
}
