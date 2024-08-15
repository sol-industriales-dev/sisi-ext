
using System.Collections.Generic;
namespace Core.Entity.FileManager
{
    public class tblFM_TipoArchivo
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public bool nombreLibre { get; set; }
        public virtual List<tblFM_ArchivotblFM_TipoArchivo> listaArchivos { get; set; }
    }
}
