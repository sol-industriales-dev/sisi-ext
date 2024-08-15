using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_Notificantes_Rel_Candidato
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public int idActividad { get; set; }
        public int idCandidato { get; set; }
        public int idNotificarApto { get; set; }
        public bool registroActivo { get; set; }
    }
}
