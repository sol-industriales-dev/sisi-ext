using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class tblAutorizacionesPpalDTO
    {
        public string nombreCurso { get; set; }
        public string nombreInstructor { get; set; }
        public string fechaCurso { get; set; }
        public int capacitacionID { get; set; }
        public string claveCurso { get; set; }
        public string centroCostos { get; set; }
    }
}
