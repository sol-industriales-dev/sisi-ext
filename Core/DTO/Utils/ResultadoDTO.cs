using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Utils
{
    public class ResultadoDTO
    {
        public string mensaje { get; set; }
        public string mensajeError { get; set; }
        public dynamic resultado { get; set; }
    }
}
