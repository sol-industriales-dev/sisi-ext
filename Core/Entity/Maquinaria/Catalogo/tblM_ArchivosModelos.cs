using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Catalogo
{
    public class tblM_ArchivosModelos
    {
        public int id { get; set; }
        public int modeloID { get; set; }
        public string RutaArchivo { get; set; }
        public string nombreArchivo { get; set; }
        public DateTime  fechaCreacion { get; set; }
        public int usuario { get; set; }
        public virtual tblM_CatModeloEquipo modeloEquipo { get; set; }
    }
}
