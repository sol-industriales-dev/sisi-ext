using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class AutorizanteDTO
    {
        public int? id { get; set; }
        public string nombre { get; set; }
        public string puesto { get; set; }
        public string correo { get; set; }
        public int tblp_usuario_id { get; set; }
    }
}
