using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_AFP
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public int tipoComision { get; set; }
        public decimal aporte { get; set; }
        public decimal comision { get; set; }
        public decimal primaSeguro { get; set; }
        public decimal topePrimaSeguro { get; set; }
        public DateTime fechaRegistro { get; set; }
        public int usuarioRegistro { get; set; }
        public DateTime fechaModifica { get; set; }
        public int usuarioModifica { get; set; }
        public bool registroActivo { get; set; }
    }
}
