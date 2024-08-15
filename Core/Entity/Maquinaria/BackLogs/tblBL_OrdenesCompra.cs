using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.BackLogs
{
    public class tblBL_OrdenesCompra
    {
        public int id { get; set; }
        public string cc { get; set; }
        public string numRequisicion { get; set; }
        public string numOC { get; set; }
        public int idBackLog { get; set; }
        public string estatus { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacionNumOC { get; set; }
        public DateTime fechaModificacionNumOC { get; set; }
    }
}
