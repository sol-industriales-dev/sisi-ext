using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class RelFasesDTO
    {
        public int id { get; set; }
        public int idFase { get; set; }
        public int idUsuario { get; set; }
        public int usuarioCC { get; set; }
        public string faseDesc { get; set; }
        public string nombreUsuario { get; set; }
    }
}
