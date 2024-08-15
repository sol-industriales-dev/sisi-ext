using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using Core.Entity.Administrativo.RecursosHumanos.Tabuladores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_Requisicion
    {
        [Key]
        public int idSigoplan { get; set; }
        public int id { get; set; }
        public string cc { get; set; }
        public int puesto { get; set; }
        public int cantidad_solicitada { get; set; }
        public int solicitante { get; set; }
        public DateTime? fecha_solicitante { get; set; }
        public int autoriza { get; set; }
        public DateTime? fecha_autoriza { get; set; }
        public int? visto_bueno { get; set; }
        public DateTime? fecha_visto_bueno { get; set; }
        public string estatus { get; set; }
        public decimal? sueldo_base { get; set; }
        public int jefe_inmediato { get; set; }
        public int? razon_solicitud { get; set; }
        public int? sustituye { get; set; }
        public DateTime? fecha_baja { get; set; }
        public DateTime? fecha_contratacion { get; set; }
        public string justificacion { get; set; }
        public int tipo_contrato { get; set; }
        public int? edad { get; set; }
        public string sexo { get; set; }
        public int? estado_civil { get; set; }
        public string presentacion { get; set; }
        public int? escolaridad { get; set; }
        public string especialidad { get; set; }
        public int? experiencia { get; set; }
        public string horario { get; set; }
        public string habilidades { get; set; }
        public string actividades { get; set; }
        public string conocimientos { get; set; }
        public int altas { get; set; }
        public int bajas { get; set; }
        public int id_plantilla { get; set; }
        public DateTime fecha_vigencia { get; set; }
        public string comentarioRechazo { get; set; }
        public int? idTabuladorDet { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int? usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("puesto")]
        public virtual tblRH_EK_Puestos virtualPuesto { get; set; }

        [ForeignKey("tipo_contrato")]
        public virtual tblRH_EK_Requisicion_Tipo_Contrato virtualTipoContrato { get; set; }

        //[ForeignKey("jefe_inmediato")]
        //public virtual tblRH_EK_Empleados virtualEmpleadoJefeInmediato { get; set; }

        //[ForeignKey("autoriza")]
        //public virtual tblRH_EK_Empleados virtualEmpleadoAutoriza { get; set; }

        //[ForeignKey("solicitante")]
        //public virtual tblRH_EK_Empleados virtualEmpleadoSolicitante { get; set; }

        [ForeignKey("id_plantilla")]
        public virtual tblRH_EK_Plantilla_Personal virtualPlantilla { get; set; }

        [ForeignKey("idTabuladorDet")]
        public virtual tblRH_TAB_TabuladoresDet tabuladorDet { get; set; }
    }
}
