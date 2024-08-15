using Core.Enum.Administracion.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class RelacionCCAutorizanteDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int usuarioID { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string nombreCompleto { get; set; }
        public int clave_empleado { get; set; }
        public PuestoAutorizanteEnum tipoPuesto { get; set; }
        public string tipoPuestoDesc { get; set; }
        public int orden { get; set; }
        public bool estatus { get; set; }
    }
}
