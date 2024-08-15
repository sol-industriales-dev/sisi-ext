using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Facturacion.CFDI33
{
    public class DatosRelacionadosDTO
    {
        /// <summary>
        /// Tipo de Relacion (Catálogo: c_TipoRelacion)
        /// </summary>
        public string CfdiTipoRelacion { get; set; }
        /// <summary>
        /// Folio fiscal (UUID) de un CFDI relacionado.
        /// </summary>
        public string CfdiRelacionado { get; set; }
    }
}
