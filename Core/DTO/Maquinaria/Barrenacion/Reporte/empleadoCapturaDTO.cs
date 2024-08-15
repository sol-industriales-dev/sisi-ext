using Core.Enum.Maquinaria.Barrenacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Barrenacion.Reporte
{
    public class empleadoCapturaDTO
    {
        public int claveEmpleado { get; set; }
        public TipoOperadorEnum tipoOperador { get; set; }
        public int turno { get; set; }
        public string nombre { get; set; }
    }
}
