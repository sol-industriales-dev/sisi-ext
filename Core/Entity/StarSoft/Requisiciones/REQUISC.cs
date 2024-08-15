using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.StarSoft.Requisiciones
{
    [Table("REQUISC")]
    public class REQUISC
    {
        [Key, Column(Order = 0)]
        public string NROREQUI { get; set; }
        public string CODSOLIC { get; set; }
        public DateTime FECREQUI { get; set; }
        public string GLOSA { get; set; }
        public string AREA { get; set; }
        public string ESTREQUI { get; set; }
        public string RQTDREF { get; set; }
        public string RQNOMREF { get; set; }
        [Key, Column(Order = 1)]
        public string TIPOREQUI { get; set; }
        public string COD_AUDITORIA { get; set; }
        public string TIPO_USUARIO { get; set; }
        public string NOMBRE_USUARIO { get; set; }
        public string CARGO_USUARIO { get; set; }
        //public File ARCHIVO_FIRMA { get; set; } ??
        public string COD_USUARIO { get; set; }
        public string TIPODOCUMENTO { get; set; }
        public string NRO_REQUERIMIENTO_PORTAL { get; set; }
    }
}
