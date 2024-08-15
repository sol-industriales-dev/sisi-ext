using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Razones_Baja
    {
        public int clave_razon_baja { get; set; }
        public string desc_motivo_baja { get; set; }
        public bool? baja_regpat { get; set; }
        public string bloqueado { get; set; }
        public string proceso_baja_inmediata { get; set; }
        public bool? bit_otros { get; set; }
        public string ind_incapacidad_riesgo { get; set; }
    }
}
