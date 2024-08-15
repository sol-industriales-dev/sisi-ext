using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion.CincoS
{
    public class TablaLideresDTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string puesto { get; set; }
        public string grupo { get; set; }
        public List<string> ccs { get; set; }
    }
}
