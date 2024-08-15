using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class PuestosDTO
    {
        public int puesto { get; set; }
        public string descripcion { get; set; }
        public int personalID { get; set; }
        public decimal salarioBase { get; set; }
        public decimal salarioComplemento { get; set; }
        public string nombre { get; set; }
        public string ape_paterno { get; set; }
        public string ape_materno { get; set; }
        public string centroCostos { get; set; }
    }
}
