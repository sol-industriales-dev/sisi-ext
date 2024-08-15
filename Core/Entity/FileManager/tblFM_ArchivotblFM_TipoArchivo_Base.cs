using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.FileManager
{
    public class tblFM_ArchivotblFM_TipoArchivo_Base
    {
        public long id { get; set; }
        public long archivoID { get; set; }
        public int tipoArchivoID { get; set; }
    }
}
