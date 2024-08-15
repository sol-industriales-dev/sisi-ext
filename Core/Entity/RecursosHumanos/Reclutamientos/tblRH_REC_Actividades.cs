using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_Actividades
    {
        public int id { get; set; }
        public int idFase { get; set; }
        [ForeignKey("idFase")]
        public virtual tblRH_REC_Fases virtualLstFases { get; set; }
        public string tituloActividad { get; set; }
        public string descActividad { get; set; }
        public bool esArchivos { get; set; }
        public bool esObligatoria { get; set; }
        public bool esGeneral { get; set; }
        public bool esCalificacion { get; set; }
        public bool esNecesarioAprobar { get; set; }
        public int? tipoArchivo { get; set; }
        public int idUsuarioEncargado { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
