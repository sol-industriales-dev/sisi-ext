using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_GestionSolicitudes
    {
        public int id { get; set; }
        public int idSolicitud { get; set; }
        [ForeignKey("idSolicitud")]
        public virtual tblRH_REC_Solicitudes virtualLstSolicitudes { get; set; }
        public bool esAutorizada { get; set; }
        public string motivoRechazo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
