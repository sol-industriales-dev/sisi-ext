using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Proyecciones
{
    public class tblPro_CapturadeObras
    {
        public int id { get; set; }
        public string CadenaJson { get; set; }
        public int Escenario { get; set; }
        public int MesInicio { get; set; }
        public int EjercicioInicial { get; set; }
    }
}
