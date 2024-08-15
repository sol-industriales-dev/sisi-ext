using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class CatFormatoCambioDTO
    {
        public tblRH_FormatoCambio objEmpleadoInfo { get; set; }
        public tblRH_FormatoCambio objFormatoCambio { get; set; }
        public List<tblRH_AutorizacionFormatoCambio> objAutorizacion { get; set; }
    }
}
