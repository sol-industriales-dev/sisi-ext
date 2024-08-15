using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas
{
    public class tblEN_ResultadoProveedoresDet
    {
        public int id { get; set; }
        public string centrocostos { get; set; }
        public int numeroOC { get; set; }
        public DateTime? fechaOC { get; set; }
        public int numProveedor { get; set; }
        public string nombreProveedor { get; set; }
        public string tipoProveedor { get; set; }
        public DateTime fechaAntiguedad { get; set; }
        public string ubicacionProveedor { get; set; }
        public string comentarios { get; set; }
        public bool estadoEncuesta { get; set; }
        public string tipoMoneda { get; set; }
        public int evaluadorID { get; set; }
        public int encuestaID { get; set; }
        public decimal? calificacion { get; set; }
        public DateTime? fechaEvaluacion { get; set; }

        [ForeignKey("evaluadorID")]
        public virtual tblP_Usuario usuario { get; set; }

        [ForeignKey("encuestaFolioID")]
        public virtual List<tblEN_ResultadoProveedores> detalles { get; set; }
    }
}
