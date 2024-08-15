using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.MAZDA
{
    public class tblMAZ_Usuario_Cuadrilla
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string correo { get; set; }
        public string nombreUsuario { get; set; }
        public string contrasena { get; set; }
        public int cuadrillaID { get; set; }
        //public int actividadPeriodoID { get; set; }
        public int nivel { get; set; }
        public int orden { get; set; }
        public bool estatus { get; set; }
    }
}
