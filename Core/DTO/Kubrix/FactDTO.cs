using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Kubrix
{
    public class FactDTO
    {
        public string insumo { get; set; }
        public string unidad { get; set; }
        public decimal cantidad { get; set; }
        public decimal monto { get; set; }
        public string descripcion { get; set; }
        public string cta { get; set; }
    }
}
