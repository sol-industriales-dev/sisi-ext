using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.DocumentosXPagar.PQ
{
    public class tblAF_DxP_PQ_TipoMovimiento
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("tipoMovimientoId")]
        public List<tblAF_DxP_PQ> pqs { get; set; }
    }
}
