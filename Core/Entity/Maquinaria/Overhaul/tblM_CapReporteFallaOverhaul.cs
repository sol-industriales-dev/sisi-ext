using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_CapReporteFallaOverhaul
    {
        public int id { get; set; }
        public int CC { get; set; }
        public string frente { get; set; }
        public DateTime fechaReporte { get; set; }
        public DateTime fechaParo { get; set; }
        public int noEconomicoID { get; set; }
        public decimal horometroReporte { get; set; }
        public DateTime fechaUltimoAnalisis { get; set; }
        public string procedencia { get; set; }
        public DateTime fechaAlta { get; set; }
        public decimal horometroAlta { get; set; }
        public string descripcionFalla { get; set; }
        public int conjuntoID { get; set; }
        public int subConjuntoID { get; set; }
        public int componenteID { get; set; }
        public string tipoReparacion { get; set; }
        public string reparacionesConjunto { get; set; }
        public string cargoCC { get; set; }
        public bool aplicaOverhaul { get; set; }
        public int usuarioCaptura { get; set; }

    }
}
