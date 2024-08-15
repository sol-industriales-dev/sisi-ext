using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Catalogo
{
    public class tblRH_CatEmpleados
    {
        public int clave_empleado { get; set; }
        public string Nombre { get; set; }
        public string Puesto { get; set; }
        public int cvePuesto { get; set; }

        public string cc { get; set; }
        public string ccDescrip { get; set; }
        public string ape_paterno { get; set; }
        public string ape_materno { get; set; }
        public DateTime? fechaBaja { get; set; }

    }
}
