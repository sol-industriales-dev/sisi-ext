using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Barrenacion
{
    public class tblB_OtrosGastos
    {
        public int id { get; set; }
        public decimal monto { get; set; }
        public DateTime fechaCaptura { get; set; }
        public string areaCuenta { get; set; }
        public bool estatus { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioCreadorID { get; set; }

    }
}
