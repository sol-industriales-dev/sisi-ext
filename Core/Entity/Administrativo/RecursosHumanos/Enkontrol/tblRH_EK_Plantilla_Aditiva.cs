using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Plantilla_Aditiva
    {
        public int id { get; set; }
        public int id_plantilla { get; set; }
        public string cc { get; set; }
        public int puesto { get; set; }
        public string tipo { get; set; }
        public int cantidad { get; set; }
        public int solicita { get; set; }
        public DateTime fecha_solicita { get; set; }
        public int autoriza { get; set; }
        public DateTime? fecha_autoriza { get; set; }
        public int visto_bueno { get; set; }
        public string estatus { get; set; }
        public DateTime? fecha { get; set; }
        public string observaciones { get; set; }

        [ForeignKey("id_plantilla")]
        public virtual tblRH_EK_Plantilla_Personal virtualPlantilla { get; set; }

        [ForeignKey("puesto")]
        public virtual tblRH_EK_Puestos virtualPuesto { get; set; }
        public bool registroSIGOPLAN { get; set; }

        //[ForeignKey("solicita")]
        //public virtual tblRH_EK_Empleados virtualSolicita { get; set; }

        //[ForeignKey("visto_bueno")]
        //public virtual tblRH_EK_Empleados virtualVistoBueno { get; set; }

        //[ForeignKey("autoriza")]
        //public virtual tblRH_EK_Empleados virtualAutoriza { get; set; }
    }
}
