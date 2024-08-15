using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.EstadoFinanciero
{
    public class EFIngresosDetDTO
    {
        public int divisionID { get; set; }
        public string divisionDescr { get; set; }
        public decimal mensualActual { get; set; }
        public decimal porcentajeMensualActual { get; set; }
        public decimal acumuladoActual { get; set; }
        public decimal porcentajeAcumuladoActual { get; set; }
        public decimal mensualAnterior { get; set; }
        public decimal porcentajeMensualAnterior { get; set; }
        public decimal acumuladoAnterior { get; set; }
        public decimal porcentajeAcumuladoAnterior { get; set; }
        public bool esActual { get; set; }
    }
}
