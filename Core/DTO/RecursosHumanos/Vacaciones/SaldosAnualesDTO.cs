using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Vacaciones
{
    public class SaldosAnualesDTO
    {
        //public int añoInicial { get; set; }
        //public int añoFinal { get; set; }
        //public string descRango { get; set; }
        //public int saldo_inicial { get; set; }
        //public int saldo_adicional { get; set; }
        //public int dias_gozados { get; set; }
        //public int saldo_actual { get; set; }

        public int clave_empleado { get; set; }
        public string nombre_completo { get; set; }
        public string cc { get; set; }
        public DateTime fecha_alta { get; set; }
        public int años_servicio { get; set; }
        public int dias_ganadosActual { get; set; }
        public int dias_difrutadosActual { get; set; }
        public int dias_pendientesGozarActual { get; set; }
        public decimal dias_proporcionalProximo { get; set; }
        public decimal dias_totalPendientesGozarProximo { get; set; }
        public decimal salario_diario { get; set; }
        public decimal vacaciones { get; set; }
        public decimal prima_vacacional { get; set; }
        public decimal prima_vacacionalProporcional { get; set; }
        public int puesto { get; set; }
        public string estatusEmpleado { get; set; }
    }
}
