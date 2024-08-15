using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_BN_EstatusPeriodos
    {
        public int id { get; set; }
        public int anio { get; set; }
        public int tipo_nomina { get; set; }
        public int periodo { get; set; }
        public DateTime fecha_inicial { get; set; }
        public DateTime fecha_final { get; set; }
        public bool estatus { get; set; }
        public DateTime fecha_limite { get; set; }
    }
}
