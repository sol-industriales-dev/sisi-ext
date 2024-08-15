using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra.CuadroComparativo
{
    public class tblCom_CC_TipoCalificacion
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool Estatus { get; set; }

        [ForeignKey("IdtipoCalificacion")]
        public virtual List<tblCom_CC_ProveedorNoOptimo> Calificaciones { get; set; }
    }
}
