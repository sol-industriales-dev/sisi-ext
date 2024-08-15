using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas
{
    public class EncuestaEstatusDTO
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public string departamento { get; set; }
        public string fecha { get; set; }
        public DateTime fechaValue { get; set; }
        public string btnVer { get; set; }
        public string btnAceptar { get; set; }
        public string btnRechazar { get; set; }
        public string tipo { get; set; }
    }
}
