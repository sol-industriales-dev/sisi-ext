using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoCorteInventarioDTO
    {
        public int economicoId { get; set; }
        public string noEconomico { get; set; }
        public string areaCuenta { get; set; }
        public string cc { get; set; }
        public string obra { get; set; }
        public DateTime fechaCorte { get; set; }
    }
}
