using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class ConceptosRelCCDTO
    {
        #region DATATABLE
        public int id { get; set; }
        public string cc { get; set; }
        public string concepto { get; set; }
        public decimal pptoMensual { get; set; }
        public decimal gastoMensual { get; set; }
        public decimal diferenciaMensual { get; set; }
        public decimal cumplimientoMensual { get; set; }
        public decimal pptoAcumulado { get; set; }
        public decimal gastoAcumulado { get; set; }
        public decimal diferenciaAcumulado { get; set; }
        public decimal cumplimientoAcumulado { get; set; }
        public bool costosAdministrativos { get; set; }
        #endregion

        #region ADICIONAL
        public List<int> arrConstruplan { get; set; }
        public List<int> arrArrendadora { get; set; }
        public List<int> arrCapturasID { get; set; }
        public int anio { get; set; }
        public int idMes { get; set; }
        public int idAgrupacion { get; set; }
        public int idConcepto { get; set; }
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
        public int idConceptoCuenta { get; set; }
        #endregion


    }
}
