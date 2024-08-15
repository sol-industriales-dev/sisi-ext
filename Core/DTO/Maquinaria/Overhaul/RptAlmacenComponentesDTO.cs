using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class RptAlmacenComponentesDTO
    {
        public string fecha { get; set; }
        public string entrada { get; set; }
        public string fechaEntradaFactura { get; set; }
        public int dias { get; set; }
        public string subconjunto { get; set; }
        public string noComponente { get; set; }
        public string modeloId { get; set; }
        public string locacion { get; set; }
        public int horasCicloActual { get; set; }
    }
}
