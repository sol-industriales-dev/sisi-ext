using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Barrenacion
{
    public class tblB_CapturaConsumoAgua
    {
        public int id { get; set; }
        public decimal cantidadLitros { get; set; }
        public int turno { get; set; }
        //public int MyProperty { get; set; }
    }
}
