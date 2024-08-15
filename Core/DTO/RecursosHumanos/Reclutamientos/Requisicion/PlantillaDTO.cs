using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos.Requisicion
{
    public class PlantillaDTO
    {
        public int id_plantilla { get; set; }
        public string cc { get; set; }
        public string ccDescripcion { get; set; }
        public int puesto { get; set; }
        public string puestoDescripcion { get; set; }
        public int solicitados { get; set; }
        public int faltantes { get; set; }
    }
}
