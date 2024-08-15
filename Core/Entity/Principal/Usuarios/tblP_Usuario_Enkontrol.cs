using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Usuarios
{
    public class tblP_Usuario_Enkontrol
    {
        public int id { get; set; }
        /// <summary>
        /// ID Usuario de Sigoplan
        /// </summary>
        public int idUsuario { get; set; }
        /// <summary>
        /// numero de empleado de la tabla empleados en las BD EK_ADM01 y EK_ADM04
        /// </summary>
        public int empleado { get; set; }
        /// <summary>
        /// cve de empleado de la tabla sn_empleado en las BD EK_NOM11_9 y EK_NOM11_9
        /// </summary>
        public int sn_empleado { get; set; }
        public string password { get; set; }
    }
}
