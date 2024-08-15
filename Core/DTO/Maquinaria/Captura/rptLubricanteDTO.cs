using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class rptLubricanteDTO
    {
        public string noEconomico { get; set; }

        public int insumo { get; set; }
        public string descripcion { get; set; }
        public int totales { get; set; }
    }
}
