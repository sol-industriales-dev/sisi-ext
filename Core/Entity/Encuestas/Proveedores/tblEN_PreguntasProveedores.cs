using Core.Entity.Encuestas.Proveedores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas
{
    public class tblEN_PreguntasProveedores
    {
        public int id { get; set; }
        public virtual tblEN_EncuestaProveedores encuesta { get; set; }
        public int encuestaID { get; set; }
        public string pregunta { get; set; }
        public string estatus { get; set; }
        public int tipo { get; set; }
        public bool visible { get; set; }
        public int orden { get; set; }
        public decimal ponderacion { get; set; }

        [ForeignKey("tipo")]
        public virtual tblEN_TipoPregunta TipoPregunta { get; set; }
    }
}
