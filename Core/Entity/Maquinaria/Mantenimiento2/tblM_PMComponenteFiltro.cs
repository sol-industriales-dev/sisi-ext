using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento2
{
    public class tblM_PMComponenteFiltro
    {
        public int id { get; set; }
        public int modeloID { get; set; }
        public int componenteID { get; set; }
        public int filtroID { get; set; }
        public bool estatus { get; set; }
        public int usuarioID { get; set; }
        public DateTime fechaCaptura { get; set; }
        public int cantidad { get; set; }

    }

}
