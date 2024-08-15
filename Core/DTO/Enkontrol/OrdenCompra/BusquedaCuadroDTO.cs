using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class BusquedaCuadroDTO
    {
        public string cc { get; set; }
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }
        public int reqInicial { get; set; }
        public int reqFinal { get; set; }
        public int tipo { get; set; }
        public bool insumosLicitados { get; set; }
        public DateTime fechaInicialInsumosLicitados { get; set; }
        public DateTime fechaFinalInsumosLicitados { get; set; }
    }
}
