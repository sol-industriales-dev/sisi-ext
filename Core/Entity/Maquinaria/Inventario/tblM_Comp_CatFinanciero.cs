using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_Comp_CatFinanciero
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaRegistro { get; set; }
        public int usuarioRegistra { get; set; }
        public int estado { get; set; }
    }
}
