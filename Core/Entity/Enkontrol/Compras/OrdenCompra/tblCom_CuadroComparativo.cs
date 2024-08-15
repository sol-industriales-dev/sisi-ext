using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblCom_CuadroComparativo
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int numero { get; set; }
        public int folio { get; set; }
        public DateTime fecha { get; set; }
        public int prov1 { get; set; }
        public decimal porcent_dcto1 { get; set; }
        public decimal porcent_iva1 { get; set; }
        public decimal dcto1 { get; set; }
        public decimal iva1 { get; set; }
        public decimal total1 { get; set; }
        public decimal tipo_cambio1 { get; set; }
        public DateTime fecha_entrega1 { get; set; }
        public int lab1 { get; set; }
        public int dias_pago1 { get; set; }
        public int prov2 { get; set; }
        public decimal porcent_dcto2 { get; set; }
        public decimal porcent_iva2 { get; set; }
        public decimal dcto2 { get; set; }
        public decimal iva2 { get; set; }
        public decimal total2 { get; set; }
        public decimal tipo_cambio2 { get; set; }
        public DateTime fecha_entrega2 { get; set; }
        public int lab2 { get; set; }
        public int dias_pago2 { get; set; }
        public int prov3 { get; set; }
        public decimal porcent_dcto3 { get; set; }
        public decimal porcent_iva3 { get; set; }
        public decimal dcto3 { get; set; }
        public decimal iva3 { get; set; }
        public decimal total3 { get; set; }
        public decimal tipo_cambio3 { get; set; }
        public DateTime fecha_entrega3 { get; set; }
        public int lab3 { get; set; }
        public int dias_pago3 { get; set; }
        public int solicito { get; set; }
        public decimal sub_total1 { get; set; }
        public decimal sub_total2 { get; set; }
        public decimal sub_total3 { get; set; }
        public decimal fletes1 { get; set; }
        public decimal fletes2 { get; set; }
        public decimal fletes3 { get; set; }
        public decimal gastos_imp1 { get; set; }
        public decimal gastos_imp2 { get; set; }
        public decimal gastos_imp3 { get; set; }
        public string nombre_prov1 { get; set; }
        public string nombre_prov2 { get; set; }
        public string nombre_prov3 { get; set; }
        public int moneda1 { get; set; }
        public int moneda2 { get; set; }
        public int moneda3 { get; set; }
        public bool inslic { get; set; }
        public DateTime? inslic_fecha_ini { get; set; }
        public DateTime? inslic_fecha_fin { get; set; }
        public string comentarios1 { get; set; }
        public string comentarios2 { get; set; }
        public string comentarios3 { get; set; }
        public string rutaArchivo1 { get; set; }
        public string rutaArchivo2 { get; set; }
        public string rutaArchivo3 { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        public string PERU_prov1 { get; set; }
        public string PERU_prov2 { get; set; }
        public string PERU_prov3 { get; set; }
        public string PERU_tipoCuadro { get; set; }
    }
}
