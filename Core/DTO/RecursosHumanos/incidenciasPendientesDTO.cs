using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class incidenciasPendientesDTO
    {
        public int id { get; set; }
        public int id_incidencia { get; set; }
        public string cc { get; set; }
        public int anio { get; set; }
        public int tipo_nomina { get; set; }
        public int periodo { get; set; }
        public string fechas { get; set; }
        public bool cambio_pendiente { get; set; }
        public int evaluacion_pendiente { get; set; }
        public DateTime fecha_inicio { get; set; }
    }
}
