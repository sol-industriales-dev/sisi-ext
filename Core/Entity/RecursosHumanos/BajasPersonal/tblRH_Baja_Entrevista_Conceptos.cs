using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Bajas
{
    public class tblRH_Baja_Entrevista_Conceptos
    {
        public int id { get; set; }
        public int preguntaID { get; set; }
        public string concepto { get; set; }
        public int orden { get; set; }
        public bool estatus { get; set; }
    }
}
