using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.RecursosHumanos.BajasPersonal;

namespace Core.DTO.RecursosHumanos.BajasPersonal
{
    public class AutorizacionBajaDTO
    {
        public int baja_id { get; set; }
        public AutorizacionEnum autorizada { get; set; }
        public string comentariosAutorizacion { get; set; }
        public string comentariosCancelacion { get; set; }
    }
}
