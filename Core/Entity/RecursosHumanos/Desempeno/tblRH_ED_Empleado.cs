using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.RecursosHumanos;
using Core.Entity.Principal.Usuarios;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.RecursosHumanos.Desempeno
{
    public class tblRH_ED_Empleado
    {
        public int id { get; set; }
        public int empleadoID { get; set; }
        public int? jefeID { get; set; }
        public ED_TipoEmpleado tipo { get; set; }
        public bool estatus { get; set; }
        [ForeignKey("empleadoID")]
        public virtual tblP_Usuario usuario { get; set; }
        [ForeignKey("jefeID")]
        public virtual tblRH_ED_Empleado jefe { get; set; }
        [ForeignKey("EmpleadoId")]
        public virtual List<tblRH_ED_RelacionEmpleadoProceso> procesos { get; set; }
    }
}
