using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Proyecciones
{
    public class tblPro_SaldosIniciales
    {
        public int id { get; set; }
        public string CadenaJson { get; set; }
        public int Mes { get; set; }
        public int Anio { get; set; }
        public bool Estatus { get; set; }
    }
}
