using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Facultamiento
{
    public class tblFa_CatFacultamiento
    {
        public int id { get; set; }
        public string obra { get; set; }
        public string cc { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fechaRegistro { get; set; }
        public int usuarioID { get; set; }
        public int estatus { get; set; }
    }
}
