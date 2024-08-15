using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Empl_Baja
    {
        public int id { get; set; }
        public int clave_empleado { get; set; }
        public DateTime? fecha_baja { get; set; }
        public int? motivo_baja { get; set; }
        public string otros_motivos { get; set; }
        public string comentarios { get; set; }
        public int? clave_solicita { get; set; }
        public int? clave_autoriza { get; set; }
        public string estatus { get; set; }
        public DateTime? fecha_solicitud { get; set; }
        public DateTime? fecha_autoriza { get; set; }
        public int? usuario_autoriza_conta { get; set; }
        public int? usuario_autoriza_inventario { get; set; }
        public bool? archivo_enviado { get; set; }
        public string estatus_inventario { get; set; }
        public string estatus_contabilidad { get; set; }
        public DateTime? fecha_autoriza_conta { get; set; }
        public DateTime? fecha_autoriza_inventario { get; set; }
        public decimal? adeudo_inventario { get; set; }
        public string comentarios_inventario { get; set; }
        public string comentarios_conta { get; set; }
        public string observaciones { get; set; }
        public bool? conflicto_laboral { get; set; }
        public DateTime? fecha_antiguedad { get; set; }
        public int? almacen { get; set; }
        public int? numero { get; set; }
        public string cc_contable { get; set; }
        public int? requisicion { get; set; }
        public string estatus_compras { get; set; }
        public int? usuario_autoriza_compras { get; set; }
        public DateTime? fecha_autoriza_compras { get; set; }
        public string comentarios_compras { get; set; }
        public string st_impreso { get; set; }
    }
}
