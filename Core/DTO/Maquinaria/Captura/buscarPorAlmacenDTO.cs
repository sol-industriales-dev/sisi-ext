using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class buscarPorAlmacenDTO
    {
        public int almacen { get; set; }
        public string descripcion { get; set; }
        public int existencia { get; set; }
        public int insumo { get; set; } 

    }
}
