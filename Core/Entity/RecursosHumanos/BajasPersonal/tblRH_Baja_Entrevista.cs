using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Bajas
{
    public class tblRH_Baja_Entrevista
    {
        public int id { get; set; }
        public int registroID { get; set; }
        public string cc { get; set; }
        public string cc_nombre { get; set; }
        public int gerente_clave { get; set; }
        public string nombreGerente { get; set; }
        public DateTime? fecha_ingreso { get; set; }
        public DateTime? fecha_salida { get; set; }
        public DateTime? fecha_nacimiento { get; set; }
        public int anios { get; set; }
        public int estado_civil_clave { get; set; }
        public string estado_civil_nombre { get; set; }
        public int escolaridad_clave { get; set; }
        public string escolaridad_nombre { get; set; }
        public int p1_clave { get; set; }
        public string p1_concepto { get; set; }
        public int p2_clave { get; set; }
        public string p2_concepto { get; set; }
        public int p3_1_clave { get; set; }
        public string p3_1_concepto { get; set; }
        public int p3_2_clave { get; set; }
        public string p3_2_concepto { get; set; }
        public int p3_3_clave { get; set; }
        public string p3_3_concepto { get; set; }
        public int p3_4_clave { get; set; }
        public string p3_4_concepto { get; set; }
        public int p3_5_clave { get; set; }
        public string p3_5_concepto { get; set; }
        public int p3_6_clave { get; set; }
        public string p3_6_concepto { get; set; }
        public int p3_7_clave { get; set; }
        public string p3_7_concepto { get; set; }
        public int p3_8_clave { get; set; }
        public string p3_8_concepto { get; set; }
        public int p3_9_clave { get; set; }
        public string p3_9_concepto { get; set; }
        public int p3_10_clave { get; set; }
        public string p3_10_concepto { get; set; }
        public int p4_clave { get; set; }
        public string p4_concepto { get; set; }
        public int p5_clave { get; set; }
        public string p5_concepto { get; set; }
        public string p6_concepto { get; set; }
        public string p7_concepto { get; set; }
        public int p8_clave { get; set; }
        public string p8_concepto { get; set; }
        public string p8_porque { get; set; }
        public int p9_clave { get; set; }
        public string p9_concepto { get; set; }
        public string p9_porque { get; set; }
        public int p10_clave { get; set; }
        public string p10_concepto { get; set; }
        public string p10_porque { get; set; }
        public int p11_1_clave { get; set; }
        public string p11_1_concepto { get; set; }
        public int p11_2_clave { get; set; }
        public string p11_2_concepto { get; set; }
        public int p12_clave { get; set; }
        public string p12_concepto { get; set; }
        public string p12_porque { get; set; }
        public int p13_clave { get; set; }
        public string p13_concepto { get; set; }
        public int p14_clave { get; set; }
        public string p14_concepto { get; set; }
        public DateTime? p14_fecha {get; set; }
        public string p14_porque { get; set; }
        public bool registroActivo { get; set; }
    }
}
