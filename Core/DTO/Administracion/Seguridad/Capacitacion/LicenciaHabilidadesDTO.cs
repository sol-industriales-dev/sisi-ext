using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class LicenciaHabilidadesDTO
    {
        public string nombre { get; set; }
        public string NSS { get; set; }
        public string puesto { get; set; }
        public string area { get; set; }
        public Tuple<string,string>[] cursos { get; set; }
    }
}
