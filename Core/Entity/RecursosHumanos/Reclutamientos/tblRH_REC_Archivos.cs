using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_Archivos
    {
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public int idCandidato { get; set; }
        [ForeignKey("idCandidato")]
        public virtual tblRH_REC_GestionCandidatos virtualLstGestionCandidatos { get; set; }
        public int idFase { get; set; }
        public int idActividad { get; set; }
        [ForeignKey("idActividad")]
        public virtual tblRH_REC_Actividades virtualLstActividades { get; set; }
        public int tipoArchivo { get; set; }
        public string nombreArchivo { get; set; }
        public string descripcion { get; set; }
        public string ubicacionArchivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
