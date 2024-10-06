using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Cheques
{
    public class tblC_sb_banco
    {
        public int id { get; set; }
        public int banco { get; set; }
        public string descripcion { get; set; }
        public string sucursal { get; set; }
        public string direccion { get; set; }
        public string ciudad { get; set; }
        public int telefonia1 { get; set; }
        public int telefonia2 { get; set; }

        public string reponsable { get; set; }
        public string linea1 { get; set; }
        public string linea2 { get; set; }
        public string desc1 { get; set; }
        public int numpro { get; set; }
        public string desc_banco_sant { get; set; }
    }
}
