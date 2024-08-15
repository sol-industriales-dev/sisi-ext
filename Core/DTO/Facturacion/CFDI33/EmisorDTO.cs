using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Facturacion.CFDI33
{
    public class EmisorDTO
    {
        /// <summary>
        /// RFC del emisor
        /// </summary>
        public string Rfc { get; set; }
        /// <summary>
        /// Nombre del emisor
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Régimen Fiscal (Catálogo: c_RegimenFiscal)
        /// </summary>
        public string RegimenFiscal { get; set; }
    }
}
