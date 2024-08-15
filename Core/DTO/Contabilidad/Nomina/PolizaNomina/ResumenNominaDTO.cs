using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina.PolizaNomina
{
    public class ResumenNominaDTO
    {
        public List<ResumenDetalleNominaDTO> detalle { get; set; }

        public ResumenNominaDTO()
        {
            detalle = new List<ResumenDetalleNominaDTO>();
        }
    }
}
