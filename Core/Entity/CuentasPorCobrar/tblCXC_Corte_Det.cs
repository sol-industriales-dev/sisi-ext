using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.CuentasPorCobrar
{
    public class tblCXC_Corte_Det
    {
        public int id { get; set; }
        //public int idCorte { get; set; }
        public string idFactura { get; set; }
        public DateTime fechaCorte { get; set; }
        public string comentariosRemove { get; set; }
        public bool esRemoved { get; set; }
        public int idUsuarioCreacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool esActivo { get; set; }

    }
}
