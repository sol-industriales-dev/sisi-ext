using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_BN_ListaBlanca
    {
        public int id { get; set; }
        public int cve_Emp { get; set; }
        public string nombre_Emp { get; set; }
        public string puesto_Emp { get; set; }
        public string puestoCve_Emp { get; set; }
        public string cc { get; set; }
        public bool estatus { get; set; }
        public DateTime fecha { get; set; }
        public int usuarioID { get; set; }
        public virtual tblP_Usuario usuario { get; set; }
    }
}
