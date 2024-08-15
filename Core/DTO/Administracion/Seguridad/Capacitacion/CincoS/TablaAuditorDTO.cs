using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion.CincoS
{
    public class TablaAuditorDTO
    {
        public int id { get; set; }
        public string auditor { get; set; }
        public List<string> ccs { get; set; }
    }
}
