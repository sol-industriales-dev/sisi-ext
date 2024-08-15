using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.DocumentosXPagar.PQ
{
    public class tblAF_DxP_PQ_RelacionMovimiento
    {
        public int id { get; set; }
        public int origenId { get; set; }
        public int destinoId { get; set; }

        [ForeignKey("origenId")]
        public virtual tblAF_DxP_PQ pqOrigen { get; set; }

        [ForeignKey("destinoId")]
        public virtual tblAF_DxP_PQ pqDestino { get; set; }
    }
}
