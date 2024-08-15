using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_ComparativoAdquisicionyRentaAutorizante
    {
        public int id { get; set; }
        public int idAsignacion { get; set; }
        public int idCuadro { get; set; }
        public int idComparativoDetalle { get; set; }
        public int autorizanteID { get; set; }
        public string autorizanteNombre { get; set; }
        public string autorizantePuesto { get; set; }
        public bool autorizanteStatus { get; set; }
        public bool autorizanteFinal { get; set; }
        public DateTime ?autorizanteFecha { get; set; }
        public string firma { get; set; }
        public string tipo { get; set; }
        public int orden { get; set; }
        public string comentario { get; set; }

        
            
    }
}
