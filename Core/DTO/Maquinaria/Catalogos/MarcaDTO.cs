using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Catalogos
{
    public class MarcaDTO
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public bool estatus { get; set; }
        public int grupoEquipoID { get; set; }
        public string grupo { get; set; }
    }
}
