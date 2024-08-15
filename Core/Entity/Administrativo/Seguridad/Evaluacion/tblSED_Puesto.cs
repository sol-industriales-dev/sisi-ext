using Core.Enum.Administracion.Seguridad.Evaluacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Evaluacion
{
    public class tblSED_Puesto
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public CategoriaPuestoEnum categoria { get; set; }
        public int division { get; set; }
        public bool estatus { get; set; }
    }
}
