using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte
{
    public class RepCargoNominaCCArreDTO
    {
        public int economicoID { get; set; }
        public string noEconomico { get; set; }
        public string descripcion { get; set; }
        public int ccID { get; set; }
        public string cc { get; set; }
        public decimal hhPeriodo { get; set; }
        public decimal porcentajeCargo { get; set; }
        public decimal cargoMaquina { get; set; }
    }
}
