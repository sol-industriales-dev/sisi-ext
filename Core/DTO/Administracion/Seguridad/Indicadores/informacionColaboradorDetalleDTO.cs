using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.Indicadores;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class informacionColaboradorDetalleDTO
    {
        public int id { get; set; }
        public string cveEmpleado { get; set; }
        public int lostDayEmpleado { get; set; }
        public string nombre { get; set; }
        public bool estatus { get; set; }
        public ClasificacionHHTEnum clasificacion { get; set; }
        public string clasificacionDesc { get; set; }
    }
}
