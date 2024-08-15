using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.DatosDiarios
{
    public class datosDiariosDTO
    {
        public DateTime fecha { get; set; }
        public string areaCuenta { get; set; }
        public string status { get; set; }
        public int Estado { get; set; }
        public int ModeloEquipo { get; set; }
        public string Economico { get; set; }
    }
}
