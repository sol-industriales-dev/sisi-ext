using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad
{
    public class tblS_FileVehiculo
    {
        public int id { get; set; }
        public int idVehiculo { get; set; }
        public string rutaArchivo { get; set; }
        public string nombreArchivo { get; set; }
        public DateTime subida { get; set; }
        public int usuario { get; set; }
    }
}
