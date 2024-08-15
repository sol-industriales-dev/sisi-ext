using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class PlantillaInformeDTO
    {
        public int id { get; set; }
        public int division_id { get; set; }
        public string division_desc { get; set; }
        public int cantidadDiapositivas { get; set; }
        public bool estatus { get; set; }
    }
}
