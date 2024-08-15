using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_SM_Justificacion
    {
        public int id { get; set; }
        public int solicitudID { get; set; }
        public int grupoID { get; set; }
        public string grupo { get; set; }
        public int modeloID { get; set; }
        public string modelo { get; set; }
        public string justificacion { get; set; }
    }
}
