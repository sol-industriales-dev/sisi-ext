using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class BusqIncidenciaDTO
    {
        public string cc { get; set; }
        public int tipoNomina { get; set; }
        public int anio { get; set; }
        public int periodo { get; set; }
        public int depto { get; set; }
        public bool isAuth { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public string stfechaInicio { get; set; }
        public string stfechaFin { get; set; }
    }
}
