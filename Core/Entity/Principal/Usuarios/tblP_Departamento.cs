using Core.Entity.Administrativo.FacultamientosDpto;
using Core.Entity.GestorArchivos;
using Core.Entity.Principal.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Usuarios
{
    public class tblP_Departamento
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public string abreviacion { get; set; }
        public virtual List<tblP_Puesto> puestos { get; set; }
        public virtual List<tblGA_Directorio> listaDirectorios { get; set; }
        public virtual List<tblGA_AccesoDepartamento> accesosDepartamentos { get; set; }
        public bool esDepartamento { get; set; }
        public virtual List<tblP_CC> listaCC { get; set; }
    }
}
