using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.MAZDA
{
    public class RevisionCua_DetalleDTO
    {
        public int id { get; set; }
        public int actividadID { get; set; }
        public string actividad { get; set; }
        public bool realizo { get; set; }
        public string estadoString { get; set; }
        public int revisionID { get; set; }
    }
}
