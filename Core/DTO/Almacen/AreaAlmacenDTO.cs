using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Almacen
{
    public class AreaAlmacenDTO
    {
        public int id { get; set; }
        public string Asignacion { get; set; }
        public string AreaCuenta { get; set; }
        public string Descripcion { get; set; }
        public List<AlmacenesDTO> lstAlmacen { get; set; }
        public string mensaje { get; set; }
    }
}
