using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Cheque
{
    public class filtroBusquedaDTO
    {
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public int cuenta { get; set; }
        public bool chequesElectronico { get; set; }
    }
}
