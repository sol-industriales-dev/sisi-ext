using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.MaquinariaRentada
{
    public class CentroCostoDTO
    {
        public int Id { get; set; }
        public string Cc { get; set; }
        public string Equipo { get; set; }
        public string NumeroSerie { get; set; }
        public int IdModelo { get; set; }
        public string Modelo { get; set; }
        public string Proveedor { get; set; }
        public string AreaCuenta { get; set; }
        public string NumeroAreaCuenta { get; set; }
        public decimal HorometroInicial { get; set; }
    }
}