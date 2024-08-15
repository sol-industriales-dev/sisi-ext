using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina.Raya
{
    public class RayaTotalDetalleDTO
    {
        public int idNomina { get; set; }
        public string cc { get; set; }
        public string ccDescripcion { get; set; }
        public int empleados { get; set; }
        public RayaPropiedadesValoresDTO totales { get; set; }
    }
}
