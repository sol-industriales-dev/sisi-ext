using Core.Enum.Administracion.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class PorcentajesExpiradosDTO
    {
        public int cursoID { get; set; }
        public string curso { get; set; }
        public decimal totales { get; set; }
        public decimal expirados { get; set; }
        public decimal porcentaje { get; set; }
        public ClasificacionCursoEnum clasificacion { get; set; }
    }
}
