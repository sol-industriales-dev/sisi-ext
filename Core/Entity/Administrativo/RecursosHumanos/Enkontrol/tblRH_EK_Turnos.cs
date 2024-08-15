using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Turnos
    {
        public int clave_turno { get; set; }
        public string desc_turno { get; set; }
        public decimal horas_dia { get; set; }
        public decimal horas_semana { get; set; }
        public string leyeda_horario { get; set; }
        public string desc_dom { get; set; }
        public string desc_lun { get; set; }
        public string desc_mar { get; set; }
        public string desc_mie { get; set; }
        public string desc_jue { get; set; }
        public string desc_vie { get; set; }
        public string desc_sab { get; set; }
    }
}
