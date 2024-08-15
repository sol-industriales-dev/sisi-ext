using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoDepreciacionDTO
    {
        public int Cuenta { get; set; }
        public string Concepto { get; set; }
        public decimal DepreciacionAnterior { get; set; }
        public decimal DepreciacionAcumulada { get; set; }
        public decimal DepreciacionRegistrada { get; set; }
        public decimal Diferencia
        {
            get 
            {
                if ((DepreciacionAcumulada - DepreciacionRegistrada) < 0.009M && (DepreciacionAcumulada - DepreciacionRegistrada) > -0.009M)
                {
                    return 0.00M;
                }
                else
                {
                    return Math.Abs(DepreciacionAcumulada - DepreciacionRegistrada);
                }
            } 
        }
    }
}