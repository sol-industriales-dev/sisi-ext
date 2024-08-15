using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol
{
    public class CentroCostoDTO
    {
        public string cc { get; set; }
        public string descripcion { get; set; }
        public string corto { get; set; }
        public string desc { get; set; }
        public string st_ppto { get; set; }
        public string tipo_iva { get; set; }
        public decimal? ppto_global { get; set; }
        public DateTime? fecha_registro { get; set; }
    }
}
