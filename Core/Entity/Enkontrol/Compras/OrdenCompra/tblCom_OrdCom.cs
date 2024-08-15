using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblCom_OrdCom
    {
        #region key
        public int id { get; set; }
        public bool isActivo { get; set; }
        public string cc { get; set; }
        /// <summary>
        /// Numero Orde de compra
        /// </summary>
        public int numero { get; set; }
        #endregion
        #region rel
        /// <summary>
        /// id de requisicion origen
        /// </summary>
        public int idReq { get; set; }
        /// <summary>
        /// Numero de requisicion origen
        /// </summary>
        public int numeroReq { get; set; }
        #endregion
        #region numero empleado
        public int comprador { get; set; }
        public int modEmpl { get; set; }
        public int authUsuario { get; set; }
        public int emplAuth { get; set; }
        #endregion        
        public DateTime fecha { get; set; }
        public int proveedor { get; set; }
        public string estatus { get; set; }
        public string comentarios { get; set; }
        public string embarquese { get; set; }
        public DateTime modFecha { get; set; }        
        public DateTime authFecha { get; set; }
        public string stAuth { get; set; }
        public string printPorcentaje { get; set; }
        public string ST_OC { get; set; }
    }
}
