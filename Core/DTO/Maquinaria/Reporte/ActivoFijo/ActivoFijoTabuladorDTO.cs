using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoTabuladorDTO
    {
        public int Año { get; set; }
        public int Mensualidad { get; set; }
        public string Mes { get; set; }
        public decimal DepreciacionMensual { get; set; }
        public decimal DepreciacionSemanal { get; set; }
        public decimal DepreciacionAcumulada { get; set; }
        public decimal ValorEnLibros { get; set; }
        public string AreaCuenta { get; set; }
    }
}