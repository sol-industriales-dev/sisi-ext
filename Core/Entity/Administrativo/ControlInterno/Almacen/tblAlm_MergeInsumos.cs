using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.ControlInterno.Almacen
{
    public class tblAlm_MergeInsumos
    {
        public int id { get; set; }
        public int insumoC { get; set; }
        public int insumoA { get; set; }
        public bool estatus { get; set; }
    }
}
