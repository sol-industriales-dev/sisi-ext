using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_ComparativosAdquisicionyRentaCaracteristicasDet
    {
        public int id { get; set; }
        public int idRow { get; set; }
        public int idComparativoDetalle { get; set; }
        public string Descripcion { get; set; }
    }
}
