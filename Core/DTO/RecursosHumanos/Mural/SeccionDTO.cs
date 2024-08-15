using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Mural
{
    public class SeccionDTO
    {
        public int Id { get; set; }
        public int PosicionX { get; set; }
        public int PosicionY { get; set; }
        public string Color { get; set; }
        public int Altura { get; set; }
        public int Ancho { get; set; }
    }
}
