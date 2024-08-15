using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Nomina.Raya
{
    public class RayaDTO
    {
        public int id { get; set; }
        public int nominaID { get; set; }
        public int numeroEmpleado { get; set; }
        public string cc { get; set; }
        public string nombreCompleto { get; set; }
        public RayaPropiedadesValoresDTO propiedadesRaya { get; set; }
    }
}
