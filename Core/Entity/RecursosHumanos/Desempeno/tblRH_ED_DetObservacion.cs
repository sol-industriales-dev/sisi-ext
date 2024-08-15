using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Desempeno
{
    public class tblRH_ED_DetObservacion
    {
        public int id { get; set; }
        public int idMeta { get; set; }
        public int idEvaluacion { get; set; }
        public int idUsuario { get; set; }
        public int idJefe { get; set; }
        public decimal resultado { get; set; }
        public decimal autoEvaluacion { get; set; }
        public decimal jefeEvaluacion { get; set; }
        public string autoObservacion { get; set; }
        public string jefeObservacion { get; set; }
        public bool esAutoEvaluado { get; set; }
        public bool esJefeEvaluado { get; set; }
        public bool notificado { get; set; }
        public bool notificadoJefeAUsuario { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
        [ForeignKey("idMeta")]
        public virtual tblRH_ED_DetMetas meta { get; set; }
        [ForeignKey("idEvaluacion")]
        public virtual tblRH_ED_CatEvaluacion evaluacion { get; set; }
        [ForeignKey("idUsuario")]
        public virtual tblP_Usuario usuario { get; set; }
        [ForeignKey("idJefe")]
        public virtual tblP_Usuario jefe { get; set; }
        public virtual ICollection<tblRH_ED_DetObservacionEvidencia> lstEvidencia { get; set; }
    }
}
