using Core.DTO.Maquinaria.Reporte.ActivoFijo.Colombia.Cedula;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoResumenCedulaDTO
    {
        public List<ActivoFijoSaldosDTO> Saldos { get; set; }
        public List<ActivoFijoDepreciacionDTO> Depreciaciones { get; set; }
        
        public decimal SaldosSuma_SaldoAnterior { get; set; }
        public decimal SaldosSuma_Altas { get; set; }
        public decimal SaldosSuma_Bajas { get; set; }
        public decimal SaldosSuma_SaldoActual { get; set; }
        public decimal SaldosSuma_Contabildiad { get; set; }
        public decimal SaldosSuma_Diferencia { get; set; }

        public decimal SumaDepreciacion_Anterior { get; set; }
        public decimal SumaDepreciacion_Acumulada { get; set; }
        public decimal SumaDepreciacion_Registrada { get; set; }
        public decimal SumaDepreciacion_Diferencia { get; set; }

        #region colombia
        public List<CedulaSaldoColombiaDTO> CedulaSaldoColombia { get; set; }
        public List<CedulaDepColombiaDTO> CedulaDepColombia { get; set; }
        #endregion

        public ActivoFijoResumenCedulaDTO(List<ActivoFijoSaldosDTO> saldos, List<ActivoFijoDepreciacionDTO> depreciacion)
        {
            foreach (var cuenta in saldos)
            {
                Saldos = saldos;
                SaldosSuma_SaldoAnterior += cuenta.SaldoAnterior;
                SaldosSuma_Altas += cuenta.Altas;
                SaldosSuma_Bajas += cuenta.Bajas;
                SaldosSuma_SaldoActual += cuenta.SaldoActual;
                SaldosSuma_Contabildiad += cuenta.Contabilidad;
                SaldosSuma_Diferencia += cuenta.Diferencia;
            }

            foreach (var cuenta in depreciacion)
            {
                Depreciaciones = depreciacion;
                SumaDepreciacion_Anterior += cuenta.DepreciacionAnterior;
                SumaDepreciacion_Acumulada += cuenta.DepreciacionAcumulada;
                SumaDepreciacion_Registrada += cuenta.DepreciacionRegistrada;
                SumaDepreciacion_Diferencia += cuenta.Diferencia;
            }
        }
    }
}