using Core.Enum.Administracion.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class AdministracionInstructoresDTO
    {
        public int status { get; set; }
        public string messaje { get; set; }

        public int id { get; set; }
        public string cveEmpleado { get; set; }
        public int grupo { get; set; }
        public DateTime? fechaInicio { get; set; }

        public string nombreCompleto { get; set; }
        public string ApeidoP { get; set; }
        public string ApeidoM { get; set; }
        public string nombreGrupo { get; set; }
        public int cantDiasTrabajados { get; set; }
        public int cantDiasDescansados { get; set; }
        public int cantDiasTrabajados2 { get; set; }
        public int cantDiasDescansados2 { get; set; }
        public TematicaCursoEnum tematica { get; set; }
        public bool instructor { get; set; }


        public string clave_empleado { get; set; }
        public string cc { get; set; }
        public string descripcion { get; set; }
        public string empresa { get; set; }
        public bool mixto { get; set; }
    }
}
