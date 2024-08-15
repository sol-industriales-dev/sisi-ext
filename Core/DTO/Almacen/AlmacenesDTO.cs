using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Almacen
{
    public class AlmacenesDTO
    {
        public int idRelacion { get; set; }
        public int almacen { get; set; }
        public string descripcion { get; set; }
        public int TipoAlmacen { get; set; }
    }
}
