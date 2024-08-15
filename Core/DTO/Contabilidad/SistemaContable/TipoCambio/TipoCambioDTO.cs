using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.Moneda;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.SistemaContable.TipoCambio
{
    public class TipoCambioDTO //: tblC_SC_TipoCambio
    {
        public int Id { get; set; }
        public int Moneda { get; set; }
        public DateTime Fecha { get; set; }
        public decimal TipoCambio { get; set; }
        public int Empleado_modifica { get; set; }
        public DateTime FechaModifica { get; set; }
        public DateTime HoraModifica { get; set; }
        public Nullable<decimal> TcAnterior { get; set; }
        public string Empleado_modifica_nombre { get; set; }
        public tblC_SC_CatMoneda Divisa { get; set; }
        public bool esGuardadoSigoplan { get; set; }
        public bool esGuardadoEnkontrol { get; set; }
    }
}
