using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class maqRentadaDTO
    {
        public string noEconomico { get; set; }
        public string equipo { get; set; }

        public string serie { get; set; }
        public string modelo { get; set; }
        public int proveedorId { get; set; }
        public string proveedor { get; set; }
    }
}
