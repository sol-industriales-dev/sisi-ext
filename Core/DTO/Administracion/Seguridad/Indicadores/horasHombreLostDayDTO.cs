using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class horasHombreLostDayDTO
    {
        public decimal lostDays { get; set; }
        public int trabadoresPromedio { get; set; }
        public decimal horasHombres { get; set; }
        public decimal horasHombresSinIncidentes { get; set; }
        public decimal HHTContratistas { get; set; }
        public decimal HHTTotales { get; set; }
        public decimal HHTSinLTI { get; set; }
        public decimal LTIFR { get; set; }
        public decimal TRIFR { get; set; }
        public decimal TIFR { get; set; }
        public decimal SeverityRate { get; set; }
    }
}
