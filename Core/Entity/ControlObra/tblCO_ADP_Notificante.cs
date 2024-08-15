using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra
{
    public class tblCO_ADP_Notificante
    {
        public int id { get; set; }
        public string puesto { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public bool esFijo { get; set; }
        public string cc { get; set; }
        public bool registroActivo { get; set; }
    }
}
