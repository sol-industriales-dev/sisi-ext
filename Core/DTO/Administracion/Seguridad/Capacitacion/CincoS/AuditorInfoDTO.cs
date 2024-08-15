using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion.CincoS
{
    public class AuditorInfoDTO
    {
        public int id { get; set; }
        public string auditor { get; set; }
        public string puesto { get; set; }
        public string proyecto { get; set; }
        public List<string> css { get; set; }
    }
}
