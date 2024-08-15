using Core.Enum.Administracion.Seguridad.Evaluacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Evaluacion
{
    public class PuestoDTO
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public CategoriaPuestoEnum categoria { get; set; }
        public bool estatus { get; set; }

        public string categoriaDesc { get; set; }
    }
}
