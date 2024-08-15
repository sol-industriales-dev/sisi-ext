using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad
{
    public class ConciliacionCCDTO
    {
        public int id { get; set; }
        public int ccPrincipal	{ get; set; }
        public string descripcionCCPrincipal	{ get; set; }
        public int ccSecundario { get; set; }
        public string descripcionCCSecundario { get; set; }
        public int idUsuarioRegistro	{ get; set; }
        public bool esActivo	{ get; set; }
        public DateTime fechaRegistro { get; set; }

    }
}
