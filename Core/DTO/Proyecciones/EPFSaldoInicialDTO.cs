using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Proyecciones
{
    public class EPFSaldoInicialDTO
    {
        public string Concepto { get; set; }
        public decimal Inicial { get; set; }
        public string Grupo { get; set; }
        public decimal D1 { get; set; }
        public decimal D2 { get; set; }
        public decimal D3 { get; set; }
        public decimal H1 { get; set; }
        public decimal H2 { get; set; }
        public decimal H3 { get; set; }
        public decimal Saldo { get; set; }
    }
}
