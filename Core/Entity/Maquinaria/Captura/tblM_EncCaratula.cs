using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_EncCaratula
    {
        public int id { get; set; }
        public int ccID { get; set; }
        public DateTime creacion { get; set; }
        public int idUsuario { get; set; }
        public bool isActivo { get; set; }
        public int moneda { get; set; }
        public DateTime fechaVigencia { get; set; }
    }
}
