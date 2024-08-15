using Core.Entity.Principal.Usuarios;
using System.Collections.Generic;

namespace Core.Entity.GestorArchivos
{
    public class tblGA_Directorio
    {

        public int id { get; set; }
        public int padreID { get; set; }
        public int nivel { get; set; }
        public bool esCarpeta { get; set; }
        public int departamentoID { get; set; }
        public virtual tblP_Departamento departamento { get; set; }
        public virtual List<tblGA_Version> versiones { get; set; }
        public virtual List<tblGA_Vistas> vistas { get; set; }
        public virtual List<tblGA_Permisos> permisos { get; set; }

    }
}
