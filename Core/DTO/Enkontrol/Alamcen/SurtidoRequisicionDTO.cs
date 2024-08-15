using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class SurtidoRequisicionDTO
    {
        public string centroCosto { get; set; }
        public string numero { get; set; }
        public string insumo { get; set; }
        public string cantidad { get; set; }
        public string almacen { get; set; }
        public string area_alm { get; set; }
        public string lado_alm { get; set; }
        public string estante_alm { get; set; }
        public string nivel_alm { get; set; }
    }
}
