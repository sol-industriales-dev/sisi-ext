using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Mural
{
    public class MuralDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Color { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
