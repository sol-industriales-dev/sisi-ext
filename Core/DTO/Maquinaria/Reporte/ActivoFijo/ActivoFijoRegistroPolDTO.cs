using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoRegistroPolDTO
    {
        public int Año { get; set; }
        public int Mes { get; set; }
        public int Poliza { get; set; }
        public string TP { get; set; }
        public int TM { get; set; }
        public DateTime FechaFactura { get; set; }
        public int Factura { get; set; }
        public int Cuenta { get; set; }
    }
}