using Core.Enum.Administracion.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class UsuarioAutorizanteDTO
    {
        public int usuarioID { get; set; }
        public string nombre { get; set; }
        public EstatusAutorizacionCapacitacion estatus { get; set; }
        public int orden { get; set; }
        public string firma { get; set; }
        public bool puedeAutorizar { get; set; }
        public string puestoDesc { get; set; }
    }
}
