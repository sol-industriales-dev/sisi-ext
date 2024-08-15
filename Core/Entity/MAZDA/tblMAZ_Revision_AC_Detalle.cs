using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.MAZDA
{
    public class tblMAZ_Revision_AC_Detalle
    {
        public int id { get; set; }
        public int tipo { get; set; }
        public int actividadID { get; set; }
        public bool realizo { get; set; }
        public string observaciones { get; set; }
        public int revisionID { get; set; }
        public bool estatus { get; set; }

        public string ultMant { get; set; }
        public string sigMant { get; set; }
        public string reprogramacion { get; set; }
        public string estatusInfo { get; set; }
    }
}
