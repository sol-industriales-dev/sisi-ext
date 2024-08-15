using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.BackLogs
{
    public class CorreoBackLogDTO
    {
        public string Correos { get; set; }
        public string nombreCompleto { get; set; }
        public string puesto { get; set; }
        public string firma { get; set; }
        public DateTime dtFecha { get; set; }

    }
}
