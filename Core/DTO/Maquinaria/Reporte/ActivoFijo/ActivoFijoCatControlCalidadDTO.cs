using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoCatControlCalidadDTO
    {
        public DateTime FechaCaptura { get; set; }
        public int Año { get; set; }
        public int Mes { get; set; }
        public string AñoMes { get; set; }
        public int Dia { get; set; }
        public string Obra { get; set; }
    }
}