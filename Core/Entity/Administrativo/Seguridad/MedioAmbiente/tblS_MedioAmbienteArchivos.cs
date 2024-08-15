using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.MedioAmbiente
{
    public class tblS_MedioAmbienteArchivos
    {
        public int id { get; set; }
        public int idCaptura { get; set; }
        public int tipoArchivo { get; set; }
        public string nombreArchivo { get; set; }
        public string rutaArchivo { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
