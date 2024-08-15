using Core.Entity.Principal.Configuraciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Sistema_Contable.Moneda
{
    /// <summary>
    /// Catálogo de tipo de cambio por moneda.
    /// </summary>
    public class tblC_SC_TipoCambio : RegistroDTO
    {
        public int Id { get; set; }
        public int Moneda { get; set; }
        public DateTime Fecha { get; set; }
        public decimal TipoCambio { get; set; }
        public int Empleado_modifica { get; set; }
        public DateTime FechaModifica { get; set; }
        public DateTime HoraModifica { get; set; }
        public Nullable<decimal> TcAnterior { get; set; }
        [ForeignKey("Clave")]
        public virtual tblC_SC_CatMoneda Divisa { get; set; }
    }
}
