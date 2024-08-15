using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Facultamiento
{
    public class CatalogoPlantillaFaDTO
    {
        public int orden { get; set; }
        public int PlantillaID { get; set; }
        public string Titulo { get; set; }
        public string Departamento { get; set; }
        public string Fecha { get; set; }
        
    }
}
