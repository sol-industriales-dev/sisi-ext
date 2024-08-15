using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class PeriodosDTO
    {
        public int periodo { get; set; }
        public DateTime fecha_inicial { get; set; }
        public DateTime fecha_final { get; set; }
        public int tipo_nomina { get; set; }
    }
}
