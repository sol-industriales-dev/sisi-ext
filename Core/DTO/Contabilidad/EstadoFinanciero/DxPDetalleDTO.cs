using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.EstadoFinanciero
{
    public class DxPDetalleDTO
    {
        public string documento { get; set; }
        public decimal tasa { get; set; }
        public decimal totales { get; set; }
        public decimal anio { get; set; }
        public decimal anioMas1 { get; set; }
        public decimal anioMas2 { get; set; }
        public decimal anioMas3 { get; set; }
        public decimal anioMas4 { get; set; }
        public bool renglonGrupo { get; set; }
    }

    public class DxPDetallePuroDTO
    {
        public string documento { get; set; }
        public string equipo { get; set; }
        public string inicio { get; set; }
        public string fin { get; set; }
        public decimal renta { get; set; }
        public int pendientes { get; set; }
        public decimal anio { get; set; }
        public decimal anio2 { get; set; }
        public bool renglonGrupo { get; set; }
    }
}
