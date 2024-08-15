using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Desempeno
{
    public class tblRH_ED_DetObservacionEvidencia
    {
        public int id { get; set; }
        public int idObservacion { get; set; }
        public string nombre { get; set; }
        public string ruta { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
        [ForeignKey("idObservacion")]
        public tblRH_ED_DetObservacion observacion { get; set; }
    }
}
