using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblCom_Auth
    {
        public int id { get; set; }
        public bool isActivo { get; set; }
        /// <summary>
        /// ID orden de compra relacionada
        /// </summary>
        public int idOC { get; set; }
        /// <summary>
        /// Orden de vobos. vobo = 0 autorizante; vobo > 1 vobo
        /// </summary>
        public int vobo { get; set; }
        public int idUsuario { get; set; }
        /// <summary>
        /// Número de empleado de la tabla empleado de enkontrol
        /// </summary>
        public int empleado { get; set; }
        public string cc { get; set; }
        /// <summary>
        /// Numero de orden de compra
        /// </summary>
        public int numero { get; set; }
        public bool isAuth { get; set; }
        public int idTipoAuth { get; set; }
        public DateTime dtAuth { get; set; }
    }
}
