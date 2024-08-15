using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad
{
    public class tblS_CatLicencias
    {
        public int id { get; set; }
        public int cve { get; set; }
        public string nombre { get; set; }
        public string puesto { get; set; }
        public string tipo { get; set; }
        public string numero { get; set; }
        public DateTime vigencia { get; set; }
    }
}
