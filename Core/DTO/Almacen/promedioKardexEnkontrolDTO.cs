using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Almacen
{
    public class promedioKardexEnkontrolDTO
    {
        public decimal entradas { get; set; }
        public decimal salidas { get; set; }
        public decimal montoEntradas { get; set; }
        public decimal montoSalidas { get; set; }
        public decimal existencias { get; set; }
        public decimal montoResultado { get; set; }
        public decimal costoPromedio { get; set; }
        public int almacen { get; set; }
        public int insumo { get; set; }
    }
}
