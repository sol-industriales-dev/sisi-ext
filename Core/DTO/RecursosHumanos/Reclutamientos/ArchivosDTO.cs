using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class ArchivosDTO
    {
        public int id { get; set; }
        public int idCandidato { get; set; }
        public int claveEmpleado { get; set; }
        public int idFase { get; set; }
        public int idActividad { get; set; }
        public int tipoArchivo { get; set; }
        public string nombreArchivo { get; set; }
        public string descripcion { get; set; }
        public string ubicacionArchivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
        List<HttpPostedFileBase> lstArchivos { get; set; }
    }
}
