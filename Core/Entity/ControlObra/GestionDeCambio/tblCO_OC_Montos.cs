using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra.GestionDeCambio
{
    public class tblCO_OC_Montos
    {
        public int id { get; set; }
        public string no { get; set; }
        public string descripcion { get; set; }
        public string unidad { get; set; }
        public int cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal importe { get; set; }
        public int idOrdenDeCambio { get; set; }
        public int tipoSoportes { get; set; }
    }
}
