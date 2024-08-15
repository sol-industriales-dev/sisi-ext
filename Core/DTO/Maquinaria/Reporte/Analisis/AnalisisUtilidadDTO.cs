using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Maquinaria.Reporte.Rentabilidad;

namespace Core.DTO.Maquinaria.Reporte.Analisis
{
    public class AnalisisUtilidadDTO
    {
        public int tipo_mov { get; set; }
        public string descripcion { get; set; }
        public decimal actual { get; set; }
        public decimal semana2 { get; set; }
        public decimal semana3 { get; set; }
        public decimal semana4 { get; set; }
        public decimal semana5 { get; set; }

        //80-20
        public decimal cfc { get; set; }
        public decimal cf { get; set; }
        public decimal mc { get; set; }
        public decimal pr { get; set; }
        public decimal tc { get; set; }
        public decimal car { get; set; }
        public decimal ex { get; set; }
        public decimal hdt { get; set; }
        public decimal otros { get; set; }

        public List<RentabilidadDTO> detalles { get; set; }
    }
}