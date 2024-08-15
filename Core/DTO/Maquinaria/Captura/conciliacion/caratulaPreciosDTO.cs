using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.conciliacion
{
    public class caratulaPreciosDTO
    {
        public int ID { get; set; }
        public string Equipo { get; set; }
        public string Unidad { get; set; }

        public int Cantidad { get; set; }

        public decimal Costo { get; set; }
        public decimal CostoTotal { get; set; }
        public string tipoCambio { get; set; }
        public string Modelo { get; set; }
    }
}
