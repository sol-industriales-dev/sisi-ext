using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class BonoPuestoDTO
    {
        public int id { get; set; }
        public int puestoID { get; set; }
        public string puesto { get; set; }
        public decimal monto { get; set; }
        public int periodicidad { get; set; }
        public string cc { get; set; }
        public string ccNombre { get; set; }
        public int tipoNominaCve { get; set; }
        public string tipoNomina { get; set; }
    }
}
