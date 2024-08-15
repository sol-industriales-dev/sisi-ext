using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class ExamenesAsistenteDTO
    {
        public HttpPostedFileBase examenDiagnostico { get; set; }
        public HttpPostedFileBase examenFinal { get; set; }
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public bool aprobado { get; set; }
        public decimal calificacion { get; set; }

        public ExamenesAsistenteDTO(HttpPostedFileBase examenDiagnostico, HttpPostedFileBase examenFinal, int id, int claveEmpleado, bool aprobado, decimal calificacion)
        {
            this.examenDiagnostico = examenDiagnostico;
            this.examenFinal = examenFinal;
            this.id = id;
            this.claveEmpleado = claveEmpleado;
            this.aprobado = aprobado;
            this.calificacion = calificacion;
        }
    }
}
