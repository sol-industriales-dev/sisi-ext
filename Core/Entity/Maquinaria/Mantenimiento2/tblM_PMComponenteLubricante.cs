using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento2
{
    public class tblM_PMComponenteLubricante
    {
        public int id { get; set; }
        public int componenteID { get; set; }
        public int lubricanteID { get; set; }
        public int modeloID { get; set; }
        public decimal vidaLubricante { get; set; }
        public decimal cantidadLitros { get; set; }
        public int usuarioID { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool estatus { get; set; }

    }
}
