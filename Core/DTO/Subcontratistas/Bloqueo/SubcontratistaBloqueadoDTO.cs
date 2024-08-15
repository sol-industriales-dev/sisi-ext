using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Subcontratistas.Bloqueo
{
    public class SubcontratistaBloqueadoDTO
    {
        public int id { get; set; }
        public int numeroProveedor { get; set; }
        public string nombreProveedor { get; set; }
        public int tipoBloqueoID { get; set; }
        public string descripcionTipoBloqueo { get; set; }
    }
}
