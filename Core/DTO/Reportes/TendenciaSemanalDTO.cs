using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Reportes
{
    public class TendenciaSemanalDTO
    {
        public int cta { get; set; }
        public string cc { get; set; }
        public decimal semana1 { get; set; }
        public decimal semana2 { get; set; }
        public decimal semana3 { get; set; }
        public decimal semana4 { get; set; }
        public decimal semana5 { get; set; }
        public decimal mesPromedio { get; set; }
        public decimal mesReal { get; set; }
    }
}
