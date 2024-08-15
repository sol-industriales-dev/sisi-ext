using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CatHorasHombre
{
    public class CatHorasHombreDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int clave_depto { get; set; }
        public int idGrupo { get; set; }
        public DateTime fechaInicio { get; set; }
        public int horas { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
