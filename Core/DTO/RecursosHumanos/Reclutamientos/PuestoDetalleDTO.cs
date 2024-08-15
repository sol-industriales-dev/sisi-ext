using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class PuestoDetalleDTO
    {
        public int orden { get; set; }
        public string cc { get; set; }
        public int idPuesto { get; set; }
        public string puesto { get; set; }
        public string tipoAditiva { get; set; }
        public string descripcionAditiva { get; set; }
        public int cantidad { get; set; }
        public DateTime fecha_solicita { get; set; }
        public string observaciones { get; set; }
    }
}
