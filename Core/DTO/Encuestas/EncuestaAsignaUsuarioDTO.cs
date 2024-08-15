using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas
{
    public class EncuestaAsignaUsuarioDTO
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public string departamento { get; set; }
        public string descripcion { get; set; }
        public string fecha { get; set; }
        public DateTime fechaValue { get; set; }
        public List<EncuestaCheckUsuarioDTO> usuarios { get; set; }
        public EncuestaAsignaUsuarioDTO()
        {
            usuarios = new List<EncuestaCheckUsuarioDTO>();
        }
    }
}
