using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo
{
    public class tblC_AF_AjusteSubConjuntos
    {
        public int id { get; set; }
        public int economicoID { get; set; }
        public int subConjuntoID { get; set; }
        public int cantidad { get; set; }
        public bool registroActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int usuarioCreacionId { get; set; }
        public int usuarioModificacionId { get; set; }
    }
}
