using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas.Proveedores.Reportes
{
    public class InfoEncabezadoEvaluacion
    {
        public string nombreProveedor { get; set; }
        public string fechaEvaluacion { get; set; }
        public string tipodeProveedor { get; set; }
        public string antiguedadDelProveedor { get; set; }
        public string evaluador { get; set; }
        public string comentario { get; set; }
        public string firmaEvaluador { get; set; }
    }
}
