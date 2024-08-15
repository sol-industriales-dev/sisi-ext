using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Mural
{
    public class tblMural_Data
    {
        public int id { get; set; }
        public string datos { get; set; }
        public string titulo { get; set; }
        public int usuarioID { get; set; }
        public DateTime fecha { get; set; }
    }
}
