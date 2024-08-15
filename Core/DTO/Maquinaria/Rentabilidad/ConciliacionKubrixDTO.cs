using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Rentabilidad
{
    public class ConciliacionKubrixDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public string factura { get; set; }
        public int estatus { get; set; }
    }
}
