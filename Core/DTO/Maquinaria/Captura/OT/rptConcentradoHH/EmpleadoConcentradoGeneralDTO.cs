using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT.rptConcentradoHH
{
    public class EmpleadoConcentradoGeneralDTO
    {
        public string categoria { get; set; }
        public string subCategoria { get; set; }
        public decimal horasHombre { get; set; }
        public decimal costoHH { get; set; }
        public decimal costoTotal { get; set; }
        public decimal totalGrupo { get; set; }
    }
}
