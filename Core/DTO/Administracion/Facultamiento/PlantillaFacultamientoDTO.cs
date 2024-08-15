using Core.Entity.Administrativo.FacultamientosDpto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Facultamiento
{
    public class PlantillaFacultamientoDTO
    {
        public List<ConceptoDTO> ListaConceptos { get; set; }
        public List<int> ListaDepartamentos { get; set; }
        public string Titulo { get; set; }
        public int PlantillaID { get; set; }
        public int orden { get; set; }
    }
}
