using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Cheques
{
    public class tblC_sb_cuenta
    {
        public int id { get; set; }
        public int cuenta { get; set; }
        public string descripcion { get; set; }
        public int banco { get; set; }
        public string moneda { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string tp { get; set; }
        public int ultimo_cheque { get; set; }
        public int salini { get; set; }
        public DateTime fecha_saldo { get; set; }
        public string desc1 { get; set; }
        public int ult_cheq_electronico { get; set; }
        public int cve_sucursal { get; set; }
        public int num_cta_banco { get; set; }
        public string lugar_emision { get; set; }
        public string clabe { get; set; }
        public string tercero { get; set; }
        public string cc { get; set; }
        public int banca_electronica { get; set; }
        public char bit_tc { get; set; }
        public string sublibro_erp { get; set; }
    }
}
