using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Facturacion.CFDI33
{
    public class ReceptorDTO
    {
        /// <summary>
        /// RFC
        /// </summary>
        public string Rfc { get; set; }
        /// <summary>
        /// Nombre Cliente
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// ResidenciaFiscal
        /// </summary>
        public string ResidenciaFiscal { get; set; }
        /// <summary>
        /// NumRegIdTrib
        /// </summary>
        public string NumRegIdTrib { get; set; }
        /// <summary>
        /// UsoCFDI (Catálogo: c_UsoCFDI)
        /// </summary>
        public string UsoCFDI { get; set; }
    }
}
