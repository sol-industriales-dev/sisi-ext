using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Poliza
{
    public class CatctaDTO
    {
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string descripcion { get; set; }
        public int digito { get; set; }
        public string requiere_oc { get; set; }
        public string lblError { get; set; }
        public int moneda { get; set; }
    }
}
