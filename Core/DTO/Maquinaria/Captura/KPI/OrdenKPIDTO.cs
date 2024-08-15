using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.KPI
{
    public class OrdenKPIDTO
    {
        public int id { get; set; }
        public int EconomicoID { get; set; }
        public decimal horometro { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaSalida { get; set; }
        public int MotivoParo { get; set; }
        public int TipoOT { get; set; }
        public bool EstatusOT { get; set; }
        public DateTime FechaEntrada { get; set; }
        public int TipoParo3 { get; set; }
    }
}
