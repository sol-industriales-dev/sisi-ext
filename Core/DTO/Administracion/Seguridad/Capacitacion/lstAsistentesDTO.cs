using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class lstAsistentesDTO
    {
        public int id { get; set; }
        public string noEmpleado { get; set; }
        public string nombreEmpleado { get; set; }
        public string puesto { get; set; }
        public string centroCostos { get; set; }
        public string examenEvidencia { get; set; }
        public string rutaExamenSubir { get; set; }
        public int estatus { get; set; }


    }
}
