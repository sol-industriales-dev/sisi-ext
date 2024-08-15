using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Rentabilidad
{
    public class tblM_KB_CorteDet
    {
        public int id { get; set; }
        public int corteID { get; set; }
        public int year { get; set; }
        public int mes { get; set; }
        public string tp { get; set; }
        public int poliza { get; set; }
        public int linea { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string concepto { get; set; }
        public decimal monto { get; set; }
        public string cc { get; set; }
        public string ccSIGOPLAN { get; set; }
        public string acSIGOPLAN { get; set; }
        public int divisionID { get; set; }
        public DateTime fechapol { get; set; }
        public int tipoEquipo { get; set; }
        public string referencia { get; set; }
        public int conciliacionID { get; set; }
        public int empresa { get; set; }
        public bool esEstimado { get; set; }
        public int usuarioCaptura { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool esCancelacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
