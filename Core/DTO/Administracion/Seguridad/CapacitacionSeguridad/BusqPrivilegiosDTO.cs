using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class BusqPrivilegiosDTO
    {
        public List<string> lstCc { get; set; }
        public List<PrivilegioEnum> privilegios { get; set; }
    }
}
