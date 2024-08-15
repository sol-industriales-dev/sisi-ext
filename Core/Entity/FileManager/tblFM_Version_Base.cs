using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.FileManager
{
    public class tblFM_Version_Base
    {
        public long id { get; set; }
        public long archivoID { get; set; }
        public int usuarioCreadorID { get; set; }
        public int version { get; set; }
        public string ruta { get; set; }
        public string nombre { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaEdicion { get; set; }
        public int numeroArchivo { get; set; }
        public bool activo { get; set; }
        public string abreviacion { get; set; }
        public bool considerarse { get; set; }
    }
}
