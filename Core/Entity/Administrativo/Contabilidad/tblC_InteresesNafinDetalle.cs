using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblC_InteresesNafinDetalle
    {
        public int id { get; set; }
        public int idInteres { get; set; }
        public DateTime fecha { get; set; }
        public string cc { get; set; }
        public decimal interesFactoraje { get; set; }
        public int divisa { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCaptura { get; set; }
    }
}
