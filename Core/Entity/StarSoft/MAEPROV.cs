using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft
{
    [Table("MAEPROV")]
    public class MAEPROV
    {
        [Key]
        public string PRVCCODIGO { get; set; }
        public string PRVCNOMBRE { get; set; }
        public string PRVCESTADO { get; set; }
        public string PRVPAGO { get; set; }
        public string PRVCDIRECC { get; set; }
        public string PRVCLOCALI { get; set; }
        public string PRVCTELEF1 { get; set; }
        public string PRVCRUC { get; set; }
        public string PRVCDOCIDEN { get; set; }

        [NotMapped]
        public string PRVCUSER { get; set; }
        public string PRVCTIPO_DOCUMENTO { get; set; }
        public string PRVCAPELLIDO_PATERNO { get; set; }
        public string PRVCAPELLIDO_MATERNO { get; set; }
        public string PRVCPRIMER_NOMBRE { get; set; }
        public string PRVCSEGUNDO_NOMBRE { get; set; }
        //public DateTime? FEC_INACTIVO_BLOQUEADO { get; set; }
        public string COD_AUDITORIA { get; set; }
        public string UBIGEO { get; set; }
        public DateTime? PRVDFECCRE { get; set; }
        public bool? FLGPORTAL_PROVEEDOR { get; set; }
    }
}
