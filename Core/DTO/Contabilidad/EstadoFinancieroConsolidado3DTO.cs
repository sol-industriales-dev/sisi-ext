using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad
{
    public class EstadoFinancieroConsolidado3DTO
    {


        public string concepto { get; set; }
        public decimal montoEmp1 { get; set; }
        public decimal porcentajeEmp1 { get; set; }
        public decimal montoEmp2 { get; set; }
        public decimal porcentajeEmp2 { get; set; }
        public decimal montoEmp3 { get; set; }
        public decimal porcentajeEmp3 { get; set; }
        public decimal montoConsolidado { get; set; }
        public decimal porcentajeConsolidado { get; set; }
        public decimal montoMes { get; set; }
        public decimal porcentajeMes { get; set; }
        public Boolean esTotalizador { get; set; }

    }
}
