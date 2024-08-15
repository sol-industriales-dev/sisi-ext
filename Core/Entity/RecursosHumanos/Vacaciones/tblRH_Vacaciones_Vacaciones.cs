using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Vacaciones
{
    public class tblRH_Vacaciones_Vacaciones
    {

        public int id { get; set; }
        public int estado { get; set; }
        public string nombreEmpleado { get; set; }
        public string claveEmpleado { get; set; }
        public string cc { get; set; }
        public string comentarioRechazada { get; set; }
        public int tipoVacaciones { get; set; }
        public int consecutivo { get; set; }
        public bool? esPagadas { get; set; }
        public string justificacion { get; set; }
        public int idJefeInmediato { get; set; }
        public string nombreJefeInmediato { get; set; }
        public string rutaArchivoActa { get; set; }
        public int? numDiasDisponiblesAlDiaCaptura { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; } 

    }
}
