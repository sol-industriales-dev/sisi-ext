using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Facultamiento
{
    public class FacultamientoDTO
    {
        public int orden { get; set; }
        public int FacultamientoID { get; set; }
        public string Titulo { get; set; }
        public List<EmpleadoFaDTO> ListaEmpleados { get; set; }
        public bool Aplica { get; set; }
        public int PlantillaID { get; set; }
        public string Fecha { get; set; }
        public string Obra { get; set; }
        public string CentroCostos { get; set; }
        public string Departamento { get; set; }

    }
}
