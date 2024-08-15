using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Propuesta.Proveedores
{
    public class tblC_EstimacionProveedor
    {
        public int id { get; set; }
        public int idGiro { get; set; }
        public int idEst { get; set; }
        public string cc { get; set; }
        public string numpro { get; set; }
        public int tm { get; set; }
        public DateTime fecha { get; set; }
        public decimal total { get; set; }
        public string comentarios { get; set; }
        public DateTime fechaRegistro { get; set; }
        public bool esActivo { get; set; }
    }
}   
