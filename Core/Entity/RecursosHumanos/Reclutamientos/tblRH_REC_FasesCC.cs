using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_FasesCC
    {
        public int id { get; set; }
        public int idFase { get; set; }
        public int idCC { get; set; }
        public string ccDesc { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
