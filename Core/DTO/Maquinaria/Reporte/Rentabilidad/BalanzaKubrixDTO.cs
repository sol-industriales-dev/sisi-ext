using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.Rentabilidad
{
    public class BalanzaKubrixDTO
    {
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string descripcion { get; set; }
        public decimal saldoInicial { get; set; }
        public decimal cargos { get; set; }
        public decimal abonos { get; set; }
        public decimal saldoActual { get; set; }
    }
}
