using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Plantilla
{
    public class PlantillaPersonal2DTO
    {
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public int plantillaID { get; set; }
        public int id { get; set; }
        public string puesto { get; set; }
        public string departamento { get; set; }
        public string nomina { get; set; }
        public int nominaID { get; set; }
        public int personalOriginal { get; set; }
        public int personalActual { get; set; }
        public string sueldoBase { get; set; }
        public string sueldoComplemento { get; set; }
        public string sueldoTotal { get; set; }
        public string reporte { get; set; }
        public string sueldoMensual { get; set; }
    }
}
