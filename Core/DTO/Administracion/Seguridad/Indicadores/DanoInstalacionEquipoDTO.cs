using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class DanoInstalacionEquipoDTO
    {
        public decimal malaOperacion { get; set; }
        public decimal faltaMantenimiento { get; set; }
        public decimal vidaUtil { get; set; }
        public decimal condicionesClimaticas { get; set; }
        public decimal operativo { get; set; }
        public decimal otros { get; set; }
    }
}
