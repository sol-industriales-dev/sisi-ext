using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class UsuarioEnkontrolDTO
    {
        public int empleado { get; set; }
        public string descripcion { get; set; }
        public int puesto { get; set; }
        public string telefono { get; set; }
        public int? almacen { get; set; }
        public string password { get; set; }
        public decimal? monto { get; set; }
        public decimal? monto_inicial { get; set; }
        public decimal? vobo_monto_inicial { get; set; }
        public decimal? vobo_monto_final { get; set; }
        public string vobo { get; set; }
        public string solicito { get; set; }
        public int? requisita_tmc { get; set; }
        public int? autoriza_req_tmc { get; set; }
        public int? vobo_tmc { get; set; }
        public int? autoriza_tmc { get; set; }
        public decimal? monto_ini_tmc { get; set; }
        public decimal? monto_fin_tmc { get; set; }
        public int autoriza_activos_fijos { get; set; }
        public string nom_empleado { get; set; }
        public string ap_paterno_empleado { get; set; }
        public string ap_materno_empleado { get; set; }
        public string rfc_empleado { get; set; }
        public int venta_activos_fijos { get; set; }
        public int autoriza_cancelar_facturas { get; set; }
        public int autoriza_notas_credito { get; set; }
        public int autoriza_factura_proveedores { get; set; }
        public int autoriza_baja_inventarios { get; set; }
        public int autoriza_baja_contabilidad { get; set; }
    }
}
