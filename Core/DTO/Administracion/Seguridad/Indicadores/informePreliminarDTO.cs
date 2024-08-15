using Core.Enum.Administracion.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Administrativo.Seguridad.Indicadores;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class informePreliminarDTO
    {
        public int id { get; set; }
        public int folio { get; set; }
        public string cc { get; set; }
        public int idEmpresa { get; set; }
        public int idAgrupacion { get; set; }
        public string proyecto { get; set; }
        public int claveEmpleado { get; set; }
        public int personaInformo { get; set; }
        public string empleado { get; set; }
        public string puestoEmpleado { get; set; }
        public string departamentoEmpleado { get; set; }
        public int departamento_id { get; set; }
        public int claveSupervisor { get; set; }
        public string supervisorEmpleado { get; set; }
        public string tipoLesion { get; set; }
        public string descripcionIncidente { get; set; }
        public string accionInmediata { get; set; }
        public string fechaIncidente { get; set; }
        public string fechaInforme { get; set; }
        public string fechaIngresoEmpleado { get; set; }
        public bool terminado { get; set; }
        public int aplicaRIA { get; set; }
        public int? riesgo { get; set; }
        public int? tipoAccidente_id { get; set; }
        public int subclasificacionID { get; set; }
        public int tipoContacto_id { get; set; }
        public int parteCuerpo_id { get; set; }
        public int agenteImplicado_id { get; set; }
        public DateTime fechaIncidenteComplete { get; set; }
        public bool esExterno { get; set; }
        public string nombreExterno { get; set; }
        public int claveContratista { get; set; }
        public string nombreEmpleado { get; set; }
        public string nombrePersonaInformo { get; set; }
        public bool tienePreliminar { get; set; }
        public bool tieneRIA { get; set; }
        public EstatusIncidenteEnum estatusAvance { get; set; }
        public string estatusAvanceDesc { get; set; }
        public List<int> procedimientosViolados { get; set; }
        public bool puedeEliminar { get; set; }
        public string abreviacionTipoIncidente { get; set; }
        public string tipoIncidenteDesc { get; set; }

        public int experienciaEmpleado_id { get; set; }
        public int antiguedadEmpleado_id { get; set; }
        public int turnoEmpleado_id { get; set; }
        public int horasTrabajadasEmpleado { get; set; }
        public int diasTrabajadosEmpleado { get; set; }
        public bool capacitadoEmpleado { get; set; }
        public bool accidentesAnterioresEmpleado { get; set; }
        public string lugarAccidente { get; set; }
        public int tipoLesion_id { get; set; }
        public bool actividadRutinaria { get; set; }
        public bool trabajoPlaneado { get; set; }
        public string trabajoRealizaba { get; set; }
        public int protocoloTrabajo_id { get; set; }
        public string descripcionAccidente { get; set; }
        public List<string> MedidasControl { get; set; }
    }
}
