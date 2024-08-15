using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.Entity.SubContratistas.Usuarios
{
    public class tblP_Usuarios
    {
        public int id { get; set; }
        public string _user { get; set; }
        public string _pass { get; set; }
        public string nombre_completo { get; set; }
        public int tipo { get; set; }
        public bool estatus { get; set; }
        public string correo { get; set; }
        public int idPadre { get; set; }
        public string cc { get; set; }
        public int idContrato { get; set; }
    }
}
