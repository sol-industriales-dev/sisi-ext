using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_Incidentes
    {
        public int id { get; set; }
        public int claveContratista { get; set; }
        public int claveEmpleado { get; set; }
        public int edadEmpleado { get; set; }
        public int horasTrabajadasEmpleado { get; set; }
        public int diasTrabajadosEmpleado { get; set; }
        public int riesgo { get; set; }
        public bool esExterno { get; set; }
        public bool actividadRutinaria { get; set; }
        public bool trabajoPlaneado { get; set; }
        public bool capacitadoEmpleado { get; set; }
        public bool accidentesAnterioresEmpleado { get; set; }
        public string cc { get; set; }
        public string lugarAccidente { get; set; }
        public DateTime fechaAccidente { get; set; }
        public string trabajoRealizaba { get; set; }
        public string puestoEmpleado { get; set; }
        public int? claveSupervisor{ get; set; }
        public string supervisorCargoEmpleado { get; set; }
        public string descripcionAccidente { get; set; }
        public string instruccionTrabajo { get; set; }
        public string porqueSehizo { get; set; }
        public string nombreEmpleadoExterno { get; set; }
        public string lugarJunta { get; set; }
        public DateTime? fechaJunta { get; set; }
        public string horaInicio { get; set; }
        public string horaFin { get; set; }


        public int informe_id { get; set; }
        public virtual tblS_IncidentesInformePreliminar Informe { get; set; }

        public int? tipoAccidente_id { get; set; }
        public int subclasificacionID { get; set; }
        public virtual tblS_IncidentesTipos TiposAccidente { get; set; }

        public int departamento_id { get; set; }
        public virtual tblS_IncidentesDepartamentos Departamentos { get; set; }

        public int tipoLesion_id { get; set; }
        public virtual tblS_IncidentesTipoLesion TiposLesion { get; set; }

        public int parteCuerpo_id { get; set; }
        public virtual tblS_IncidentesPartesCuerpo PartesCuerpo { get; set; }

        public int agenteImplicado_id { get; set; }
        public virtual tblS_IncidentesAgentesImplicados AgentesImplicados { get; set; }

        public int experienciaEmpleado_id { get; set; }
        public virtual tblS_IncidentesEmpleadoExperiencia ExperienciaEmpleado { get; set; }

        public int antiguedadEmpleado_id { get; set; }
        public virtual tblS_IncidentesEmpleadoAntiguedad AntiguedadEmpleado { get; set; }

        public int turnoEmpleado_id { get; set; }
        public virtual tblS_IncidentesEmpleadosTurno TurnoEmpleado { get; set; }

        public int tipoContacto_id { get; set; }
        public virtual tblS_IncidentesTipoContacto TiposContacto { get; set; }

        public int protocoloTrabajo_id { get; set; }
        public virtual tblS_IncidentesProtocolosTrabajo ProtocolosTrabajo { get; set; }

        public int tecnicaInvestigacion_id { get; set; }
        public virtual tblS_IncidentesTecnicasInvestigacion TecnicasInvestigacion { get; set; }

        public virtual List<tblS_IncidentesCausasBasicas> CausasBasicas { get; set; }
        public virtual List<tblS_IncidentesCausasInmediatas> CausasInmediatas { get; set; }
        public virtual List<tblS_IncidentesCausasRaiz> CausasRaiz { get; set; }
        public virtual List<tblS_IncidentesEventoDetonador> EventosDedonador { get; set; }
        public virtual List<tblS_IncidentesGrupoInvestigacion> GrupoInvestigacion { get; set; }
        public virtual List<tblS_IncidentesMedidasControl> MedidasControl { get; set; }
        public virtual List<tblS_IncidentesOrdenCronologico> OrdenCronologico { get; set; }
        public int idEmpresa { get; set; }
        public int? idAgrupacion { get; set; }
    }
}
