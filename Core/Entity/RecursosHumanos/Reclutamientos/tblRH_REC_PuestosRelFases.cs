using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_PuestosRelFases
    {
        public int id { get; set; }
        public int idFase { get; set; }
        [ForeignKey("idFase")]
        public virtual tblRH_REC_Fases virtualLstFases { get; set; }
        public int idPuesto { get; set; }
        public string puesto { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}