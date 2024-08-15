using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class IndicadorFactorCapacitacionDTO
    {
        public int cursoID { get; set; }
        public string cursoDesc { get; set; }
        public bool capacitado { get; set; }
        public string cc { get; set; }
    }
}
