using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_Grales_Peru
    {
        public string CODAFP { get; set; }
        public bool AFECTOQUINTA { get; set; }
        public bool NOPDT { get; set; }
        public string CARNETSEG { get; set; }
        public string TIPOTRAB { get; set; }
        public string REGIMEN_LABORAL { get; set; }
        public string SITUACIÓN { get; set; }
        public DateTime FECHACESE { get; set; }

        public string RUCEPS { get; set; }
        public string CTACTS { get; set; }

        public string BANCOCTS { get; set; }
    }
}
