using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class potencialSeveridadDTO
    {
        public decimal menor { get; set; }
        public decimal moderado { get; set; }
        public decimal mayor { get; set; }
        public decimal catastrotico { get; set; }
    }
}
