using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad
{
    public class EstadoFinancieroMensualDTO
    {
        public string concepto { get; set; }
        public decimal montoMes { get; set; }
        public decimal porcentaje { get; set; }
        public decimal montoMesAnterior { get; set; }
        public decimal porcentajeMesAnterior { get; set; }
        public decimal variaciones { get; set; }
        public decimal montoAcumulado { get; set; }
        public decimal porcentajeAcumulado { get; set; }
        public decimal montoAcumuladoAnterior { get; set; }
        public decimal porcentajeAcumuladoAnterior { get; set; }
        public decimal variacionesAcumulado { get; set; }
        public Boolean esTotalizador { get; set; }
    }
}
