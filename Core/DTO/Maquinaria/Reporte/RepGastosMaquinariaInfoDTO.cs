using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte
{
    public class RepGastosMaquinariaInfoDTO
    {
        public int mes { get; set; }
        public double importe { get; set; }
        public string descripcion {get;set;}
        public int anio { get; set; }
    }
}
