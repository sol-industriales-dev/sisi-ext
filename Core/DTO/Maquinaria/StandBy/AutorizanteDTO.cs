using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.StandBy
{
    public class AutorizanteDTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string correo { get; set; }
        public string ac { get; set; }
        public int perfilAutorizaID { get; set; }
    }
}
