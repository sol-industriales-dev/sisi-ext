using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Principal.Usuarios
{
    public class TiempoSesionDTO
    {
        public string UpdateSession { get; set; }
        public string ServerTime { get; set; }
        public int Minutos { get; set; }
        public int Timeout { get; set; }
        public int Restante { get; set; }
    }
}
