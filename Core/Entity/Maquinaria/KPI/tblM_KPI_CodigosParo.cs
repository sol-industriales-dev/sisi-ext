using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.KPI
{
    public class tblM_KPI_CodigosParo
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public bool activo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioIDCrea { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int usuarioIDModifica { get; set; }
        public int tipoParo { get; set; }
        public string  areaCuenta { get; set; }
    }
}
