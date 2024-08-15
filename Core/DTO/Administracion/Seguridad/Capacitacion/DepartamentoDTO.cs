using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class DepartamentoDTO
    {
        public string id { get; set; }
        public string departamento { get; set; }
        public string cc { get; set; }
        public int restantes { get; set; }
        public int capacitados { get; set; }
        public string descripcion { get; set; }
        public string centroCosto { get; set; }
        public int empresa { get; set; }
    }
}
