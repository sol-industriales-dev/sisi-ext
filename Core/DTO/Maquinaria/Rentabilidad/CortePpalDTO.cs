using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Rentabilidad
{
    public class CortePpalDTO
    {
        public string cuenta { get; set; }
        public decimal monto { get; set; }
        public int tipoEquipo { get; set; }
        public string cc { get; set; }
        public int semana { get; set; }
        public string areaCuenta { get; set; }
        public int tipoMov { get; set; }
        public string referencia { get; set; }
        public int empresa { get; set; }
        public bool acumulado { get; set; }
    }
}
