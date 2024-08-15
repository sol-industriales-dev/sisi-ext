using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class RepTblIndicadorContrarecibo
    {
        public string Obra { get; set; }
        public DateTime FechaRecepFact { get; set; }
        public string NoFactura { get; set; }
        public string ContraRecibo { get; set; }
        public DateTime FechaCR { get; set; }
        public double Desface { get; set; }
        public bool Moneda { get; set; }
        public string NoEconomico { get; set; }
        public decimal PrecioMes { get; set; }
    }
}
