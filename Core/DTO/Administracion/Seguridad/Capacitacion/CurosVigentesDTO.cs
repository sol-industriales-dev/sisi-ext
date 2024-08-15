using Core.Entity.Principal.Usuarios;
using Core.Enum.Administracion.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class CurosVigentesDTO
    {
        public int id { get; set; }
        public string claveCurso { get; set; }
        public string nombre { get; set; }
        public ClasificacionCursoEnum clasificacionEnum { get; set; }
        public string clasificacion { get; set; }
        public string lugar { get; set; }
        public string instructor { get; set; }
        public string fechaCapacitacion { get; set; }
        public string fechaVigencia { get; set; }
        public tblP_Usuario usuarioInstructor { get; set; }
        public bool esExterno { get; set; }
        public string externo { get; set; }
        public DateTime fechaCapacitacionDate { get; set; }
        public DateTime fechaVigenciaDate { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
    }
}
