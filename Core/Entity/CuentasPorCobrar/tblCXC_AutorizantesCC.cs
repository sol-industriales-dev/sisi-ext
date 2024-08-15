using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.CuentasPorCobrar
{
    public class tblCXC_AutorizantesCC
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string cc { get; set; }
        public bool esNotificar { get; set; }
        public bool esActivo { get; set; }
    }
}
