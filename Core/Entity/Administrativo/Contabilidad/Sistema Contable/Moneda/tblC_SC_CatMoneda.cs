using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Sistema_Contable.Moneda
{
    /// <summary>
    /// Catálogo de Monedas/Divisas
    /// </summary>
    public class tblC_SC_CatMoneda
    {
        public int Id { get; set; }
        public string Moneda { get; set; }
        /// <summary>
        /// Clave de la moneda de tblC_SC_TipoCambio
        /// </summary>
        public int Clave { get; set; }
        public string Denominacion { get; set; }
        public string Codigo { get; set; }
        public string Idioma { get; set; }
        public int idAccion { get; set; }
    }
}
