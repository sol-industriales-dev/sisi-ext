using Core.Entity.Principal.Configuraciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.CuadroComparativo.Equipo
{
    public class tblM_CCE_CatConcepto : RegistroDTO
    {
        public int Id { get; set; }
        public int Orden { get; set; }
        public OdbcType TipoDato { get; set; }
        public string Descripcion { get; set; }
        [ForeignKey("IdConcepto")]
        public virtual IEnumerable<tblM_CCE_DetConcepto> Valores { get; set; }
    }
}
