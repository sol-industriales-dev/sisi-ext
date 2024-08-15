using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.DocumentosXPagar
{
    public class cboDocumentosDTO
    {

        /*
         * cc = z.cc,
                    area = z.area,
                    cuenta = z.cuenta,
                    descripcion = z.descripcion.Trim()
                });
         */
        public string cc { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
        public string descripcion { get; set; }
    }
}
