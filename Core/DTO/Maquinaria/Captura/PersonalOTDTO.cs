using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class PersonalOTDTO
    {
        public string Nombre { get; set; }
        public string Puesto { get; set; }
        public string Horas { get; set; }
        public byte[] imagen { get; set; }
    }
}
