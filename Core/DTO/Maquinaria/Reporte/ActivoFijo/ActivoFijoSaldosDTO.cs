using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoSaldosDTO
    {
        public int Cuenta { get; set; }
        public string Concepto { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal Altas { get; set; }
        public decimal Bajas { get; set; }
        public decimal SaldoActual { get { return (SaldoAnterior + Altas) - Bajas; } }
        public decimal Contabilidad { get; set; }
        public decimal Diferencia
        {
            get
            {
                return Math.Abs(SaldoActual - Contabilidad);
                //if ((SaldoActual - Contabilidad) > -0.009M && (SaldoActual - Contabilidad) < 0.009M)
                //{
                //    return 0.00M;
                //}
                //else
                //{
                //    return Math.Abs(SaldoActual - Contabilidad);
                //}
            }
        }
    }
}