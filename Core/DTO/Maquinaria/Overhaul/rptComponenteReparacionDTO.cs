using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class rptComponenteReparacionDTO
    {
        
       /// <summary>
       /// Serie
       /// </summary>
        public string noComponente { get; set; }
        /// <summary>
        /// Descripión
        /// </summary>
        public string subconjunto { get; set; }

        /// <summary>
        /// NoEconomico
        /// </summary>
        public string noEconomico { get; set; }

        /// <summary>
        /// obra
        /// </summary>
        public string cc { get; set; }
        /// <summary>
        /// proveedor
        /// </summary>
        public string locacion { get; set; }
        /// <summary>
        /// cotizacion
        /// </summary>
        public string cotizacion { get; set; }
        /// <summary>
        /// costoReparacion
        /// </summary>
        public string costo { get; set; }
        /// <summary>
        /// costoPromReparacion 
        /// </summary>
        public string costoPromedio { get; set; }
        

    }
}
