using Core.Enum.Administracion.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class BusqPrivilegiosDTO
    {
        public List<string> lstCc { get; set; }
        public List<PrivilegioEnum> privilegios { get; set; }
    }
}
