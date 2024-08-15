using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class rptPrecisionOverhaulDTO
    {
        public string economico { get; set; }
        public int tipo { get; set; }
        public DateTime fechaProgramada { get; set; }
        public int diasDG { get; set; }
        public DateTime fechaEjecutado { get; set; }
        public int diasReales { get; set; }
    }
}