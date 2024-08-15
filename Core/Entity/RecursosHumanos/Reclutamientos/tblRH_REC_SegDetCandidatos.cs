using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_SegDetCandidatos
    {
        public int id { get; set; }
        public int idSeg { get; set; }
        public int idActividad { get; set; }
        public decimal calificacion { get; set; }
        public int esAprobada { get; set; }
        public bool esOmitida { get; set; }
        public bool esNotificada { get; set; }
        public string comentario { get; set; }
        public DateTime fechaActividad { get; set; }
        public string firma { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
