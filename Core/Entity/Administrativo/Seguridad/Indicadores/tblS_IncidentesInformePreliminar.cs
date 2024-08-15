using Core.Enum.Administracion.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesInformePreliminar
    {
        public int id { get; set; }
        public int folio { get; set; }
        public int claveEmpleado { get; set; }
        public int personaInformo { get; set; }
        public string cc { get; set; }
        public DateTime fechaInforme { get; set; }
        public DateTime fechaIncidente { get; set; }
        public DateTime fechaIngresoEmpleado { get; set; }
        public string puestoEmpleado { get; set; }
        public string departamentoEmpleado { get; set; }
        public int departamento_id { get; set; }
        public int? claveSupervisor { get; set; }
        public string supervisorEmpleado { get; set; }
        public string tipoLesion { get; set; }
        public string descripcionIncidente { get; set; }
        public string accionInmediata { get; set; }
        public bool terminado { get; set; }
        public bool aplicaRIA { get; set; }
        public int? riesgo { get; set; }
        public int subclasificacionID { get; set; }
        public bool esExterno { get; set; }
        public string nombreExterno { get; set; }
        public int claveContratista { get; set; }
        public string rutaPreliminar { get; set; }
        public string rutaRIA { get; set; }
        public EstatusIncidenteEnum estatusAvance { get; set; }

        public int? tipoAccidente_id { get; set; }
        public virtual tblS_IncidentesTipos TiposAccidente { get; set; }

        public int tipoContacto_id { get; set; }
        public virtual tblS_IncidentesTipoContacto TipoContacto { get; set; }

        public int parteCuerpo_id { get; set; }
        public virtual tblS_IncidentesPartesCuerpo ParteCuerpo { get; set; }

        public int agenteImplicado_id { get; set; }
        public virtual tblS_IncidentesAgentesImplicados AgenteImplicado { get; set; }

        public virtual List<tblS_Incidentes> Incidentes { get; set; }

        public virtual List<tblS_IncidentesTipoProcedimientosViolados> procedimientosViolados { get; set; }

        public virtual tblS_IncidentesDepartamentos Departamentos { get; set; }

        #region Campos nuevos del RIA.
        public string lugarAccidente { get; set; }
        public int tipoLesion_id { get; set; }
        public virtual tblS_IncidentesTipoLesion TiposLesion { get; set; }
        public bool actividadRutinaria { get; set; }
        public bool trabajoPlaneado { get; set; }
        public string trabajoRealizaba { get; set; }
        public int protocoloTrabajo_id { get; set; }
        public virtual tblS_IncidentesProtocolosTrabajo ProtocolosTrabajo { get; set; }
        public int experienciaEmpleado_id { get; set; }
        public virtual tblS_IncidentesEmpleadoExperiencia ExperienciaEmpleado { get; set; }
        public int antiguedadEmpleado_id { get; set; }
        public virtual tblS_IncidentesEmpleadoAntiguedad AntiguedadEmpleado { get; set; }
        public int turnoEmpleado_id { get; set; }
        public virtual tblS_IncidentesEmpleadosTurno TurnoEmpleado { get; set; }
        public int horasTrabajadasEmpleado { get; set; }
        public int diasTrabajadosEmpleado { get; set; }
        public bool capacitadoEmpleado { get; set; }
        public bool accidentesAnterioresEmpleado { get; set; }
        public string descripcionAccidente { get; set; }
        #endregion

        public int idEmpresa { get; set; }
        public int? idAgrupacion { get; set; }
    }
}
