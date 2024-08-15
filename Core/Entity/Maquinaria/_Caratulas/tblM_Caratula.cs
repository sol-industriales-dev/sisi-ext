using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria._Caratulas
{
    public class tblM_Caratula
    {
        public int id { get; set; }
        public int idCaratula { get; set; }
        [ForeignKey("idCaratula")]
        public virtual tblM_CaratulaDet lstCaratula { get; set; }
        public DateTime? fechaAutorizacion { get; set; }
        public int autorizada { get; set; }
        public int usuario { get; set; }
        public decimal tipoCambio { get; set; }
    }
}
