using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad
{
    public class GrupoGiroDTO
    {
        public int id { get; set; }
        public int idPadre { get; set; }
        public string padre { get; set; }
        public string descripcion { get; set; }
        public bool esActivo { get; set; }
    }
}
