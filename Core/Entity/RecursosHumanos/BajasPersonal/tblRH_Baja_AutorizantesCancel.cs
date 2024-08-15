using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.BajasPersonal
{
    public class tblRH_Baja_AutorizantesCancel
    {

        public int id { get; set; }
        public int idUsuario { get; set; }
        public bool registroActivo { get; set; }
    }
}
