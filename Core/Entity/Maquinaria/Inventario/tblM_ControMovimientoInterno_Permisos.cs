using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_ControMovimientoInterno_Permisos
    {
        public int id { get; set; }
        public int tipo { get; set; }
        public string cc { get; set; }
        public int usuarioID { get; set; }
    }
}
