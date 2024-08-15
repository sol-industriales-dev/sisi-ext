using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Vacaciones
{
    public class SaldosExcelMensualDTO
    {
        public int clave_empleado { get; set; }
        public string nombre_completo { get; set; }
        public string cc { get; set; }
        public DateTime fecha_alta { get; set; }
        public decimal años_servicio { get; set; }
        public decimal dias_pendientesGozarAnt { get; set; }
        public decimal dias_periodoActual { get; set; }
        public decimal dias_disfrutados { get; set; }
        public decimal dias_pendienteGozar { get; set; }
        public decimal dias_totalPendientesGozar { get; set; }
        public decimal salario_diario { get; set; }
        public decimal vacaciones { get; set; }
        public decimal prima_vacacional { get; set; }
        public int puesto { get; set; }
    }
}
