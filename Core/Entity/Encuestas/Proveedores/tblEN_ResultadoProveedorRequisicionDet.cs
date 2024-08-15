using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas
{
    public class tblEN_ResultadoProveedorRequisicionDet
    {
        public int id { get; set; }
        public string centroCostos { get; set; }
        public int numeroRequisicion { get; set; }
        public string nombreProveedor { get; set; }
        public string comentarios { get; set; }
        public bool estatus { get; set; }
        public int evaluadorID { get; set; }
        public int numProveedor { get; set; }
        public DateTime? fechaRequisicion { get; set; }
        public decimal? calificacion { get; set; }
        public DateTime? fechaEvaluacion { get; set; }

        [ForeignKey("evaluadorID")]
        public virtual tblP_Usuario usuario { get; set; }

        [ForeignKey("encuestaFolioID")]
        public virtual List<tblEN_ResultadoProveedorRequisiciones> detalles { get; set; }

        [NotMapped]
        public string tipoMoneda { get; set; }
    }
}
