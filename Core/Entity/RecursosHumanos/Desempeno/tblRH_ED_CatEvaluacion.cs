using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Desempeno
{
    public class tblRH_ED_CatEvaluacion
    {
        public int id { get; set; }
        public int idProceso { get; set; }
        public decimal? calificacion { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
        [ForeignKey("idProceso")]
        public virtual tblRH_ED_CatProceso proceso { get; set; }
        [ForeignKey("idEvaluacion")]
        public virtual List<tblRH_ED_DetObservacion> observaciones { get; set; }
    }
}
