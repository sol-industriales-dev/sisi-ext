using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.MAZDA
{
    public class CuadrillaDTO
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public string personal { get; set; }
        public List<UsuarioMAZDADTO> personalLista { get; set; }

        public List<AreaDTO> areas { get; set; }
    }
}
