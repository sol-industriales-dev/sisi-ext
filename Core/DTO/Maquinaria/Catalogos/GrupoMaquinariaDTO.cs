using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Catalogos
{
    public class GrupoMaquinariaDTO
    {
       public int id { get; set; }
       public string tipoMaquina { get; set; }
       public string descripcion { get; set; }
       public string estatus { get; set; }
    }
}
