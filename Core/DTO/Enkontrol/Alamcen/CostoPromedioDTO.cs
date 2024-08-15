using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class CostoPromedioDTO
    {
        public int tipo_mov { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal importe { get; set; }
    }
}
