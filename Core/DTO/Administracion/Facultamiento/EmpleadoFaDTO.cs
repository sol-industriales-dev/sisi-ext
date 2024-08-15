using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Facultamiento
{
    public class EmpleadoFaDTO
    {
        public int EmpleadoID { get; set; }
        public int ConceptoID { get; set; }
        public string NombreEmpleado { get; set; }
        public int? ClaveEmpleado { get; set; }
        public int FacultamientoID { get; set; }
        public bool Editado { get; set; }
        public bool EsActivo { get; set; }
        public string Concepto { get; set; }
        public bool EsAutorizante { get; set; }
        public bool Aplica { get; set; }
    }
}
