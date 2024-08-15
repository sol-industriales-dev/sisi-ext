using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Poliza
{
    public class CtaDifCambiariaDTO
    {
        public int cta_dif_cambiaria { get; set; }
        public int scta_dif_cambiaria { get; set; }
        public int sscta_dif_cambiaria { get; set; }
        public int digito_dif_cambiaria { get; set; }
        public int cta_ini { get; set; }
        public int scta_ini { get; set; }
        public int sscta_ini { get; set; }
        public int digito_ini { get; set; }
        public int cta_fin { get; set; }
        public int scta_fin { get; set; }
        public int sscta_fin { get; set; }
        public int digito_fin { get; set; }
        public int cta_perdida { get; set; }
        public int scta_perdida { get; set; }
        public int sscta_perdida { get; set; }
        public int digito_perdida { get; set; }
    }
}
