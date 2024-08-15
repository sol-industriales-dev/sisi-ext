using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario.Movimiento_Interno
{
    public class tblM_CatRelacionMovimientoInterno
    {
        public int id { get; set; }
        public string Destino { get; set; }
        public string Origen { get; set; }
        public bool Estatus { get; set; }
    }
}
