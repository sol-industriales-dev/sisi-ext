using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Rentabilidad
{
    public class ClientesKubrixDTO
    {							
        public int numcte { get; set; }
        public string nombre { get; set; }
        public double factura { get; set; }
        public int tm { get; set; }
        public DateTime fecha { get; set; }
        public DateTime vencimiento { get; set; }
        public int id_segmento { get; set; }
        public string desc_segmento { get; set; }
        public int tipo_cliente { get; set; }
        public string desc_tipo_cliente { get; set; }
        public int moneda { get; set; }
        public decimal tipocambio { get; set; }
        public string desc_moneda { get; set; }
        public string cc { get; set; }
        public string desc_cc { get; set; }
        public string corto { get; set; }
        public DateTime corte { get; set; }
        public int plazo1 { get; set; } 
        public int plazo2 { get; set; }
        public int plazo3 { get; set; }
        public DateTime dif1 { get; set; }
        public DateTime dif2 { get; set; }
        public DateTime dif3 { get; set; }
        public int movs_x_factura { get; set; }
        public decimal sum_venc_x_fact { get; set; }
        public decimal sum_por_vencer { get; set; }
        public decimal sum_vencido_1 { get; set; }
        public decimal sum_vencido_2 { get; set; }
        public decimal sum_vencido_3 { get; set; }
        public decimal sum_vencido_4 { get; set; }
        public decimal sum_vencido { get; set; }
        public decimal saldo_x_factura { get; set; }
        public decimal monto_factura { get; set; }
        public int fact_abono_no_apli { get; set; }
        public decimal monto_fact_abono_no_apli { get; set; }
        public DateTime fecha_max { get; set; }
        public decimal ultimo_tipocambio { get; set; }
        public string descrip_tm { get; set; }
        public string concep_mov { get; set; }
        public string ls_telefono { get; set; }
    }
}
