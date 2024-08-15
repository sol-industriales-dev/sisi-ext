using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte
{
    public class InfoCombustibleDTO
    {
        public string noEconomico { get; set; }
        public string noSerie { get; set; }
        public DateTime fecha { get; set; }
        public decimal carga1 { get; set; }
        public decimal carga2 { get; set; }
        public decimal carga3 { get; set; }
        public decimal carga4 { get; set; }
        public decimal total { get; set; }
        public string descripcion { get; set; }

    }
}
