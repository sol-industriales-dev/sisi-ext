using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Desempeno
{
    public class tblRH_ED_RelacionEmpleadoProceso
    {
        public int Id { get; set; }
        public int ProcesoId { get; set; }
        public int EmpleadoId { get; set; }
        public bool Estatus { get; set; }

        [ForeignKey("ProcesoId")]
        public virtual tblRH_ED_CatProceso Proceso { get; set; }
        [ForeignKey("EmpleadoId")]
        public virtual tblRH_ED_Empleado Empleado { get; set; }
    }
}