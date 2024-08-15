using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Periodos
    {
        public int id { get; set; }
        public int tipo_nomina { get; set; }
        public int tipo_periodo { get; set; }
        public int periodo { get; set; }
        public DateTime fecha_inicial { get; set; }
        public DateTime fecha_final { get; set; }
        public string estatus { get; set; }
        public int mes_cc { get; set; }
        public int year { get; set; }
        public string formula_ispt { get; set; }
        public string prorrateo { get; set; }
        public int subtipo_periodo { get; set; }
        public string transf_estimaciones { get; set; }
        public DateTime ? fecha_pago { get; set; }
    }
}
