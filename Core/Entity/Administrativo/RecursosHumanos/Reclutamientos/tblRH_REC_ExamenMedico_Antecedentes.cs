using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_ExamenMedico_Antecedentes
    {
        public int id { get; set; }
        public int examenMedico_id { get; set; }
        public int antecedente { get; set; }
        public bool registroActivo { get; set; }
    }
}
