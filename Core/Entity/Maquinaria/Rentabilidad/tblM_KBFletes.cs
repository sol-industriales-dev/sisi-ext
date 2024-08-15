using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Rentabilidad
{
    public class tblM_KBFletes
    {
        public int id { get; set; }
        public int economicoID { get; set; }
        public string noEconomico { get; set; }
        public string areaCuenta { get; set; }
        public DateTime fechaCreacion { get; set; }
        public bool estatus { get; set; }
        public int usuarioCreadorID { get; set; }
    }
}
