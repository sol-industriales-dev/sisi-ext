using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina
{
    public class LayoutIncidenciasDTO
    {
        public int id { get; set; }
        public int clave_empleado { get; set; }
        public string nombre_empleado { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int total_Dias { get; set; }
        public decimal horas_extras { get; set; }
        public decimal bonoTotal { get; set; }
        public int id_incidencia { get; set; }
    }
}
