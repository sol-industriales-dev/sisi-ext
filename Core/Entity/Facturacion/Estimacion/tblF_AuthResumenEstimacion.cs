using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Facturacion.Estimacion
{
    public class tblF_AuthResumenEstimacion
    {
        public int id { get; set; }
        /// <summary>
        /// stAuthEnum
        /// </summary>
        public int stAuth { get; set; }
        public DateTime fechaResumen { get; set; }
        public int usuarioID { get; set; }
        public string firma { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool esActivo { get; set; }
    }
}
