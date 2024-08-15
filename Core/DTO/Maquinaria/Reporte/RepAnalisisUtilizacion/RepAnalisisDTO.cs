using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.RepAnalisisUtilizacion
{
    public class RepAnalisisDTO
    {
        public string grupo { get; set; }
        public string modelo { get; set; }
        #region Equipo a requerir
        /// <summary>
        /// Requerido según programa
        /// </summary>
        public decimal requerido { get; set; }
        /// <summary>
        /// Equipo adicional
        /// </summary>
        public decimal adicional { get; set; }
        /// <summary>
        /// Existente en obra
        /// </summary>
        public decimal existente { get; set; }
        #endregion
        #region Explosión de insumos
        #region Utilización
        public decimal mesEjecucion { get; set; }
        public decimal horasIniciales { get; set; }
        public decimal horasAdicional { get; set; }
        #endregion
        #region Periodo de utilización
        public DateTime inicio { get; set; }
        public DateTime fin { get; set; }
        #endregion
        #endregion
        #region Utilización ejecutada actual
        public decimal ejecutadaMes { get; set; }
        public decimal ejecutadaHoras { get; set; }
        #endregion
        #region Utilización por ejecutar
        public decimal porEjecutarMes { get; set; }
        public decimal porEjecutarHoras { get; set; }
        #endregion
        /// <summary>
        /// % de utilización
        /// </summary>
        public decimal utilizacion { get; set; }
        public string comentarios { get; set; }
    }
}
