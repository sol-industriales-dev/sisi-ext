using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Incidencias_Conceptos
    {
        public int id { get; set; }
        public string concepto { get; set; }
        public string abreviatura { get; set; }
        public string cuenta_como_asistencia { get; set; }
        public string bloqueado { get; set; }
        public int asistencia { get; set; }
        public int bit_no_aplica { get; set; }
    }
}
