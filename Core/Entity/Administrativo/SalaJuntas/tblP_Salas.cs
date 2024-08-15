using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.SalaJuntas
{
    public class tblP_Salas
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int capacidad { get; set; }
        public string correo { get; set; }
        public bool estatus { get; set; }


    }
}
