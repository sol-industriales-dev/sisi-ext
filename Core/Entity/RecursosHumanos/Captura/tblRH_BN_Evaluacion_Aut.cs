using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_BN_Evaluacion_Aut
    {
        public int id { get; set; }
        public int evaluacionID { get; set; }
        public virtual tblRH_BN_Evaluacion evaluacion { get; set; }
        public int aprobadorClave { get; set; }
        [ForeignKey("aprobadorClave")]
        public virtual tblP_Usuario aprobador { get; set; }
        public string aprobadorNombre { get; set; }
        public string aprobadorPuesto { get; set; }
        public string tipo { get; set; }
        public int estatus { get; set; }
        public string firma { get; set; }
        public bool autorizando { get; set; }
        public int orden { get; set; }
        public string comentario { get; set; }
        public DateTime fecha { get; set; }
    }
}
