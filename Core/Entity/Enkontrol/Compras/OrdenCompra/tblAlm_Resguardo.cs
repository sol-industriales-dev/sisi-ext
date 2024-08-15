using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblAlm_Resguardo
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int folio { get; set; }
        public int id_activo { get; set; }
        public int id_tipo_activo { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public string color { get; set; }
        public string num_serie { get; set; }
        public decimal? valor_activo { get; set; }
        public string compania { get; set; }
        public string plan_desc { get; set; }
        public string condiciones { get; set; }
        public int? numpro { get; set; }
        public decimal? factura { get; set; }
        public DateTime? fec_factura { get; set; }
        public int? empleado { get; set; }
        public int? empleadoSIGOPLAN { get; set; }
        public string claveEmpleado { get; set; }
        public string licencia { get; set; }
        public string tipo { get; set; }
        public DateTime? fec_licencia { get; set; }
        public string observaciones { get; set; }
        public DateTime fec_resguardo { get; set; }
        public string foto { get; set; }
        public string estatus { get; set; }
        public int? entrega { get; set; }
        public int? entregaSIGOPLAN { get; set; }
        public int? autoriza { get; set; }
        public int? autorizaSIGOPLAN { get; set; }
        public int? recibio { get; set; }
        public int? recibioSIGOPLAN { get; set; }
        public string condiciones_ret { get; set; }
        public DateTime? fec_devolucion { get; set; }
        public decimal cantidad_resguardo { get; set; }
        public int alm_salida { get; set; }
        public int alm_entrada { get; set; }
        public string foto_2 { get; set; }
        public string foto_3 { get; set; }
        public string foto_4 { get; set; }
        public string foto_5 { get; set; }
        public decimal? costo_promedio { get; set; }
        public decimal? resguardo_parcial { get; set; }
        public bool estatusRegistro { get; set; }
    }
}
