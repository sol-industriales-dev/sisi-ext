using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Usuarios
{
    public class tblP_Puesto
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public bool estatus { get; set; }
        public int departamentoID { get; set; }
        public virtual tblP_Departamento departamento { get; set; }
        public int nivel { get; set; }
        public int? puestoPadreID { get; set; }
        public virtual List<tblP_Usuario> usuarios { get; set; }
    }
}
