using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT
{
    public class rptMaquinariaHorasHombre
    {
        public string economico { get; set; }
        public string centroCostos { get; set; }
        public string nombreCC { get; set; }
        public string Puesto { get; set; }
        public int puestoID { get; set; }
        public int cantidadOT { get; set; }
        public decimal horasXPuesto { get; set; }
        public decimal costoXhora { get; set; }
        public decimal totalCostoXhora { get; set; }
        public string nombrePersonal { get; set; }
    }
}
