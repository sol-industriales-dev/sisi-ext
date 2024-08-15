using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.BackLogs
{
    public class BackLogs_ArchivosDTO
    {

        public int id { get; set; }
        public int idBL { get; set; }
        public string nombreArchivo { get; set; }
        public string tipoEvidencia { get; set; }
        public int intTipoEvidencia { get; set; }
        public string rutaArchivo { get; set; }
    }
}
