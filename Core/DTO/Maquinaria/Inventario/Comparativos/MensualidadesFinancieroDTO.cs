using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario.Comparativos
{
    public class MensualidadesFinancieroDTO {
        public int  periodo { get; set; }
        public decimal capital { get; set; }
        public decimal intereses { get; set; }
        public decimal ivaCapital { get; set; }
        public decimal ivaIntereses { get; set; }
        public decimal pagoTotal { get; set; }
        public decimal pagoFinal { get; set; }
    }
}
