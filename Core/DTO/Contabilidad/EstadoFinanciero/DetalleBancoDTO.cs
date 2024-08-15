using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.EstadoFinanciero
{
    public class DetalleBancoDTO
    {
        public string concepto { get; set; }
        public decimal mesMonto { get; set; }
        public decimal mesMontoResultado { get; set; }
        public decimal mesPorcentaje { get; set; }
        public decimal mesPorcentajeResultado { get; set; }
        public decimal acumuladoMonto { get; set; }
        public decimal acumuladoMontoResultado { get; set; }
        public decimal acumuladoPorcentaje { get; set; }
        public decimal acumuladoPorcentajeResultado { get; set; }
        public bool renglonGrupo { get; set; }
    }
}
