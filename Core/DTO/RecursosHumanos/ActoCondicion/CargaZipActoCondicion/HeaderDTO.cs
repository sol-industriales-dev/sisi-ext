using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.ActoCondicion.CargaZipActoCondicion
{
    public class HeaderDTO
    {
        public int columna { get; set; }
        public string columnaConLetras { get; set; }
        public string nombreHeader { get; set; }
        public bool esCausaAccion { get; set; }
        public int? causaAccionId { get; set; }
    }
}
