using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class PuestosDTO
    {
        public int clave_empleado { get; set; }
        public string puesto { get; set; }
        public int clave_depto { get; set; }
    }
}
