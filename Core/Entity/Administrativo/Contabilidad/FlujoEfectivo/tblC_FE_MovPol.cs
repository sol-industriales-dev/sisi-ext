using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.FlujoEfectivo
{
    public class tblC_FE_MovPol
    {
        public int id { get; set; }
        /// <summary>
        /// Concepto - Operativo
        /// </summary>
        public int idConcepto { get; set; }
        /// <summary>
        /// Concepto - Directo
        /// </summary>
        public int idConceptoDir { get; set; }
        public int year { get; set; }
        public int mes { get; set; }
        public DateTime fechapol { get; set; }
        public string tp { get; set; }
        public int poliza { get; set; }
        public string cc { get; set; }
        public string ac { get; set; }
        public int linea { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public int numpro { get; set; }
        public string concepto { get; set; }
        public int tm { get; set; }
        public int itm { get; set; }
        public int itmOri { get; set; }
        public decimal monto { get; set; }
        public bool esFlujoEfectivo { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
        [NotMapped]
        public EmpresaEnum empresa { get; set; }
    }
}
