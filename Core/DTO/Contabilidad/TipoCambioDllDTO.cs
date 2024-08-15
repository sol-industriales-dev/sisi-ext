using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad
{
    public class TipoCambioDllDTO
    {
        public string moneda { get; set; }
        public DateTime fecha { get; set; }
        public decimal tipo_cambio { get; set; }
        public decimal empleado_modifica { get; set; }
        public DateTime fecha_modifica { get; set; }
        public TimeSpan hora_modifica { get; set; }
        public decimal tc_anterior { get; set; }
    }
}
