using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.Capacitacion;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class EmpleadoExpirarDTO
    {
        public int claveEmpleado { get; set; }
        public string nombre { get; set; }
        public int puestoID { get; set; }
        public string puesto { get; set; }
        public int cursoID { get; set; }
        public string curso { get; set; }
        public DateTime fechaExpiracion { get; set; }
        public string fechaExpiracionStr { get; set; }
        public string ccID { get; set; }
        public string cc { get; set; }
        public bool expirado { get; set; }
        public ClasificacionCursoEnum clasificacion { get; set; }
    }
}
