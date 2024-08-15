using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoDepMaquinaResumenDTO
    {
        public List<ActivoFijoDepMaquinaDTO> Depreciaciones { get; set; }

        public decimal SumaMOI { get; set; }
        public decimal SumaSemanal { get; set; }
        public decimal SumaAcumulada { get; set; }
        public decimal SumaMensual { get; set; }
        public decimal SumaLibros { get; set; }
        public decimal SumaOH14_1 { get; set; }

        public ActivoFijoDepMaquinaResumenDTO(List<ActivoFijoDepMaquinaDTO> depreciaciones)
        {
            Depreciaciones = depreciaciones;

            foreach (var activo in depreciaciones)
            {
                SumaMOI += activo.Monto;
                SumaSemanal += activo.DepreciacionSemanal;
                SumaAcumulada += activo.DepreciacionAcumulada;
                SumaMensual += activo.DepreciacionMensual;
                SumaLibros += activo.ValorLibro;
                SumaOH14_1 += activo.depreciacionOH_14_1;
            }
        }
    }
}