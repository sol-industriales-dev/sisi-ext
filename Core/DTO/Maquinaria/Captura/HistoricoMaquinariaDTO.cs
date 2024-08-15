using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class HistoricoMaquinariaDTO
    {
        public int id { get; set; }
        public string Centro_Costos { get; set; }
        public string FechaAsignacion { get; set; }
        public string FechaEnvio { get; set; }
        public string FechaEntrega { get; set; }
        public string FechaLiberacion { get; set; }
        public string totalHoras { get; set; }
        public int estASig { get; set; }
        public string noEconomico { get; set; }
    }
}
