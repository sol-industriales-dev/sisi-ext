using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta
{
    public class ReservaDTO
    {
        public int id { get; set; }
        public int tipo { get; set; }
        public DateTime fecha { get; set; }
        public string cc { get; set; }
        public decimal anterior { get; set; }
        public decimal cargo { get; set; }
        public decimal abono { get; set; }
        public decimal porcentaje { get; set; }
        public decimal global { get; set; }
        public bool esValida()
        {
            var esValida = cargo != 0 || abono != 0;
            if(string.IsNullOrEmpty(cc) && cc.Length.Equals(3))
                esValida = false;
            if(tipo <= 0)
                esValida = false;
            if(fecha.Equals(default(DateTime)))
                esValida = false;
            return esValida;
        }
    }
}
