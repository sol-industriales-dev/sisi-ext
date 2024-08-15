using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class reporteRemocionComponenteDTO
    {
        public string obra { get; set; }
        public decimal costoCRC { get; set; }
        public int componenteID { get; set; }
    }
}
