using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Vacaciones
{
    public class IncidenciasVacacionesDTO
    {
        public int id { get; set; }
        public int vacacionID { get; set; }
        public string claveEmpleado { get; set; }
        public string nombreEmpleado { get; set; }
        public string cc { get; set; }
        public string ccDescripcion { get; set; }
        public string claveResponsable { get; set; }
        public string nombreResponsable { get; set; }
        public DateTime fecha { get; set; }
        public int tipoVacaciones { get; set; }
        public bool aplica { get; set; }
    }
}
