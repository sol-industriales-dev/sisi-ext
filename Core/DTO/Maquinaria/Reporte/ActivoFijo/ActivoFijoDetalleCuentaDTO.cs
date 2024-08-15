using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoDetalleCuentaDTO
    {
        public int Cuenta { get; set; }
        public string Descripcion { get; set; }
        public List<ActivoFijoDetalleDTO> Detalles { get; set; }
    }
}