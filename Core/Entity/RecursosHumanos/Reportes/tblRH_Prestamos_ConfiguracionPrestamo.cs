using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reportes
{
    public class tblRH_Prestamos_ConfiguracionPrestamo
    {

        public int id { get; set; }
        public string descripcionFinPeriodo { get; set; }  
        public DateTime fechaInicioPeriodo { get; set; }
        public DateTime fechaFinPeriodo { get; set; }        
        public DateTime fecha_creacion { get; set; }
        public DateTime? fecha_modificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }      
        public bool registroActivo { get; set; }
        public string estatus { get; set; }  
    }
}
