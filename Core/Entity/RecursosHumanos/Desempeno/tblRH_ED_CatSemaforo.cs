using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Desempeno
{
    public class tblRH_ED_CatSemaforo
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public decimal minimo { get; set; }
        public decimal maximo { get; set; }
        public string color { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}
