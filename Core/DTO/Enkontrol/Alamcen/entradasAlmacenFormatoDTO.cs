using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class entradasAlmacenFormatoDTO
    {
        public int partida { get; set; }
        public string insumo { get; set; }
        public string areaCuenta { get; set; }
        public string referencia { get; set; }
        public string remision { get; set; }
        public decimal cantidad { get; set; }
        public string precio { get; set; }
        public string importe { get; set; }
    }
}
