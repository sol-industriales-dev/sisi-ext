using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DTO.FileManager
{
    public class ArchivoPorSubirDTO
    {
        public string nombre { get; set; }
        public int tipoArchivoID { get; set; }
        public long versionArchivoID { get; set; }
        public HttpPostedFileBase archivo { get; set; }
    }
}
