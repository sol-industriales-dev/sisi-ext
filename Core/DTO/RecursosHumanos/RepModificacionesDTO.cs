using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class RepModificacionesDTO
    {
        public string cC { get; set; }
        public string cCSolo { get; set; }
        public string concepto { get; set; }
        public string puesto { get; set; }
        public string cantidad { get; set; }
        public string fechaStr { get; set; }
        public DateTime fecha { get; set; }
        public string solicitaID { get; set; }
        public string solicita { get; set; }
        public string observacion { get; set; }
        public string autoriza { get; set; }
        public string categoria { get; set; }
    }
}
