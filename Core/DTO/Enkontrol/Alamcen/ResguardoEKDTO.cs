using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class ResguardoEKDTO
    {
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int folio { get; set; }

        //id_activo == insumo
        public int id_activo { get; set; }
        public string insumoDesc { get; set; }

        public int id_tipo_activo { get; set; }
        public string tipo_activoDesc { get; set; }
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
        public int empleado { get; set; }
        public int? empleadoSIGOPLAN { get; set; }
        public string empleadoNombre { get; set; }
        public string claveEmpleado { get; set; }
        public string licencia { get; set; }
        public string tipo { get; set; }
        public DateTime? fec_licencia { get; set; }
        public string observaciones { get; set; }
        public DateTime fec_resguardo { get; set; }
        public string fec_resguardoString { get; set; }
        public string foto { get; set; }
        public string estatus { get; set; }
        public int entrega { get; set; }
        public int? entregaSIGOPLAN { get; set; }
        public string entregaNombre { get; set; }
        public int autoriza { get; set; }
        public int? autorizaSIGOPLAN { get; set; }
        public string autorizaNombre { get; set; }
        public int? recibio { get; set; }
        public int? recibioSIGOPLAN { get; set; }
        public string recibioNombre { get; set; }
        public string condiciones_ret { get; set; }
        public DateTime? fec_devolucion { get; set; }
        public string fec_devolucionString { get; set; }
        public decimal cantidad_resguardo { get; set; }
        public string cantidad_resguardoDesc { get; set; }
        public int alm_salida { get; set; }
        public string alm_salidaDesc { get; set; }
        public int alm_entrada { get; set; }
        public string foto_2 { get; set; }
        public string foto_3 { get; set; }
        public string foto_4 { get; set; }
        public string foto_5 { get; set; }
        public decimal? costo_promedio { get; set; }
        public string costo_promedioDesc { get; set; }
        public decimal? resguardo_parcial { get; set; }
        public decimal cantidad_resguardoResultado { get; set; }

        public string condBuenas { get; set; }
        public string condRegulares { get; set; }
        public string condMalas { get; set; }

        public string area_alm { get; set; }
        public string lado_alm { get; set; }
        public string estante_alm { get; set; }
        public string nivel_alm { get; set; }

        public bool parcial { get; set; }
    }
}
