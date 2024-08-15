using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad
{
    public class SaldoCCDTO
    {
        public int anio { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public decimal saldoInicial { get; set; }
        public decimal cargosMes { get; set; }
        public decimal abonosMes { get; set; }
        public decimal cargosAcumulados { get; set; }
        public decimal abonosAcumulados { get; set; }
        public string cc { get; set; }
    }
}
