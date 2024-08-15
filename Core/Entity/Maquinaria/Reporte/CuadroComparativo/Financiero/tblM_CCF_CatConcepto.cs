using Core.Entity.Principal.Configuraciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.CuadroComparativo.Financiero
{
    public class tblM_CCF_CatConcepto : RegistroDTO
    {
        public int Id { get; set; }
        public int Orden { get; set; }
        public string Descripcion { get; set; }
        public OdbcType TipoDato { get; set; }
        [ForeignKey("IdConcepto")]
        public virtual IEnumerable<tblM_CCF_DetFinanciero> Detalles { get; set; }
    }
}
