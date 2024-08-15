using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_PresupuestoHC
    {
        public int id { get; set; }
        public string obra { get; set; }
        public decimal pAutorizado { get; set; }
        public decimal pProgramado { get; set; }
        public decimal eProgramado { get; set; }
        public decimal eNoProgramado { get; set; }
        public decimal pTotal { get; set; }
        public decimal eTotal { get; set; }
        public decimal bolsa { get; set; }
    }
}
