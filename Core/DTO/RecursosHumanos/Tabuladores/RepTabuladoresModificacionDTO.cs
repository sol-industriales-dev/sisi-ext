using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Tabuladores
{
    public class RepTabuladoresModificacionDTO
    {
        public string id { get; set; }
        public string nombreEmpleado { get; set; }
        public string puesto { get; set; }
        public string categoria { get; set; }
        public string lineaNegocios { get; set; }

        public string sueldoBase_Anterior { get; set; }
        public string complemento_Anterior { get; set; }
        public string totalNominal_Anterior { get; set; }
        public string totalMensual_Anterior { get; set; }

        public string sueldoBase_Actual { get; set; }
        public string complemento_Actual { get; set; }
        public string totalNominal_Actual { get; set; }
        public string totalMensual_Actual { get; set; }

        public string nomina { get; set; }
        public string personal { get; set; }
        public string descAreaDepartamento { get; set; }
        public string descSindicato { get; set; }
        public string esquemaPagoDesc { get; set; }
    }
}