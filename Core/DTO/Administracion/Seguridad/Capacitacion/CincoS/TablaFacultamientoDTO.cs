using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion.CincoS
{
    public class TablaFacultamientoDTO
    {
        public int id { get; set; }
        public string tipoUsuario { get; set; }
        public string nombreCompleto { get; set; }
        public List<string> ccs { get; set; }
    }
}
