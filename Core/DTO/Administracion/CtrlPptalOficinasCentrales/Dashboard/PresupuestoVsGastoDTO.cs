using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales.Dashboard
{
    public class PresupuestoVsGastoDTO
    {
        public int idCC { get; set; }
        public int year { get; set; }
        public int mes { get; set; }
        public int idAgrupador { get; set; }
        public bool esAgrupador { get; set; }
        public int? idConcepto { get; set; }
        public string descripcion { get; set; }
        public decimal presupuestoMensual { get; set; }
        public decimal gastoMensual { get; set; }
        public decimal cumplimientoMensual { get; set; }
        public decimal presupuestoAcumulado { get; set; }
        public decimal gastoAcumulado { get; set; }
        public decimal cumplimientoAcumulado { get; set; }
        public int empresa { get; set; }
        public string title { get; set; }

        #region ADICIONAL
        public int agrupacionID { get; set; }
        public string agrupacion { get; set; }
        public int conceptoID { get; set; }
        public string concepto { get; set; }
        public decimal importeEnero { get; set; }
        public decimal importeFebrero { get; set; }
        public decimal importeMarzo { get; set; }
        public decimal importeAbril { get; set; }
        public decimal importeMayo { get; set; }
        public decimal importeJunio { get; set; }
        public decimal importeJulio { get; set; }
        public decimal importeAgosto { get; set; }
        public decimal importeSeptiembre { get; set; }
        public decimal importeOctubre { get; set; }
        public decimal importeNoviembre { get; set; }
        public decimal importeDiciembre { get; set; }
        public decimal pptoMensual { get; set; }
        public decimal diferenciaMensual { get; set; }
        public decimal pptoAcumulado { get; set; }
        public decimal diferenciaAcumulado { get; set; }
        public decimal gastoAnioPasado { get; set; }
        public decimal diferenciaAnioActualVsAnioAcumulado { get; set; }
        public int idCaptura { get; set; }
        public int conceptoCuentaID { get; set; }
        public string idConceptoString { get; set; }
        #endregion
    }
}
