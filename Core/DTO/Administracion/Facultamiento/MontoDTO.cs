using Core.Entity.Administrativo.Facultamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Facultamiento
{
    public class MontoDTO
    {
        public tblFa_CatMonto monto { get; set; }
        public List<AutorizacionesDTO> autorizaciones { get; set; }
    }
}
