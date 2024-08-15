using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Facultamiento
{
    public class tblFa_CatAuth
    {
        public int id { get; set; }
        public int idFacultamiento { get; set; }
        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public int orden { get; set; }
        public DateTime fechaFirma { get; set; }
        public string firma { get; set; }
        public bool auth { get; set; }
        public bool esRechazado { get; set; }
        public string motivoRechazo { get; set; }
    }
}
