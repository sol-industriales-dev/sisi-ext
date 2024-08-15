using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.MaquinariaRentada
{
    public class RN_Maquinaria_TiempoRequeridoVsUtilizadoDTO
    {
        public string CentroCosto { get; set; }
        public int TiempoRequerido { get; set; }
        public int TiempoUtilizado { get; set; }
        public string AreaCuenta { get; set; }
    }

    public class RN_Maquinaria_PeriodosDTO
    {
        public int IdRenta { get; set; }
        public int IdPeriodoInicial { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFinal { get; set; }
    }
}