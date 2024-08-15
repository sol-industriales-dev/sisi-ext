using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Rentabilidad
{
    public class ReporteSemanalKubrixDTO
    {
        public string cc { get; set; }
        public string areaCuenta { get; set; }
        public string nombre { get; set; }
        public decimal acumulado1 { get; set; }
        public decimal acumulado2 { get; set; }
        public decimal acumulado3 { get; set; }
        public decimal acumulado4 { get; set; }
        public decimal acumulado5 { get; set; }
        public decimal acumulado6 { get; set; }
        public decimal semana1 { get; set; }
        public decimal semana2 { get; set; }
        public decimal semana3 { get; set; }
        public decimal semana4 { get; set; }
        public decimal semana5 { get; set; }
    }
}
