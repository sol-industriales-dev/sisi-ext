using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Vacaciones
{
    public class RetardosDTO
    {
        public int id { get; set; }
        public int consecutivo { get; set; }
        public int estado { get; set; }
        public string nombreEmpleado { get; set; }
        public string claveEmpleado { get; set; }
        public string cc { get; set; }
        public string comentarioRechazada { get; set; }
        public int? tipoRetardo { get; set; } //HACER ENUM
        public int motivoJustificacion { get; set; } //TABLA
        public string justificacion { get; set; }
        public int? horario { get; set; } //HACER ENUM
        public TimeSpan? horarioLower { get; set; }
        public TimeSpan? horarioUpper { get; set; } 
        public decimal tiempoRequeridoHrs { get; set; }
        public decimal tiempoRequeridoMin { get; set; }
        public DateTime diaTomado { get; set; }
        public string rutaArchivoActa { get; set; }
        public int idJefeInmediato { get; set; }
        public string nombreJefeInmediato { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; } 

        //COMPLEMENTARIOS
        public bool esFirmar { get; set; }
        public List<VacacionesGestionDTO> lstAutorizantes { get; set; }
        public List<string> lstFiltroCC { get; set; }
        public string descPuesto { get; set; }
        public string nombreCapturo { get; set; }
        public DateTime? fechaFiltroInicio { get; set; }
        public DateTime? fechaFiltroFin { get; set; }
        public string ccEmpleado { get; set; }
        public string ccDesc { get; set; }
        public string descMotivo { get; set; }

    }
}
