using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado
{
    public class tblC_EstimacionCobranza
    {
        public int id { get; set; }
        public string cc { get; set; }
        public DateTime fecha { get; set; }
        public decimal estimado { get; set; }
        public decimal semana1 { get; set; }
        public decimal semana2 { get; set; }
        public decimal semana3 { get; set; }
        public DateTime fechaRegistro { get; set; }
        public bool esActivo { get; set; }
    }
}
