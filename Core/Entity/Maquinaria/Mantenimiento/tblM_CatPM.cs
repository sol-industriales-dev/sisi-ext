using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento
{
    public class tblM_CatPM
    {
        public int id { get; set; }
        //public int factor { get; set; }
        public string  tipoMantenimiento { get; set; }
        public int PM { get; set; }
    }
}
