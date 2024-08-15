using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Desempeno
{
    public class tblRH_ED_CatProceso
    {
        public int id { get; set; }
        public string proceso { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
        public virtual ICollection<tblRH_ED_CatEvaluacion> lstEvaluacion { get; set; }
        [ForeignKey("ProcesoId")]
        public virtual List<tblRH_ED_RelacionEmpleadoProceso> relacionEP { get; set; }

        [ForeignKey("idProceso")]
        public virtual List<tblRH_ED_DetMetas> Metas { get; set; }
    }
}
