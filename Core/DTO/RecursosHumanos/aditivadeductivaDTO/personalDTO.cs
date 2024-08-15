using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.aditivadeductivaDTO
{
    public class personalDTO
    {
        public int id { get; set; }
        public string categoria { get; set; }
        public int personalNecesario { get; set; }
        public int personalExistente { get; set; }
        public int personalFaltante { get; set; }
    }
}
