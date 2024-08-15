using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas.Proveedores.Reportes
{
    public class tblRequisicionesDTO
    {
        public int id { get; set; }
        public string centroCostos { get; set; }
        public string centroCostosName { get; set; }
        public string fechaRequisicion { get; set; }
        public int noRequisicion { get; set; }
        public string  requisicion { get; set; }
        public string comentarios { get; set; }
        public bool estatus { get; set; }

        public int numeroProveedor { get; set; }
        public string nombreProveedor { get; set; }
        public string monedaProveedor { get; set; }
        public string nombreEvaluador { get; set; }
        public DateTime fechaEvaluacion { get; set; }
        public decimal calificacion { get; set; }
        public string comentario { get; set; }
    }
}
