using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales.Dashboard
{
    public class MovPolControlPptalDTO
    {
        public int year { get; set; }
        public int mes { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string cc { get; set; }
        public decimal monto { get; set; }
        public int area { get; set; }
        public int cuenta_oc { get; set; }
        public int empresa { get; set; }
    }
}
