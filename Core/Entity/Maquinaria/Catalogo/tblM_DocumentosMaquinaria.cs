using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Catalogo
{
   public class tblM_DocumentosMaquinaria
    {
        public int id { get; set; }
        public int economicoID { get; set; }
        public string nombreRuta { get; set; }
        public string nombreArchivo { get; set; }
        public int tipoArchivo { get; set; }
        public DateTime fechaCarga { get; set; }
        public int usuarioSubeArchivo { get; set; }
    }
}
