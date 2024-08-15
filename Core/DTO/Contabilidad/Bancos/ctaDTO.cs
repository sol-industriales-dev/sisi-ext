using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Bancos
{
    public class ctaDTO
    {
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string  descripcion { get; set; }
        public int digito { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public int tipoS { get; set; } // Es el tipo de busqueda para la tabla. 
        public string requiere_oc { get; set; }
    }
}
