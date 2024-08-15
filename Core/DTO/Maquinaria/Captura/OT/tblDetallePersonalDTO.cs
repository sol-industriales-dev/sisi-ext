using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT
{
    public class tblDetallePersonalDTO
    {
        public string folio { get; set; }
        public string economico { get; set; }
        public string motivoParo { get; set; }
        public string inicioParo { get; set; }
        public string finParo { get; set; }

    }
}
