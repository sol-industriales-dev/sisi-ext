using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class ValuacionInventarioFisicoDTO
    {
        public int partida { get; set; }
        public int insumo { get; set; }
        public string insumoDesc { get; set; }
        public decimal cantidad { get; set; }
        public string unidad { get; set; }
        public decimal costoPromedio { get; set; }
        public decimal total { get; set; }
    }
}
