using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Vacaciones
{
    public class FechasUserDTO
    {

        public string claveEmpleado { get; set; }
        public DateTime fechaAntiguedad { get; set; }
        public DateTime fechaReingreso { get; set; }
        public string descPuesto { get; set; }
    }
}
