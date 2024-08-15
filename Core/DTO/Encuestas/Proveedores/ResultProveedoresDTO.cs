using Core.Entity.Encuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas.Proveedores
{
    public class ResultProveedoresDTO
    {

        public int id { get; set; }
        public string centrocostos { get; set; }
        public string centrocostosName { get; set; }
        public int numeroOC { get; set; }
        public DateTime fechaOC { get; set; }
        public int numProveedor { get; set; }
        public string nombreProveedor { get; set; }
        public string tipoProveedor { get; set; }
        public DateTime? fechaAntiguedad { get; set; }
        public string ubicacionProveedor { get; set; }
        public int encuestaID { get; set; }
        public string comentarios { get; set; }
        public bool estadoEncuesta { get; set; }
        public string tipoMoneda { get; set; }
        public int usuarioID { get; set; }
        public string nombreEvaluador { get; set; }
        public decimal ponderacion { get; set; }
        public string resultado { get; set; }

    }
}
