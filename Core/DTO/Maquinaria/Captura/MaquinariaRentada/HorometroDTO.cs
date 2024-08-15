using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.MaquinariaRentada
{
    public class HorometroDTO
    {
        public int Id { get; set; }
        public string AreaCuenta { get; set; }
        public string CentroCosto { get; set; }
        public decimal HorometroInicial { get; set; }
        public decimal? HorometroFinal { get; set; }
        public DateTime Fecha { get; set; }
    }
}