using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class OpusDTO
    {
        public string descripcion { get; set; }
        public int nivel { get; set; }
        public string signo { get; set; }
        public decimal cantidad { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaTermino { get; set; }
        public string unidad { get; set; }
        public decimal costo { get; set; }
        public decimal precioUnitario { get; set; }
        public string tipoRenglon { get; set; }
        public string claveOPUS { get; set; }
    }
}
