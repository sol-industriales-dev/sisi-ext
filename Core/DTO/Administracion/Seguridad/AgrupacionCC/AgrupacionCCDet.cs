using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.AgrupacionCC
{
    public class AgrupacionCCDet
    {

        public int idDet { get; set; }
        public int id { get; set; }
        public string nomAgrupacion { get; set; }
        public string cc { get; set; }
        public string value { get; set; }
        public bool esActivo { get; set; }
    }
}
