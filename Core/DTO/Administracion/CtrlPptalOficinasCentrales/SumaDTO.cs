using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class SumaDTO
    {
        public int mes { get; set; }
        public decimal suma { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string cc { get; set; }
        public decimal monto { get; set; }
        public int idConcepto { get; set; }
    }
}
