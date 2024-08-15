using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Almacen
{
    public class tblAlm_RelAreaCuentaXAlmacenDet
    {
        public int id { get; set; }
        public int idRelacion { get; set; }
        public int Almacen { get; set; }
        public int Prioridad { get; set; }
        public int TipoAlmacen { get; set; }

    }
}
