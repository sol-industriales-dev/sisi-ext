using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_RelInstitucionCta
    {

        public int id { get; set; }
        public int institucionID { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public int digito { get; set; }
        public bool activo { get; set; }
        public int moneda { get; set; }
        public bool complementaria { get; set; }

    }
}
