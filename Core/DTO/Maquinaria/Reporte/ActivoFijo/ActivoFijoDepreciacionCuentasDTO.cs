using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoDepreciacionCuentasDTO
    {
        public int Id { get; set; }
        public int Cuenta { get; set; }
        public string Descripcion { get; set; }
        public decimal PorcentajeDepreciacion { get; set; }
        public int MesesDeDepreciacion { get; set; }
    }
}