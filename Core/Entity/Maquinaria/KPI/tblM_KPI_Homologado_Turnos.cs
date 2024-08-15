using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.KPI
{
    public class tblM_KPI_Homologado_Turnos
    {
        public int id { get; set; }
        public string ac { get; set; }
        public int turnos { get; set; }
        public int horas_turno { get; set; }
        public int horas_dia { get; set; }
    }
}
