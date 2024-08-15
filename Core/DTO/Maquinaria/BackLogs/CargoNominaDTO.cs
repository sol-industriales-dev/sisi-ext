using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.BackLogs
{
    public class CargoNominaDTO
    {
        public string noEconomico { get; set; }
        public string areaCuenta { get; set; }
        public string descripcion { get; set; }
        public string cc { get; set; }
        public decimal horasPeriodo { get; set; }
        public decimal porcentajeCargo { get; set; }
        public decimal montoCargo { get; set; }
    }
}
