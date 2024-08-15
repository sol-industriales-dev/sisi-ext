using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class AutorizadoresDTO
    {

        public int idUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public bool firma { get; set; }
        public string firmaCadena { get; set; }
    }
}
