using Core.DTO.Principal.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta
{
    public class ConcentradoDTO
    {
        public int tipo { get; set; }
        public int tipoReserva { get; set; }
        public int tipoReservaAutomatica { get; set; }
        public DateTime fecha { get; set; }
        public string beneficiario { get; set; }
        public string concepto { get; set; }
        public string cc { get; set; }
        public string obra { get; set; }
        public string noCheque { get; set; }
        public decimal cargo { get; set; }
        public decimal abono { get; set; }
        public decimal saldo { get; set; }
        public int poliza { get; set; }
        public string tp { get; set; }
        public int tm { get; set; }
        public bool sonDolares { get; set; }
        /// <summary>
        /// reservas compatibles
        /// </summary>
        public List<ComboDTO> items { get; set; }
        public bool esValida()
        {
            var esValida = !string.IsNullOrEmpty(cc) && cc.Length.Equals(3);
            if (tipo <= 0)
                esValida = false;
            if (fecha.Equals(default(DateTime)))
                esValida = false;
            if (!esValida)
                return esValida;
            return esValida;
        }
    }
}
