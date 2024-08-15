using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta.Nomina
{
    public class PeriodosNominaDTO
    {
        public int tipo_nomina { get; set; }
        public int periodo { get; set; }
        public int tipo_periodo { get; set; }   
        public DateTime fecha_inicial { get; set; }
        public DateTime fecha_final { get; set; }
        public DateTime ? fecha_pago { get; set; }
        public int mes_cc { get; set; }
        public string fecha_inicialStr { get; set; }
        public string fecha_finalStr { get; set; }
        public string fecha_pagoStr { get; set; }
        public int year { get; set; }
        public PeriodosNominaDTO()
        {
            this.fecha_inicialStr = fecha_inicial.ToShortDateString();
            this.fecha_finalStr = fecha_final.ToShortDateString();
            this.fecha_pagoStr = (fecha_pago ?? DateTime.Now).ToShortDateString();
        }
    }
}
