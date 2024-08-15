using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.SaludOcupacional
{
    public class ArchivoDTO
    {
        public int id { get; set; }
        public int idHC { get; set; }
        public string nombreArchivo { get; set; }
        public string rutaArchivo { get; set; }
        public int tipoArchivo { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
